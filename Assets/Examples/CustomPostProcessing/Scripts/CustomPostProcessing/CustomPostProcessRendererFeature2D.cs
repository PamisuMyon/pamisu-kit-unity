using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP
{
    public class CustomPostProcessRendererFeature2D : CustomPostProcessRendererFeature
    {
        protected override string AfterPostProcessTextureName => "_CameraColorTexture";
        
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
            // TODO 临时使用BeforeRenderingPostProcessing，由于UberPostProcess中包含_InternalLut与_UserLut相关处理逻辑，直接插入到PostProcess之后而不做这些处理会得到错误的画面
            afterPostProcess.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            // afterPostProcess.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

            // 初始化用于after PostProcess的render target
            // afterPostProcessTexture.Init("_AfterPostProcessTexture");
            afterPostProcessTexture.Init(AfterPostProcessTextureName);
        }
    }
}