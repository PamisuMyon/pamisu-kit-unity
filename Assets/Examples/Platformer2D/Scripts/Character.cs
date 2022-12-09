using Pamisu.GASExtension;
using UnityEngine;

namespace Pamisu.Platformer2D
{
    public enum CharacterOrientation
    {
        Left, Right
    }
    
    public class Character : AbilitySystemCharacterEx
    {
        [Header("Components")]
        [SerializeField]
        public SpriteRenderer MainSpriteRenderer;

        protected void Awake()
        {
            if (MainSpriteRenderer == null)
                MainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        public void SetOrientation(CharacterOrientation orientation)
        {
            MainSpriteRenderer.flipX = orientation == CharacterOrientation.Left;
        }
        
        public CharacterOrientation GetOrientation(CharacterOrientation orientation)
        {
            return MainSpriteRenderer.flipX ? CharacterOrientation.Left : CharacterOrientation.Right;
        }
        
    }
}