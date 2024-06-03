using System;
using System.Collections.Generic;

using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Entities
{
    public class Entity
    {
        public int Id { get; private set; }
        public string Tag { get; set; }
        private Dictionary<Type, BaseComponent> _components = new();

        public Entity(int id)
        {
            Id = id;
        }

        public void AddComponent(BaseComponent baseComponent)
        {
            if (baseComponent != null) _components[baseComponent.GetType()] = baseComponent;
        }

        public void RemoveComponent<T>() where T : BaseComponent
        {
            _components.Remove(typeof(T));
        }

        public T GetComponent<T>() where T : BaseComponent
        {
            _components.TryGetValue(typeof(T), out var component);
            return (T)component;
        }
    }
}
