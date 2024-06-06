using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Entities
{
    public class EnemyEntity : Entity
    {
        public EnemyEntity(Texture2D[] textures, Vector2 startPosition)
        {
            AddComponent(new PositionComponent(startPosition));
            AddComponent(new VelocityComponent(Vector2.Zero));
            AddComponent(new AnimationComponent(textures));
            AddComponent(new GravityComponent(33.81F));
        }
    }
}