using System;
using System.Collections.Generic;

namespace ECS
{
    public class Entity
    {
        private Dictionary<Type, Component> _components;


        public bool IsActive;

        public Entity(bool isActive = true)
        {
            _components = new Dictionary<Type, Component>();
            IsActive = isActive;
        }

        public void AddComponent(Component component)
        {

        }

        public void RemoveComponent<T>() where T : Component
        {
        
        }

        public T GetComponent<T>() where T : Component

            return default(T);
        }
    }
}