using Pamisu.CustomPP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP.Effects
{
    [VolumeComponentMenu("Custom Post-processing/Grayscale")]
    public class Grayscale : CustomVolumeComponent
    {
        public ClampedFloatParameter blend = new(0, 0, 1);

        private Material material;

        private const string ShaderName = "Hidden/PostProcess/Grayscale";

        public override int OrderInPass => 100;

        public override void Setup()
        {
            if (material == null)
                material = CoreUtils.CreateEngineMaterial(ShaderName);
        }

        public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
        {
            if (material == null)
                return;

            material.SetFloat(Shader.PropertyToID("_Blend"), blend.value);
            // material.SetTexture("_MainTex", source);
            // cmd.SetGlobalTexture(Shader.PropertyToID("_MainTex"), source);
            // CoreUtils.DrawFullScreen(cmd, material, destination);
            cmd.Blit(source, destination, material, 0);
        }

        public override bool IsActive() => material != null && blend.value > 0f;

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CoreUtils.Destroy(material);
        }
    }
}