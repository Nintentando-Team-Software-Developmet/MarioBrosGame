using System;
using System.Collections.Generic;

namespace SuperMarioBros.Source.Events
{
    /// <summary>
    /// The EventDispatcher class is a singleton that manages event subscription, unsubscription and dispatching.
    /// </summary>
    public class EventDispatcher
    {
        private static EventDispatcher _instance;
        private readonly Dictionary<Type, List<Action<BaseEvent>>> _eventListeners = new();

        /// <summary>
        /// Private constructor to prevent instantiation of the class from outside.
        /// </summary>
        private EventDispatcher() { }

        /// <summary>
        /// Property to get the singleton instance of the EventDispatcher.
        /// </summary>
        public static EventDispatcher Instance => _instance ??= new EventDispatcher();

        /// <summary>
        /// Subscribes a listener to a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of the event to subscribe to.</typeparam>
        /// <param name="listener">The action to be performed when the event is dispatched.</param>
        public void Subscribe<T>(Action<BaseEvent> listener) where T : BaseEvent
        {
            var eventType = typeof(T);

            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Action<BaseEvent>>();
            }

            _eventListeners[eventType].Add(listener);
        }

        /// <summary>
        /// Unsubscribes a listener from a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of the event to unsubscribe from.</typeparam>
        /// <param name="listener">The action to be removed from the event's listeners.</param>
        public void Unsubscribe<T>(Action<BaseEvent> listener) where T : BaseEvent
        {
            var eventType = typeof(T);

            if (_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType].Remove(listener);
            }
        }

        /// <summary>
        /// Dispatches an event to all its listeners.
        /// </summary>
        /// <typeparam name="T">The type of the event to be dispatched.</typeparam>
        /// <param name="eventArgs">The event data.</param>
        public void Dispatch<T>(T eventArgs) where T : BaseEvent
        {
            var eventType = typeof(T);

            if (_eventListeners.ContainsKey(eventType))
            {
                Console.WriteLine($"Dispatching event: {eventArgs}");

                foreach (var listener in _eventListeners[eventType])
                {
                    listener.Invoke(eventArgs);
                }
            }
            else
            {
                Console.WriteLine($"No listeners for event: {eventArgs}");
            }
        }
    }
}
