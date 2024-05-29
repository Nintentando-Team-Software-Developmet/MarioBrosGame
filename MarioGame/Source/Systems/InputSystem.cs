using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Events;

namespace SuperMarioBros.Source.Systems
{
    public class InputSystem : BaseSystem
    {
        private EventDispatcher _eventDispatcher;

        public InputSystem(EventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Space))
            {
                _eventDispatcher.Dispatch(new InputEvent("Jump"));
            }
        }
    }
}
