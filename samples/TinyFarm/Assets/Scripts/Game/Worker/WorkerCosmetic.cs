using Game.Framework;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Worker.Models
{
    public class WorkerCosmetic : MonoEntity, IUpdatable
    {
        [SerializeField]
        private SpriteAnimator _bodyAnimator;
        
        [SerializeField]
        private SpriteAnimator _hairAnimator;
        
        [SerializeField]
        private SpriteAnimator _toolsAnimator;
        
        private int _faceDirection;
        public int FaceDirection
        {
            get => _faceDirection;
            set
            {
                _faceDirection = value;
                _bodyAnimator.SpriteRenderer.flipX = _faceDirection == -1;
                _hairAnimator.SpriteRenderer.flipX = _faceDirection == -1;
                _toolsAnimator.SpriteRenderer.flipX = _faceDirection == -1;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            PlayAnim("Idle");
        }

        public void OnUpdate(float deltaTime)
        {
            _bodyAnimator.OnUpdate(deltaTime);
            _hairAnimator.OnUpdate(deltaTime);
            _toolsAnimator.OnUpdate(deltaTime);
        }

        private void PlayAnim(string animName)
        {
            _bodyAnimator.Play(animName);
            _hairAnimator.Play(animName);
            _toolsAnimator.Play(animName);
        }

    }
}