using PamisuKit.Framework;
using UnityEngine;

namespace Game.Farm
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Produce : MonoEntity
    {

        private SpriteRenderer _spriteRenderer;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}