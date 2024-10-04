using PamisuKit.Framework;
using UnityEngine;

namespace Game.Title
{
    public class TitleCameraController : MonoEntity, IUpdatable
    {
        [SerializeField]
        private float _rotateSpeed = 10f;
        
        public void OnUpdate(float deltaTime)
        {
            Trans.Rotate(0f, _rotateSpeed * deltaTime, 0f, Space.World);
        }
    }
}