using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP
{

    /// <summary>
    /// 自定义后处理Renderer Pass
    /// </summary>
    public class CustomPostProcessRenderPass : ScriptableRenderPass
    {

        private List<CustomVolumeComponent> volumeComponents;   // 所有自定义后处理组件
        private List<int> activeComponents; // 当前可用的组件下标
        private string profilerTag;
        private List<ProfilingSampler> profilingSamplers; // 每个组件对应的ProfilingSampler

        private RenderTargetHandle source;  // 当前源与目标
        private RenderTargetHandle destination;
        private RenderTargetHandle tempRT0; // 临时RT
        private RenderTargetHandle tempRT1;

        /// <param name="profilerTag">Profiler标识</param>
        /// <param name="volumeComponents">属于该RendererPass的后处理组件</param>
        public CustomPostProcessRenderPass(string profilerTag, List<CustomVolumeComponent> volumeComponents)
        {
            this.profilerTag = profilerTag;
            this.volumeComponents = volumeComponents;
            activeComponents = new List<int>(volumeComponents.Count);
            profilingSamplers = volumeComponents.Select(c => new ProfilingSampler(c.ToString())).ToList();

            tempRT0.Init("_TemporaryRenderTexture0");
            tempRT1.Init("_TemporaryRenderTexture1");
        }

        /// <summary>
        /// 设置后处理组件
        /// </summary>
        /// <returns>是否存在有效组件</returns>
        public bool SetupComponents()
        {
            activeComponents.Clear();
            for (int i = 0; i < volumeComponents.Count; i++)
            {
                volumeComponents[i].Setup();
                if (volumeComponents[i].IsActive())
                {
                    activeComponents.Add(i);
                }
            }
            return activeComponents.Count != 0;
        }

        /// <summary>
        /// 设置渲染源和渲染目标
        /// </summary>
        public void Setup(RenderTargetHandle source, RenderTargetHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        // 你可以在这里实现渲染逻辑。
        // 使用<c>ScriptableRenderContext</c>来执行绘图命令或Command Buffer
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // 你不需要手动调用ScriptableRenderContext.submit，渲染管线会在特定位置调用它。
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(profilerTag);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            // 获取Descriptor
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 0;

            // int width = descriptor.width;
            // int height = descriptor.height;
            // cmd.SetGlobalVector("_ScreenSize", new Vector4(width, height, 1.0f / width, 1.0f / height));

            // 初始化临时RT
            RenderTargetIdentifier buff0, buff1;
            bool rt1Used = false;
            
            cmd.GetTemporaryRT(tempRT0.id, descriptor);
            buff0 = tempRT0.id;
            // 如果destination没有初始化，则需要获取RT，主要是destination为AfterPostProcessTexture的情况
            if (destination != RenderTargetHandle.CameraTarget && !destination.HasInternalRenderTargetId())
            {
                cmd.GetTemporaryRT(destination.id, descriptor);
            }

            // 执行每个组件的Render方法
            // 如果只有一个组件，则直接source -> buff0
            if (activeComponents.Count == 1)
            {
                int index = activeComponents[0];
                using (new ProfilingScope(cmd, profilingSamplers[index]))
                {
                    volumeComponents[index].Render(cmd, ref renderingData, source.Identifier(), buff0);
                }
            }
            else
            {
                // 如果有多个组件，则在两个RT上来回blit
                cmd.GetTemporaryRT(tempRT1.id, descriptor);
                buff1 = tempRT1.id;
                rt1Used = true;
                Blit(cmd, source.Identifier(), buff0);
                for (int i = 0; i < activeComponents.Count; i++)
                {
                    int index = activeComponents[i];
                    var component = volumeComponents[index];
                    using (new ProfilingScope(cmd, profilingSamplers[index]))
                    {
                        component.Render(cmd, ref renderingData, buff0, buff1);
                    }
                    CoreUtils.Swap(ref buff0, ref buff1);
                }
            }

            // 最后blit到destination
            Blit(cmd, buff0, destination.Identifier());

            // 释放
            cmd.ReleaseTemporaryRT(tempRT0.id);
            if (rt1Used)
                cmd.ReleaseTemporaryRT(tempRT1.id);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

    }

}