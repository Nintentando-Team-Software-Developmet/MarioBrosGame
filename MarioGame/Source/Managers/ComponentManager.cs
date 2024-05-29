using System;
using System.Collections.Generic;

using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Managers
{
    public class ComponentManager
    {
        private Dictionary<int, Dictionary<Type, BaseComponent>> _entityComponents = new();

        public void AddComponent(int entityId, BaseComponent baseComponent)
        {
            if (!_entityComponents.ContainsKey(entityId))
            {
                _entityComponents[entityId] = new Dictionary<Type, BaseComponent>();
            }

            if (baseComponent != null) _entityComponents[entityId][baseComponent.GetType()] = baseComponent;
        }

        public void RemoveComponent<T>(int entityId) where T : BaseComponent
        {
            if (_entityComponents.ContainsKey(entityId))
            {
                _entityComponents[entityId].Remove(typeof(T));
            }
        }

        public T GetComponent<T>(int entityId) where T : BaseComponent
        {
            if (_entityComponents.ContainsKey(entityId) && _entityComponents[entityId].TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }

        public IEnumerable<int> GetAllEntitiesWithComponent<T>() where T : BaseComponent
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
