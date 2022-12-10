using DG.Tweening;
using Pamisu.Commons.Pool;
using UnityEngine;

namespace Pamisu.Platformer2D
{
    public class ImpactWave : RecycleOnCondition
    {
        [SerializeField]
        private float animDuration = .6f;
        
        private SpriteRenderer spriteRenderer;
        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play()
        {
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();

            var radius = 0f;
            DOTween.To(() => radius, v =>
            {
                radius = v;
                spriteRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat("Radius", radius);
                spriteRenderer.SetPropertyBlock(propertyBlock);
            }, 1f, animDuration);
            
        }
        
    }
}