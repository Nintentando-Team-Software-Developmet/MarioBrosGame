using System.Collections.Generic;
using System.Linq;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Managers
{
    public class EntityManager
    {
        private List<Entity> _entities = new();


        public IEnumerable<Entity> GetEntities()
        {
            return _entities.AsReadOnly();
        }

        public IEnumerable<Entity> GetEntitiesWithComponent<T>() where T : BaseComponent
        {
            return _entities.Where(e => e.GetComponent<T>() != null);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }
    }
}
