using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Systems;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source
{
    public class InputSystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> inputComponents = entities.WithComponents(typeof(InputComponent));
            foreach (var input in inputComponents)
            {
                var inputComponent = input.GetComponent<InputComponent>();
                HandleOnePressedAction(inputComponent.A);
                HandleActionInput(inputComponent.B);
                HandleActionInput(inputComponent.DOWN);
                HandleActionInput(inputComponent.LEFT);
                HandleActionInput(inputComponent.RIGHT);
            }
        }

        private static void HandleActionInput(InputList actions)
        {
            foreach (var action in actions.actions)
            {
                if (action.IsPressed())
                {
                    actions.IsPressed = true;
                    actions.IsReleased = false;
                    break;
                }
                else
                {
                    actions.IsPressed = false;
                    actions.IsReleased = true;
                }
            }

        }

        private static void HandleOnePressedAction(InputList actions)
        {
            bool foundPressed = false;
            bool allReleased = true;

            foreach (var action in actions.actions)
            {
                if (action.IsPressed() && !actions.IsHeld && actions.IsReleased)
                {
                    actions.IsPressed = true;
                    actions.setHeld(true);
                    foundPressed = true;
                    break; 
                }
            }

            if (!foundPressed)
            {
                actions.IsPressed = false;
            }

            foreach (var action in actions.actions)
            {
                if (!action.IsReleased())
                {
                    allReleased = false;
                    break; 
                }
            }

            actions.IsReleased = allReleased;
            if (allReleased)
            {
                actions.setHeld(false);
            }
        }
    }
}