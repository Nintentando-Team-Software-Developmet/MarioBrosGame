using System;
using System.Collections.Generic;

using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Entities
{
    public class Entity
    {
        private Dictionary<string, BaseComponent> _components = new();
        public bool Sliding { get; set; }

        public void AddComponent<T>(T component) where T : BaseComponent
        {
            _components[typeof(T).Name] = component;
        }

        public T GetComponent<T>() where T : BaseComponent
        {
            _components.TryGetValue(typeof(T).Name, out var component);
            return component as T;
        }

        public void RemoveComponent<T>() where T : BaseComponent
        {
            _components.Remove(typeof(T).Name);
        }

        public bool HasComponent<T>() where T : BaseComponent
        {
            return _components.ContainsKey(typeof(T).Name);
        }


    }
}
