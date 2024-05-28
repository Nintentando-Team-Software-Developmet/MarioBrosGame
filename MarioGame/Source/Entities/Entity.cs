using Components;

using System;
using System.Collections.Generic;

namespace Entities
{
    public class Entity
    {
        public int Id { get; private set; }
        public string Tag { get; set; }
        private Dictionary<Type, Component> _components = new();

        public Entity(int id)
        {
            Id = id;
        }

        public void AddComponent(Component component)
        {
            _components[component.GetType()] = component;
        }

        public void RemoveComponent<T>() where T : Component
        {
            _components.Remove(typeof(T));
        }

        public T GetComponent<T>() where T : Component
        {
            _components.TryGetValue(typeof(T), out var component);
            return (T)component;
        }
    }
}
