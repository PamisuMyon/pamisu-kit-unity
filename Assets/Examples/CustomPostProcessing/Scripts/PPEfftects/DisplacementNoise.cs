using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP.Effects
{
    [VolumeComponentMenu("Custom Post-processing/Displacement Noise")]
    public class DisplacementNoise : CustomVolumeComponent
    {
        
        public ClampedFloatParameter Intensity = new(0, 0, 1);
        public FloatParameter Speed = new(1);
        public FloatParameter NoiseScale = new(10);

        private Material material;
        
        private const string ShaderName = "Shader Graphs/DisplacementNoise";

        private static readonly int IntensityID = Shader.PropertyToID("_Intensity");
        private static readonly int SpeedID = Shader.PropertyToID("_Speed");
        private static readonly int NoiseScaleID = Shader.PropertyToID("_NoiseScale");

        public override int OrderInPass => 10;

        public override void Setup()
        {
            if (material == null)
                material = CoreUtils.CreateEngineMaterial(ShaderName);
        }

        public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
        {
            if (material == null)
                return;

            material.SetFloat(IntensityID, Intensity.value);
            material.SetFloat(SpeedID, Speed.value);
            material.SetFloat(NoiseScaleID, NoiseScale.value);

            cmd.Blit(source, destination, material, 0);
        }

        public override bool IsActive() => material != null && Intensity.value > 0f;

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CoreUtils.Destroy(material);
        }
        
    }
}