using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public interface IRenderableSystem
    {
        void Draw(GameTime gameTime, IEnumerable<Entity> entities);
    }
}
