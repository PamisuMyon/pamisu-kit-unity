using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP.Effects
{
    [VolumeComponentMenu("Custom Post-processing/Pixelate")]
    public class Pixelate : CustomVolumeComponent
    {
        public BoolParameter Enable = new(false);
        public IntParameter PixelSize = new(10);

        private Material material;

        private const string ShaderName = "Shader Graphs/Pixelate";
        private static readonly int PixelSizeID = Shader.PropertyToID("_PixelSize");

        public override int OrderInPass => 20;

        public override void Setup()
        {
            if (material == null)
                material = CoreUtils.CreateEngineMaterial(ShaderName);
        }

        public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
        {
            if (material == null)
                return;

            material.SetFloat(PixelSizeID, PixelSize.value);

            cmd.Blit(source, destination, material, 0);
        }

        public override bool IsActive() => material != null && Enable.value && PixelSize.value > 1;

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CoreUtils.Destroy(material);
        }
    }
}