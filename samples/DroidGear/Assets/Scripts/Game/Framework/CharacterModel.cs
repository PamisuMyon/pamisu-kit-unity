using Game.Common;
using UnityEngine;

namespace Game.Framework
{
    public class CharacterModel : MonoBehaviour
    {
        public float VisualRadius = 1f;
        public float VisualHeight = 2.5f;
        public Animator Anim;
        public Transform[] FirePoints;
        public MeshEffector MeshEffector;
    }
}