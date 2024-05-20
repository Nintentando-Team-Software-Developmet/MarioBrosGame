using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS
{
    public abstract class System
    {
        public virtual void AddEntity(Entity entity)
        {

        }

        public virtual void RemoveEntity(Entity entity)
        {

        }

        public virtual void Subscribe()
        {

        }
        public virtual void Unsubscribe()
        {

        }


        public virtual void Update(GameTime gameTime)
        {

        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}