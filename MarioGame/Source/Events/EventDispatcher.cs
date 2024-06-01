using System;
using System.Collections.Generic;

namespace SuperMarioBros.Source.Events
{
    public class EventDispatcher
    {
        private static EventDispatcher _instance;
        private readonly Dictionary<Type, List<Action<object>>> _eventListeners = new();

        private EventDispatcher() { }

        public static EventDispatcher Instance => _instance ??= new EventDispatcher();

        public void Subscribe<T>(Action<object> listener)
        {
            var eventType = typeof(T);

            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Action<object>>();
            }

            _eventListeners[eventType].Add(listener);
        }

        public void Unsubscribe<T>(Action<object> listener)
        {
            var eventType = typeof(T);

            if (_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType].Remove(listener);
            }
        }

        public void Dispatch<T>(T eventArgs)
        {
            var eventType = typeof(T);

            if (_eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in _eventListeners[eventType])
                {
                    listener.Invoke(eventArgs);
                }
            }
        }
    }
}
