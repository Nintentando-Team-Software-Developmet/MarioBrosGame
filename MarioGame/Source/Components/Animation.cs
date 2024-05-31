using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Source.Components;

public class Animation
{
       private Texture2D[] spritesheets;
        private Texture2D[] spritesheets2;
        private Texture2D[] spritesheets3;
        private Texture2D[] spritesheets4;
        private Texture2D[] spritesheets5;
        private Texture2D[] spritesheets6;
        private  int currentTextureIndex { get; set; }
        private  int frames;
        private  int rows { get; set; }
        private int c { get; set;}
        private float timeSinceLastFrame { get; set; }
        private bool isActive { get; set; }
        private bool isInitialMove = true;
        private float initialMoveDuration = 200f;
        private float initialMoveElapsedTime { get; set; }
        public Animation(Texture2D spritesheet1, Texture2D spritesheet2, Texture2D spritesheet3, Texture2D spritesheet4,
            Texture2D spritesheet5, Texture2D spritesheet6,
            Texture2D spritesheet7, Texture2D spritesheetB, Texture2D spritesheetS, Texture2D spritesheetS2,
            Texture2D spritesheetR3, Texture2D spritesheetR3l,
            Texture2D spritesheetBB, Texture2D spritesheetBB2, float width = 14, float height = 16)
        {
            spritesheets = new Texture2D[] { spritesheet1, spritesheetBB2, spritesheet2, spritesheet3, spritesheetR3 };
            spritesheets2 = new Texture2D[] { spritesheetB, spritesheetBB, spritesheet4, spritesheet5, spritesheetR3l };
            spritesheets3 = new Texture2D[] { spritesheet6 };
            spritesheets4 = new Texture2D[] { spritesheet7 };
            spritesheets5 = new Texture2D[] { spritesheetS };
            spritesheets6 = new Texture2D[] { spritesheetS2 };

            isActive = false;
            currentTextureIndex = 0;
            rows = 0;
            c = 0;
            timeSinceLastFrame = 0;
            initialMoveElapsedTime = 0f;

            if (spritesheet1 != null) frames = (int)(spritesheet1.Width / width);
            Console.WriteLine(frames);

        }

        public void SetActive(bool active)
        {

            isActive = active;
            if (!isActive)
            {
                currentTextureIndex = 0;
                c = 0;
                isInitialMove = true;
                initialMoveElapsedTime = 0f;
            }
        }

        public void RunRight(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime,
            float millisecondsPerFrame = 5)
        {
            DrawMovement(spriteBatch, position, gameTime, spritesheets, millisecondsPerFrame);
        }

        public void RunLeft(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime,
            float millisecondsPerFrame = 5)
        {
            DrawMovement(spriteBatch, position, gameTime, spritesheets2, millisecondsPerFrame);
        }

        public void DrawMovement(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, Texture2D[] sprites,
            float millisecondsPerFrame = 5)
        {
            if (!isActive)
            {
                DrawInitialFrame(spriteBatch, position, sprites);

                return;
            }

            if (c >= frames)
                return;

            var rect = new Rectangle(15 * c, rows, 16, 32);

            if (isInitialMove)
            {
                if (spriteBatch != null)
                    if (sprites != null)
                        if (gameTime != null)
                            DrawInitialMove(spriteBatch, position, rect, sprites, gameTime);
            }
            else
            {
                if (spriteBatch != null)
                    if (sprites != null)
                        spriteBatch.Draw(sprites[currentTextureIndex + 2], position, rect, Color.White);
            }

            if (gameTime != null) UpdateFrameTiming(gameTime, millisecondsPerFrame);
        }

        private void DrawInitialFrame(SpriteBatch spriteBatch, Vector2 position, Texture2D[] sprites)
        {
            DrawStatic(spriteBatch, position, sprites, 32);
        }

        private void DrawInitialMove(SpriteBatch spriteBatch, Vector2 position, Rectangle rect, Texture2D[] sprites,
            GameTime gameTime)
        {
            spriteBatch.Draw(sprites[1], position, rect, Color.White);
            initialMoveElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (initialMoveElapsedTime >= initialMoveDuration)
            {
                isInitialMove = false;
                initialMoveElapsedTime = 0f;
                c = 0;
            }
        }

        private void UpdateFrameTiming(GameTime gameTime, float millisecondsPerFrame)
        {
            timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                c++;
                if (c == frames)
                {
                    c = 0;
                    if (!isInitialMove)
                    {
                        currentTextureIndex = (currentTextureIndex + 1) % 3;
                    }
                }
            }
        }

        public void DrawStatic(SpriteBatch spriteBatch, Vector2 position, Texture2D[] sprites, int height)
        {
            var rect = new Rectangle(16 * c, rows, 16, height);
            if (spriteBatch != null)
                if (sprites != null)
                    spriteBatch.Draw(sprites[0], position, rect, Color.White);
        }

        public void BendRight(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawStatic(spriteBatch, position, spritesheets3, 22);

        }

        public void BendLeft(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawStatic(spriteBatch, position, spritesheets4, 22);

        }

        public void JumpRight(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawStatic(spriteBatch, position, spritesheets5, 32);

        }

        public void JumpLeft(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawStatic(spriteBatch, position, spritesheets6, 32);

        }
}

