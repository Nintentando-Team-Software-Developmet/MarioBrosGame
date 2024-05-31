using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Source.Entities;

public abstract class EntityBase
{
    public Texture2D spritesheet  { get; set;}
    public Vector2 position { get; set;}

    public abstract void Update();

    public abstract void Draw(SpriteBatch spriteBatch,GameTime gameTime);

}
