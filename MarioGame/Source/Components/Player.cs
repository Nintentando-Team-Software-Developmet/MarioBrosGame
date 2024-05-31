using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Components;

public class Player:EntityBase

{

    public Vector2 velocity  { get; set; }
    public float playerSpeed { get; set; }
    public Animation playerAnimation { get; set; }
    private bool isMovingLeft { get; set; }
    float originalYPosition { get; set; }
    private bool isJumping { get; set; } //false
    private bool isBending { get; set; } //false




    private Vector2 previousPosition { get; set; }
    public Player(Texture2D sprite1, Texture2D sprite2,Texture2D sprite3,Texture2D sprite4,Texture2D sprite5,Texture2D sprite6,
        Texture2D sprite7,Texture2D spriteB,Texture2D spriteS,Texture2D spriteS2,Texture2D spriteR3,Texture2D spriteR3l,Texture2D spriteBB,Texture2D spriteBB2)
    {
        velocity = new Vector2();
        playerAnimation = new Animation(sprite1, sprite2,sprite3,sprite4,sprite5,
            sprite6,sprite7,spriteB,spriteS,spriteS2,spriteR3,spriteR3l,spriteBB,spriteBB2);
        playerSpeed = 3;
        isJumping = false;
        isBending = false;
        position = position with { X = 10 };
        position = position with { Y = 100 };
        originalYPosition = position.Y;
    }

    public override void Update()
    {
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyboardState = Keyboard.GetState();

        velocity = Vector2.Zero;


        if (gamePadState.ThumbSticks.Left.X < -0.1f  || keyboardState.IsKeyDown(Keys.Left))
        {
            velocity = velocity with { X = -playerSpeed };
        }


        if (gamePadState.ThumbSticks.Left.X > 0.1f || keyboardState.IsKeyDown(Keys.Right) )
        {
            velocity = velocity with { X = playerSpeed };
        }

        HandleJumping(gamePadState,keyboardState);

        isBending = gamePadState.IsButtonDown(Buttons.LeftThumbstickDown) || keyboardState.IsKeyDown(Keys.Down);

        position += velocity;

        playerAnimation.SetActive(velocity != Vector2.Zero);
    }


    private void HandleJumping(GamePadState gamePadState,KeyboardState keyboardState)
    {

        const float jumpHeight = 30f;
        const float fallSpeed = 1f;

        if ((gamePadState.IsButtonDown(Buttons.LeftThumbstickUp) || keyboardState.IsKeyDown(Keys.Up) ) && !isJumping)
        {
            if (position.Y == originalYPosition)
            {
                position = position with { Y = position.Y - jumpHeight };
                isJumping = true;
            }
        }

        else if (isJumping)
        {

            position = position with { Y = position.Y + fallSpeed };

            if (position.Y >= originalYPosition)
            {
                position = position with { Y = originalYPosition };
                isJumping = false;
            }
        }
    }


    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {

            if (position.Y > previousPosition.Y)
            {
                DrawJumping(spriteBatch);
            }
            else if (position.X != previousPosition.X)
            {
                DrawRunning(spriteBatch, gameTime);
            }
            else if (isBending)
            {
                DrawBending(spriteBatch);
            }
            else
            {
                DrawStopped(spriteBatch, gameTime);
            }

            previousPosition = position;
        }

        private void DrawJumping(SpriteBatch spriteBatch)
        {
            if (isMovingLeft)
            {
                playerAnimation.JumpLeft(spriteBatch, position);
            }
            else
            {
                playerAnimation.JumpRight(spriteBatch, position);
            }
        }

        private void DrawRunning(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (position.X > previousPosition.X)
            {
                playerAnimation.RunRight(spriteBatch, position, gameTime, 100);
                isMovingLeft = false;
            }
            else
            {
                playerAnimation.RunLeft(spriteBatch, position, gameTime, 100);
                isMovingLeft = true;
            }
        }

        private void DrawBending(SpriteBatch spriteBatch)
        {
            Vector2 newPosition = position;
            newPosition.Y += 10;

            if (isMovingLeft)
            {
                playerAnimation.BendLeft(spriteBatch, newPosition);
            }
            else
            {
                playerAnimation.BendRight(spriteBatch, newPosition);
            }
        }

        private void DrawStopped(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isMovingLeft)
            {
                playerAnimation.RunLeft(spriteBatch, position, gameTime, 100);
            }
            else
            {
                playerAnimation.RunRight(spriteBatch, position, gameTime, 100);
            }
        }
}
