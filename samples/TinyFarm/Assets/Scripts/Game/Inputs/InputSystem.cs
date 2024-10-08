using System;
using System.Collections.Generic;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Inputs
{
    public class InputSystem : MonoSystem
    {
        [SerializeField]
        private List<CursorDefine> _cursorDefines;
        
        private Dictionary<CursorStyle, CursorDefine> _cursorDic = new();
        private CursorStyle _currentStyle;
        
        public DefaultInputActions Actions { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Actions = new DefaultInputActions();
            Actions.Game.Enable();
        }
        
    }

    [Serializable]
    public class CursorDefine
    {
        public CursorStyle Style;
        public Texture2D Texture;
        public Vector2 Offset;
    }

    public enum CursorStyle
    {
        Normal,
    }
    
}