
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using SuperMarioBros.Utils.DataStructures;
using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
using MonoVector2 = Microsoft.Xna.Framework.Vector2;

namespace SuperMarioBros.Utils
{
    public class DebuggerColliders : IDisposable
    {
        private World _world;
        private SpriteData spriteData;
        private Texture2D pixelTexture;
        private bool disposed;

        public DebuggerColliders(World world, SpriteData spriteData)
        {
            _world = world;
            this.spriteData = spriteData;
            pixelTexture = new Texture2D(spriteData?.graphics.GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pixelTexture.Dispose();
                }

                disposed = true;
            }
        }

        public void DrawColliders()
        {
            foreach (Body body in _world.BodyList)
            {
                foreach (Fixture fixture in body.FixtureList)
                {
                    if (fixture.Shape.ShapeType == ShapeType.Polygon)
                    {
                        var vertices = ((PolygonShape)fixture.Shape).Vertices;
                        DrawPolygon(body, vertices, Color.Yellow);
                    }
                }
            }
        }

        private void DrawPolygon(Body body, Vertices vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                AetherVector2 start = body.GetWorldPoint(vertices[i]);
                AetherVector2 end = body.GetWorldPoint(vertices[(i + 1) % vertices.Count]);

                MonoVector2 startMono = new MonoVector2(start.X * 100, start.Y * 100);
                MonoVector2 endMono = new MonoVector2(end.X * 100, end.Y * 100);

                DrawLine(startMono, endMono, color);
            }
        }

        private void DrawLine(MonoVector2 start, MonoVector2 end, Color color)
        {
            MonoVector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteData.spriteBatch.Draw(pixelTexture,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1),
                null,
                color,
                angle,
                MonoVector2.Zero,
                SpriteEffects.None,
                0);
        }
    }
}