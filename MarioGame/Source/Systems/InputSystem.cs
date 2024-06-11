using System;
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
        private GamePadState _gamePadState;
        private bool wasZPressed { get; set; }
        private bool wasZPressedPlay { get; set; }
        private bool wasLeftPressed{ get; set; }
        private bool wasRightPressed{ get; set; }
        private bool wasLeftPressedPlay{ get; set; }
        private bool wasRightPressedPlay{ get; set; }



        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            _currentKeyboardState = Keyboard.GetState();
            _gamePadState = GamePad.GetState(PlayerIndex.One);

            if (entities == null) return;

            foreach (var entity in entities)
            {
                var velocity = entity.GetComponent<VelocityComponent>();
                var position = entity.GetComponent<PositionComponent>();

                if (position == null || velocity == null) continue;

                position.pass = true;
                position.passR = true;
                position.passBed = true;
                position.LastPosition = position.Position;

                velocity.Velocity = Vector2.Zero;

                ProcessKeyboardInput(velocity, position);

            }
        }

        private void ProcessKeyboardInput(VelocityComponent velocity, PositionComponent position)
        {
            bool isZPressed = _currentKeyboardState.IsKeyDown(Keys.Z);
            bool isLeftPressed = _currentKeyboardState.IsKeyDown(Keys.Left);
            bool isRightPressed = _currentKeyboardState.IsKeyDown(Keys.Right);

            bool isAPressed = _gamePadState.IsButtonDown(Buttons.A);
            bool isLeftPressedPlay = _gamePadState.ThumbSticks.Left.X < -0.1f;
            bool isRightPressedPlay = _gamePadState.ThumbSticks.Left.X > 0.1f;

            if (isZPressed && !wasZPressed || isAPressed && !wasZPressedPlay )
            {
                position.pass = false;
            }
            if (isZPressed && isLeftPressed && !wasLeftPressed || isLeftPressedPlay && isAPressed && !wasLeftPressedPlay)
            {
                position.passR = false;
                velocity.Velocity += new Vector2(-1, 0);
            }
            else if (isZPressed && isRightPressed && !wasRightPressed || isRightPressedPlay && isAPressed && !wasRightPressedPlay)
            {
                position.passR = false;
                velocity.Velocity += new Vector2(1, 0);
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Left) || _gamePadState.ThumbSticks.Left.X < -0.1f)
            {
                velocity.Velocity += new Vector2(-1, 0);
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Right) || _gamePadState.ThumbSticks.Left.X > 0.1f)
            {
                velocity.Velocity += new Vector2(1, 0);
            }

            else if (_currentKeyboardState.IsKeyDown(Keys.X) || _gamePadState.IsButtonDown(Buttons.X))
            {
                position.passBed = false;
            }
            else
            {
                wasZPressed = false;
            }

            wasZPressed = isZPressed;
            wasLeftPressed = isLeftPressed;
            wasRightPressed = isRightPressed;

            wasZPressedPlay = isAPressed;
            wasLeftPressedPlay = isLeftPressedPlay;
            wasRightPressedPlay = isRightPressedPlay;
        }


    }
}
