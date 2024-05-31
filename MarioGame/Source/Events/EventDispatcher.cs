using System;
using System.Collections.Generic;

namespace SuperMarioBros.Source.Events
{
    public class EventDispatcher
    {
        private Dictionary<Type, List<Action<BaseEvent>>> _subscribers = new();

        public void Subscribe<T>(Action<BaseEvent> callback) where T : BaseEvent
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)] = new List<Action<BaseEvent>>();
            }
            _subscribers[typeof(T)].Add(callback);
        }

        public void Unsubscribe<T>(Action<BaseEvent> callback) where T : BaseEvent
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)].Remove(callback);
            }
        }

        public void Dispatch(InputEvent evt)
        {
            if (evt != null)
            {
                var type = evt.GetType();
                if (_subscribers.ContainsKey(type))
                {
                    foreach (var subscriber in _subscribers[type])
                    {
                        subscriber.Invoke(evt);
                    }
                }
            }
        }
    }
}
