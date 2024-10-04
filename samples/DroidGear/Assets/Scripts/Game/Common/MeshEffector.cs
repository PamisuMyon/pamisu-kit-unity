using System;
using UnityEngine;

namespace Game.Common
{
    public class MeshEffector : MonoBehaviour
    {
        
        public enum EffectType
        {
            None,
            Ice,
        }
        
        [SerializeField]
        private Material _iceMaterial;

        private Material _originMaterial;
        private MeshRenderer _targetRenderer;

        private void Awake()
        {
            _targetRenderer = GetComponentInChildren<MeshRenderer>();
            _originMaterial = _targetRenderer.material;
        }

        // private void Test()
        // {
        //     
        //     LMotion.Create(0f, 1f, 2f)
        //         .BindWithState(_targetRenderer.material, (t, m) =>
        //         {// TODO Fixme
        //             m.Lerp(m, _iceMaterial, t);
        //         });
        // }

        public void ChangeEffect(EffectType type)
        {
            _targetRenderer.material = type switch
            {
                EffectType.None => _originMaterial,
                EffectType.Ice => _iceMaterial,
                _ => _originMaterial
            };
        }
        
    }
}