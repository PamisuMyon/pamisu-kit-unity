using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP.Effects
{

    [VolumeComponentMenu("Custom Post-processing/Glitch")]
    public class Glitch : CustomVolumeComponent
    {

        public FloatParameter Speed = new(1);
        public Vector2Parameter Intensity = new(Vector2.zero);

        private Material material;

        private const string ShaderName = "Shader Graphs/Glitch";
        private static readonly int SpeedID = Shader.PropertyToID("_Speed");
        private static readonly int IntensityID = Shader.PropertyToID("_Intensity");

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

            material.SetFloat(SpeedID, Speed.value);
            material.SetVector(IntensityID, Intensity.value);

            cmd.Blit(source, destination, material, 0);
        }

        public override bool IsActive() => material != null && (Intensity.value.x > 0f || Intensity.value.y > 0f);

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CoreUtils.Destroy(material);
        }
        
    }
}