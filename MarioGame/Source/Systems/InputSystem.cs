using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public class InputSystem : BaseSystem
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            _currentKeyboardState = Keyboard.GetState();

            if (entities != null)
                foreach (var entity in entities)
                {
                    var velocity = entity.GetComponent<VelocityComponent>();

                    if (velocity != null)
                    {
                        velocity.Velocity = Vector2.Zero;

                        if (_currentKeyboardState.IsKeyDown(Keys.Left))
                        {
                            velocity.Velocity += new Vector2(-1, 0);
                        }

                        if (_currentKeyboardState.IsKeyDown(Keys.Right))
                        {
                            velocity.Velocity += new Vector2(1, 0);
                        }

                        if (_currentKeyboardState.IsKeyDown(Keys.Up))
                        {
                            velocity.Velocity += new Vector2(0, -1);
                        }

                        if (_currentKeyboardState.IsKeyDown(Keys.Down))
                        {
                            velocity.Velocity += new Vector2(0, 1);
                        }
                    }
                }

            _previousKeyboardState = _currentKeyboardState;
        }
    }
}
