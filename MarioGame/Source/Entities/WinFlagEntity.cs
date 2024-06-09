using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Entities
{
    public class WinFlagEntity : Entity
    {
        public WinFlagEntity(Texture2D[] texture, Vector2 initialPosition)
        {
            AddComponent(new PositionComponent(initialPosition));
            AddComponent(new AnimationComponent(texture));
        }
    }
}
