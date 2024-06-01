using System.Collections.Generic;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public class RenderingSystem : BaseSystem, IRenderableSystem
    {
        private ComponentManager _componentManager;
        private SpriteBatch _spriteBatch;

        public RenderingSystem(ComponentManager componentManager, SpriteBatch spriteBatch)
        {
            _componentManager = componentManager;
            _spriteBatch = spriteBatch;
        }


        public void Render(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach (var entityId in _componentManager.GetAllEntitiesWithComponent<SpriteBaseComponent>())
            {
                var sprite = _componentManager.GetComponent<SpriteBaseComponent>(entityId);
                var transform = _componentManager.GetComponent<TransformBaseComponent>(entityId);
                _spriteBatch.Draw(sprite.Texture, transform.Position, Color.White);
            }
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}
