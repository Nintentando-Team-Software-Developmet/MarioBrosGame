using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public abstract class BaseSystem
    {
        public abstract void Update(GameTime gameTime, IEnumerable<Entity> entities);
    }
}
