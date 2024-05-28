using System.Collections.Generic;

using Entities;

namespace Managers
{
    public class EntityManager
    {
        private Dictionary<int, Entity> _entities = new();
        private int _nextId = 0;

        public Entity CreateEntity(string tag)
        {
            var entity = new Entity(_nextId++) { Tag = tag };
            _entities[entity.Id] = entity;
            return entity;
        }

        public void DestroyEntity(int entityId)
        {
            _entities.Remove(entityId);
        }

        public Entity GetEntity(int entityId)
        {
            _entities.TryGetValue(entityId, out var entity);
            return entity;
        }
    }
}
