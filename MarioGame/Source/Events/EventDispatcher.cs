using System;
using System.Collections.Generic;

namespace SuperMarioBros.Source.Events
{
    /// <summary>
    /// Singleton class that manages event subscription, unsubscription and dispatching.
    /// </summary>
    public class EventDispatcher
    {
        private static EventDispatcher _instance;
        private readonly Dictionary<Type, List<Action<object>>> _eventListeners = new();

        /// <summary>
        /// Private constructor to prevent instantiation of this singleton class.
        /// </summary>
        private EventDispatcher() { }

        /// <summary>
        /// Gets the singleton instance of the EventDispatcher.
        /// </summary>
        public static EventDispatcher Instance => _instance ??= new EventDispatcher();

        /// <summary>
        /// Subscribes a listener to a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="listener">The listener to subscribe.</param>
        public void Subscribe<T>(Action<object> listener)
        {
            var eventType = typeof(T);

            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Action<object>>();
            }

            _eventListeners[eventType].Add(listener);
        }

        /// <summary>
        /// Unsubscribes a listener from a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="listener">The listener to unsubscribe.</param>
        public void Unsubscribe<T>(Action<object> listener)
        {
            var eventType = typeof(T);

            if (_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType].Remove(listener);
            }
        }

        /// <summary>
        /// Dispatches an event to all subscribed listeners of the event type.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="eventArgs">The event arguments.</param>
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
