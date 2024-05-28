using System;
using System.Collections.Generic;

using Components;

namespace Managers
{
    public class ComponentManager
    {
        private Dictionary<int, Dictionary<Type, Component>> _entityComponents = new();

        public void AddComponent(int entityId, Component component)
        {
            if (!_entityComponents.ContainsKey(entityId))
            {
                _entityComponents[entityId] = new Dictionary<Type, Component>();
            }
            _entityComponents[entityId][component.GetType()] = component;
        }

        public void RemoveComponent<T>(int entityId) where T : Component
        {
            if (_entityComponents.ContainsKey(entityId))
            {
                _entityComponents[entityId].Remove(typeof(T));
            }
        }

        public T GetComponent<T>(int entityId) where T : Component
        {
            if (_entityComponents.ContainsKey(entityId) && _entityComponents[entityId].TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }

        public IEnumerable<int> GetAllEntitiesWithComponent<T>() where T : Component
        {
            foreach (var entity in _entityComponents)
            {
                if (entity.Value.ContainsKey(typeof(T)))
                {
                    yield return entity.Key;
                }
            }
        }
    }
}
