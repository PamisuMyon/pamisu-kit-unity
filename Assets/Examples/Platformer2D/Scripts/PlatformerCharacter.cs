using Pamisu.GASExtension;
using UnityEngine;

namespace Pamisu.Platformer2D
{
    public enum CharacterOrientation
    {
        Left, Right
    }
    
    public class PlatformerCharacter : AbilitySystemCharacterEx
    {
        [Header("Components")]
        [SerializeField]
        public Transform Cosmetic;
        [SerializeField]
        public SpriteRenderer MainSpriteRenderer;

        public virtual CharacterOrientation Orientation
        {
            get => MainSpriteRenderer.flipX ? CharacterOrientation.Left : CharacterOrientation.Right;
            set => MainSpriteRenderer.flipX = value == CharacterOrientation.Left;
        }
        
        protected void Awake()
        {
            if (Cosmetic == null)
                Cosmetic = transform.Find("Cosmetic");
            if (MainSpriteRenderer == null)
                MainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
    }
}