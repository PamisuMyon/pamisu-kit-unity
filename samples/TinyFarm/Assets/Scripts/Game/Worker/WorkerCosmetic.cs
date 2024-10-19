using System;
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

        private Action _playAnimOneShotOnCompleteDelegate;
        
        public WorkerController Controller { get; internal set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _bodyAnimator.PlaybackComplete += OnBodyAnimatorPlaybackComplete;
            
            PlayAnim("Idle");
        }

        private void OnBodyAnimatorPlaybackComplete()
        {
            _playAnimOneShotOnCompleteDelegate?.Invoke();
            _playAnimOneShotOnCompleteDelegate = null;
        }

        public void OnUpdate(float deltaTime)
        {
            _bodyAnimator.OnUpdate(deltaTime);
            _hairAnimator.OnUpdate(deltaTime);
            _toolsAnimator.OnUpdate(deltaTime);
        }

        public void PlayAnim(string animName)
        {
            _bodyAnimator.Play(animName);
            _hairAnimator.Play(animName);
            _toolsAnimator.Play(animName);
        }

        public void PlayAnimOneShot(string animName, Action onComplete)
        {
            _playAnimOneShotOnCompleteDelegate = onComplete;
            _bodyAnimator.Play(animName, SpriteAnimator.LoopMode.OneShot);
            _hairAnimator.Play(animName, SpriteAnimator.LoopMode.OneShot);
            _toolsAnimator.Play(animName, SpriteAnimator.LoopMode.OneShot);
        }

        private NPBehave.Action.Result PlayAnimTask(string animName, NPBehave.Action.Request request)
        {
            if (request == NPBehave.Action.Request.CANCEL)
                return NPBehave.Action.Result.FAILED;
            if (request == NPBehave.Action.Request.START)
            {
                PlayAnim(animName);
                return NPBehave.Action.Result.PROGRESS;
            }
            if (request == NPBehave.Action.Request.UPDATE)
            {
                if (_bodyAnimator.IsPlaying)
                    return NPBehave.Action.Result.PROGRESS;
                return NPBehave.Action.Result.SUCCESS;
            }
            return NPBehave.Action.Result.FAILED;
        }
        
        public void PlayIdleAnim() => PlayAnim("Idle");
        public void PlayWalkingAnim() => PlayAnim("Walking");
        public void PlayCarryAnim() => PlayAnim("Carry");
        public NPBehave.Action.Result PlayWateringAnim(NPBehave.Action.Request request) => PlayAnimTask("Watering", request);
        
        public NPBehave.Action.Result UpdateFaceDirection(bool shouldAbort)
        {
            if (shouldAbort)
                return NPBehave.Action.Result.FAILED;
            if (!Controller.NavAgent.enabled || Controller.NavAgent.isStopped)
                return NPBehave.Action.Result.PROGRESS;
            FaceDirection = (int)Mathf.Sign(Controller.NavAgent.velocity.x);
            return NPBehave.Action.Result.PROGRESS;
        }

    }
}