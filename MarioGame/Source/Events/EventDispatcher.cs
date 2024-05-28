using System;
using System.Collections.Generic;

namespace Events
{
    public class EventDispatcher
    {
        private Dictionary<Type, List<Action<Event>>> _subscribers = new();

        public void Subscribe<T>(Action<Event> callback) where T : Event
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)] = new List<Action<Event>>();
            }
            _subscribers[typeof(T)].Add(callback);
        }

        public void Unsubscribe<T>(Action<Event> callback) where T : Event
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)].Remove(callback);
            }
        }

        public void Dispatch(Event evt)
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
