using System;
using System.Collections.Generic;
using System.Linq;


using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Extensions
{
    public static class EntityExtensions
    {
        public static IEnumerable<Entity> WithComponents(this IEnumerable<Entity> entities, params Type[] componentTypes)
        {
            return entities.Where(e => componentTypes.All(type => e.HasComponent(type)));
        }

        private static bool HasComponent(this Entity entity, Type componentType)
        {
            var method = typeof(Entity).GetMethod("HasComponent", Type.EmptyTypes)?.MakeGenericMethod(componentType);
            return method != null && (bool)method.Invoke(entity, null)!;
        }

        public static void ClearAll(this List<Entity> entities)
        {
            if (entities != null)
            {
                entities.ForEach(e => e.ClearComponents());
                entities.Clear();
            }
        }

    }
}
