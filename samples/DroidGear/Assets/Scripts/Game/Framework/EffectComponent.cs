using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class EffectComponent : MonoBehaviour, IUpdatable
    {
        public bool IsActive => Owner.IsActive;
        public Character Owner { get; private set; }
        
        public void Init(Character owner)
        {
            Owner = owner;
        }
        
        public void OnUpdate(float deltaTime)
        {
        }
        
    }
}