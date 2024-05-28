using Components;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
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

        public override void Update(GameTime gameTime)
        {
            // Update logic for rendering system if needed
        }

        public void Render(GameTime gameTime)
        {
            _spriteBatch.Begin();
            // Render all entities with SpriteComponent
            foreach (var entityId in _componentManager.GetAllEntitiesWithComponent<SpriteComponent>())
            {
                var sprite = _componentManager.GetComponent<SpriteComponent>(entityId);
                var transform = _componentManager.GetComponent<TransformComponent>(entityId);
                _spriteBatch.Draw(sprite.Texture, transform.Position, Color.White);
            }
            _spriteBatch.End();
        }
    }
}
