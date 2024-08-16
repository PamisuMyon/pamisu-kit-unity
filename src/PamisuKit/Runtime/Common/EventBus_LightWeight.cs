/*
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Common
{
    public static class EventBus_LightWeight
    {

        private static readonly Dictionary<Type, Delegate> _handlerDic = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Add event listener
        /// </summary>
        /// <param name="handler">event handler</param>
        /// <param name="isSingleHandler">Whether the same handler can only be added once</param>
        public static void On<T>(Action<T> handler, bool isSingleHandler = true) where T : struct
        {
            var messageType = typeof(T);
            _handlerDic.TryAdd(messageType, null);
            if (isSingleHandler)
            {
                var h = (Action<T>)_handlerDic[messageType];
                h -= handler;
                _handlerDic[messageType] = h + handler;
            }
            else
            {
                _handlerDic[messageType] = (Action<T>) _handlerDic[messageType] + handler;
            }
        }

        /// <summary>
        /// Remove event listener
        /// </summary>
        /// <param name="handler">event handler</param>
        public static void Off<T>(Action<T> handler) where T : struct
        {
            var messageType = typeof(T);
            if (!_handlerDic.ContainsKey(messageType))
                return;
            _handlerDic[messageType] = (Action<T>) _handlerDic[messageType] - handler;
        }

        /// <summary>
        /// Send event
        /// </summary>
        public static void Emit<T>(T message) where T : struct
        {
            var messageType = typeof(T);
            if (!_handlerDic.TryGetValue(messageType, out var del))
                return;
            var handler = del as Action<T>;
            try
            {
                handler?.Invoke(message);
            }
            catch (Exception e)
            {
                Debug.LogError("Event Emit ERROR:");
                Debug.LogException(e);
            }
        }

        public static void Clear()
        {
            _handlerDic.Clear();
        }
        
    }
}
*/