using Pamisu.Gameplay.Platformer;
using UnityEngine;

namespace Pamisu.Platformer2D
{
    public class SimplePlatformerCharacter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        public SpriteRenderer MainSpriteRenderer;

        private PlatformerPlayerController2D controller;
        
        public virtual CharacterOrientation Orientation
        {
            get => MainSpriteRenderer.flipX ? CharacterOrientation.Left : CharacterOrientation.Right;
            set => MainSpriteRenderer.flipX = value == CharacterOrientation.Left;
        }
        
        protected void Awake()
        {
            if (MainSpriteRenderer == null)
                MainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            controller = GetComponent<PlatformerPlayerController2D>();
        }

        private void Update()
        {
            if (controller.Input.Move.x > 0)
                Orientation = CharacterOrientation.Right;
            else if (controller.Input.Move.x < 0)
                Orientation = CharacterOrientation.Left;
        }
        
    }
}