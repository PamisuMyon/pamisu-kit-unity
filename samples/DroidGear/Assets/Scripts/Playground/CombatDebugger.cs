using Game.Combat;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground
{
    public class CombatDebugger : MonoEntity, IUpdatable
    {
#if UNITY_EDITOR

        [SerializeField]
        private float _addExp = 2;
        
        private CombatSystem _combatSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            _combatSystem = GetSystem<CombatSystem>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                _combatSystem.AddPlayerExp(_addExp);
            }
        }

#endif

    }
}