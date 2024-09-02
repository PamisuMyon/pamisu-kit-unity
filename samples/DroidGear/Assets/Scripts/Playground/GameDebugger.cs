using Game.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground
{
    public class GameDebugger : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField]
        private float _addExp = 2;
        
        private CombatDirector _combatDirector;
        private CombatSystem _combatSystem;
        
        private void Start()
        {
            _combatDirector = FindFirstObjectByType<CombatDirector>();
            _combatSystem = _combatDirector.GetSystem<CombatSystem>();
        }

        public void Update()
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                _combatSystem.AddPlayerExp(_addExp);
            }
        }

#endif
    }
}