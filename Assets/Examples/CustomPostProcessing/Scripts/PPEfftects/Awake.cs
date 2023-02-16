using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP.Effects
{
    [VolumeComponentMenu("Custom Post-processing/Awake")]
    public class Awake : CustomVolumeComponent
    {
        public ClampedFloatParameter Progress = new(1f, 0, 1f);
        public ClampedFloatParameter ArchHeight = new(0.2f, 0, 0.5f);
        public ClampedIntParameter BlurIterations = new(3, 0, 4);
        public ClampedFloatParameter BlurSpread = new(.6f, .2f, 3f);

        private Material material;

        private const string ShaderName = "Hidden/PostProcess/Awake";
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        private static readonly int ArchHeightID = Shader.PropertyToID("_ArchHeight");
        private static readonly int BlurSizeID = Shader.PropertyToID("_BlurSize");

        private RenderTargetHandle tempRT0; // 临时RT
        private RenderTargetHandle tempRT1;
        
        public override int OrderInPass => 90;

        public override void Setup()
        {
            if (material == null)
                material = CoreUtils.CreateEngineMaterial(ShaderName);
            tempRT0.Init("_TemporaryRenderTexture0");
            tempRT1.Init("_TemporaryRenderTexture1");
        }

        public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
        {
            if (material == null)
                return;
            
            material.SetFloat(ProgressID, Progress.value);
            material.SetFloat(ArchHeightID, ArchHeight.value);

            if (Progress.value < 1f)
            {
                var descriptor = renderingData.cameraData.cameraTargetDescriptor;
                descriptor.msaaSamples = 1;
                descriptor.depthBufferBits = 0;

                // 初始化临时RT
                RenderTargetIdentifier buffer0, buffer1;
                cmd.GetTemporaryRT(tempRT0.id, descriptor);
                buffer0 = tempRT0.id;
                cmd.Blit(source, buffer0, material, 0);

                // int iterations = Mathf.RoundToInt(blurIterations - blurIterations * progress);
                for (var i = 0; i < BlurIterations.value; i++)
                {
                    // 将progress(0~1)映射到blurSize(blurSize~0)
                    var blurSize = 1f + i * BlurSpread.value;
                    blurSize -= blurSize * Progress.value;
                    material.SetFloat(BlurSizeID, blurSize);

                    cmd.GetTemporaryRT(tempRT1.id, descriptor);
                    buffer1 = tempRT1.id;
                    cmd.Blit(buffer0, buffer1, material, 1);

                    CoreUtils.Swap(ref buffer0, ref buffer1);
                    cmd.Blit(buffer0, buffer1, material, 2);

                    CoreUtils.Swap(ref buffer0, ref buffer1);
                }
                
                cmd.Blit(buffer0, destination);
                cmd.ReleaseTemporaryRT(tempRT0.id);
                if (BlurIterations.value != 0)
                    cmd.ReleaseTemporaryRT(tempRT1.id);     
            }

        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        public override bool IsActive() => material != null && Progress.value != 1f;

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CoreUtils.Destroy(material);
        }
        
    }
}