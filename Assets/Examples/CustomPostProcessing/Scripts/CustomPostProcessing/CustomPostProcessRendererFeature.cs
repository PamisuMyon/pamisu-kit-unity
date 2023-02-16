using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP
{

    /// <summary>
    /// Custom Post-processing Renderer Feature
    /// </summary>
    public class CustomPostProcessRendererFeature : ScriptableRendererFeature
    {

        // 不同插入点的render pass
        protected CustomPostProcessRenderPass afterOpaqueAndSky;
        protected CustomPostProcessRenderPass beforePostProcess;
        protected CustomPostProcessRenderPass afterPostProcess;

        // 所有自定义的VolumeComponent
        protected List<CustomVolumeComponent> components;

        // 用于after PostProcess的render target
        protected RenderTargetHandle afterPostProcessTexture;
        protected virtual string AfterPostProcessTextureName => "_CameraColorAttachmentB";

        // 初始化Feature资源，每当序列化发生时都会调用
        public override void Create()
        {
            // 从VolumeManager获取所有自定义的VolumeComponent
            var stack = VolumeManager.instance.stack;
            components = VolumeManager.instance.baseComponentTypeArray
                .Where(t => t.IsSubclassOf(typeof(CustomVolumeComponent)) && stack.GetComponent(t) != null)
                .Select(t => stack.GetComponent(t) as CustomVolumeComponent)
                .ToList();

            // 初始化不同插入点的render pass
            var afterOpaqueAndSkyComponents = components
                .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.AfterOpaqueAndSky)
                .OrderBy(c => c.OrderInPass)
                .ToList();
            afterOpaqueAndSky = new CustomPostProcessRenderPass("Custom PostProcess after Skybox", afterOpaqueAndSkyComponents);
            afterOpaqueAndSky.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

            var beforePostProcessComponents = components
                .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.BeforePostProcess)
                .OrderBy(c => c.OrderInPass)
                .ToList();
            beforePostProcess = new CustomPostProcessRenderPass("Custom PostProcess before PostProcess", beforePostProcessComponents);
            beforePostProcess.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            var afterPostProcessComponents = components
                .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.AfterPostProcess)
                .OrderBy(c => c.OrderInPass)
                .ToList();
            afterPostProcess = new CustomPostProcessRenderPass("Custom PostProcess after PostProcess", afterPostProcessComponents);
            // afterPostProcess.renderPassEvent = RenderPassEvent.AfterRendering;
            afterPostProcess.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

            // 初始化用于after PostProcess的render target
            // afterPostProcessTexture.Init("_AfterPostProcessTexture");
            afterPostProcessTexture.Init(AfterPostProcessTextureName);
        }

        // 你可以在这里将一个或多个render pass注入到renderer中。
        // 当为每个摄影机设置一次渲染器时，将调用此方法。
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.postProcessEnabled)
            {
                // 为每个render pass设置render target
                var source = new RenderTargetHandle(renderer.cameraColorTarget);
                if (afterOpaqueAndSky.SetupComponents())
                {
                    afterOpaqueAndSky.Setup(source, source);
                    renderer.EnqueuePass(afterOpaqueAndSky);
                }
                if (beforePostProcess.SetupComponents())
                {
                    beforePostProcess.Setup(source, source);
                    renderer.EnqueuePass(beforePostProcess);
                }
                if (afterPostProcess.SetupComponents())
                {
                    // 如果下一个Pass是FinalBlit，则输入与输出均为AfterPostProcessTexture
                    source = renderingData.cameraData.resolveFinalTarget ? afterPostProcessTexture : source;
                    afterPostProcess.Setup(source, source);
                    renderer.EnqueuePass(afterPostProcess);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && components != null)
            {
                foreach(var item in components)
                {
                    item.Dispose();
                }
            }
        }

    }

}