using System.Collections.Generic;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;

namespace SuperMarioBros.Source.Systems
{
    public class InputSystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            var keyboardState = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            foreach (var entity in entities.WithComponents<InputComponent, PositionComponent, VelocityComponent>())
            {
                var velocity = entity.GetComponent<VelocityComponent>();
                var position = entity.GetComponent<PositionComponent>();

                position.pass = true;
                position.LastPosition = position.Position;

                velocity.Velocity = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left) || gamePadState.ThumbSticks.Left.X < -0.1f)
                {
                    velocity.Velocity += new Vector2(-1, 0);
                }

                if (keyboardState.IsKeyDown(Keys.Right) || gamePadState.ThumbSticks.Left.X > 0.1f)
                {
                    velocity.Velocity += new Vector2(1, 0);
                }

                if (keyboardState.IsKeyDown(Keys.Z) || gamePadState.IsButtonDown(Buttons.A))
                {
                    position.pass = false;
                }

                if (keyboardState.IsKeyDown(Keys.X) || gamePadState.IsButtonDown(Buttons.B))
                {
                    velocity.Velocity += new Vector2(0, 1);
                }
            }
        }
    }
}
