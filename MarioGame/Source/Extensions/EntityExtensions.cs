using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Extensions;

public static class EntityExtensions
{
    public static IEnumerable<Entity> WithComponents<T1>(this IEnumerable<Entity> entities)
        where T1 : BaseComponent
    {
        return entities.Where(e => e.HasComponent<T1>());
    }

    public static IEnumerable<Entity> WithComponents<T1, T2>(this IEnumerable<Entity> entities)
        where T1 : BaseComponent
        where T2 : BaseComponent
    {
        return entities.Where(e => e.HasComponent<T1>() && e.HasComponent<T2>());
    }

    public static IEnumerable<Entity> WithComponents<T1, T2, T3>(this IEnumerable<Entity> entities)
        where T1 : BaseComponent
        where T2 : BaseComponent
        where T3 : BaseComponent
    {
        return entities.Where(e => e.HasComponent<T1>() && e.HasComponent<T2>() && e.HasComponent<T3>());
    }

    public static IEnumerable<Entity> WithComponents<T1, T2, T3, T4>(this IEnumerable<Entity> entities)
        where T1 : BaseComponent
        where T2 : BaseComponent
        where T3 : BaseComponent
        where T4 : BaseComponent
    {
        return entities.Where(e => e.HasComponent<T1>() && e.HasComponent<T2>() && e.HasComponent<T3>() && e.HasComponent<T4>());
    }
}
