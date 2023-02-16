using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.Commons
{
    [ExecuteAlways]
    public class AutoLoadPipelineAsset : MonoBehaviour
    {
        public UniversalRenderPipelineAsset pipelineAsset;
    
        private void OnEnable()
        {
            UpdatePipeline();
        }

        void UpdatePipeline()
        {
            if (pipelineAsset)
            {
                QualitySettings.renderPipeline = pipelineAsset;
                GraphicsSettings.renderPipelineAsset = pipelineAsset;
            }
        }
    }

}

