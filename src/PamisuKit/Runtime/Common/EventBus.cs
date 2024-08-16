using System;
using System.Collections.Generic;
using UnityEngine;

namespace PamisuKit.Common
{
    public static class EventBus
    {

        private static Dictionary<Type, Entry> _entries = new();
        
        public static IDisposable On<TMessage>(Action<TMessage> handler, SubscribeOptions option = SubscribeOptions.None) where TMessage : struct
        {
            return On(typeof(TMessage), handler, option);
        }

        private static IDisposable On(Type messageType, Delegate handler, SubscribeOptions option = SubscribeOptions.None)
        {
            OnRaw(messageType, handler, option);
            return new Subscription(messageType, handler);
        }

        public static void OnRaw<TMessage>(Action<TMessage> handler, SubscribeOptions option = SubscribeOptions.None)
            where TMessage : struct
        {
            OnRaw(typeof(TMessage), handler, option);
        }

        private static void OnRaw(Type messageType, Delegate handler, SubscribeOptions option = SubscribeOptions.None)
        {
            if (!_entries.TryGetValue(messageType, out var entry))
            {
                entry = new Entry();
                _entries[messageType] = entry;
            }
            entry.Add(handler, option);
        }

        public static bool Off<TMessage>(Action<TMessage> handler) where TMessage : struct
        {
            return Off(typeof(TMessage), handler);
        }
        
        private static bool Off(Type messageType, Delegate handler)
        {
            if (!_entries.TryGetValue(messageType, out var entry))
            {
                return false;
            }

            var isFound = false;
            for (var i = entry.Handlers.Count - 1; i >= 0; i--)
            {
                if (entry.Handlers[i] == handler)
                {
                    isFound = true;
                    entry.Handlers.RemoveAt(i);
                    entry.Options.RemoveAt(i);
                }
            }

            return isFound;
        }

        public static void Emit<TMessage>(TMessage message) where TMessage : struct
        {
            if (!_entries.TryGetValue(typeof(TMessage), out var entry))
            {
                return;
            }

            for (var i = 0; i < entry.Handlers.Count; i++)
            {
                try
                {
                    ((Action<TMessage>)entry.Handlers[i])?.Invoke(message);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Event Emit ERROR:");
                    Debug.LogException(ex);
                }
                finally
                {
                    if (entry.Options[i].HasFlag(SubscribeOptions.Once))
                    {
                        entry.Handlers.RemoveAt(i);
                        entry.Options.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        
        public static void Clear()
        {
            _entries.Clear();
        }
        
        [Flags]
        public enum SubscribeOptions
        {
            None = 0,
            Unique = 1 << 0,
            Once = 1 << 1,
        }
        
        private class Entry
        {
            public readonly List<Delegate> Handlers = new();
            public readonly List<SubscribeOptions> Options = new();

            public void Add(Delegate handler, SubscribeOptions option)
            {
                if (option.HasFlag(SubscribeOptions.Unique)
                    && Handlers.Contains(handler))
                {
                    return;
                }
                
                Handlers.Add(handler);
                Options.Add(option);
            }

        }
        
        private class Subscription : IDisposable
        {
            
            public Type MessageType { get; private set; }
            public Delegate Handler { get; private set; }

            public Subscription(Type messageType, Delegate handler)
            {
                MessageType = messageType;
                Handler = handler;
            }

            public void Dispose()
            {
                if (Handler == null)
                    return;
                try
                {
                    Off(MessageType, Handler);
                }
                finally
                {
                    MessageType = null;
                    Handler = null;
                }
            }
        }
        
    }
}