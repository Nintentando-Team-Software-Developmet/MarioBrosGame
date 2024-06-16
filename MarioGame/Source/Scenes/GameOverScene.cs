using System;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Scenes;

public class GameOverScene : IScene, IDisposable
{
    private bool _disposed;
    private ProgressDataManager _progressDataManager;
    private double _displayTime;
    public const double MaxDisplayTime = 6.0;

    public GameOverScene(ProgressDataManager progressDataManager)
    {
        _progressDataManager = progressDataManager;
        _displayTime = 0;
    }

    /*
     * Loads resources needed for the game over scene.
     * This includes loading game sprites and background music.
     *
     * Parameters:
     *   spriteData: SpriteData object containing content manager for loading resources.
     *               If null, no resources will be loaded.
     */
    public void Load(SpriteData spriteData)
    {
        Sprites.Load(spriteData?.content);
        MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/game_over"));
        MediaPlayer.IsRepeating = true;

    }

    /*
     * Unloads resources and performs cleanup operations for the game over scene.
     * This method is called when the scene is being unloaded or switched.
     * It prints the current screen information to the console.
     */
    public void Unload()
    {
        _displayTime = 0;
        MediaPlayer.Stop();
    }

    /*
     * Draws the gameover scene on the screen.
     * This method clears the graphics device, then draws various elements
     * such as information of world, time, etc
     */
    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        if (spriteData != null)
        {
            spriteData.spriteBatch.Begin();
            CommonRenders.DrawProgressData(spriteData, _progressDataManager.Score,
                _progressDataManager.Coins,
                "1-1",
                0);
            CommonRenders.DrawText("GAME OVER", 420, 330, spriteData);
            spriteData.spriteBatch.End();
        }
    }


    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
        _progressDataManager.UpdateHighScore();

        if (gameTime != null) _displayTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (_displayTime >= MaxDisplayTime)
        {
            _displayTime = 0;
            sceneManager.ChangeScene(SceneName.MainMenu);
            _progressDataManager.ResetTime();
            _progressDataManager.Lives = 3;
        }
    }

    public SceneType GetSceneType()
    {
        return SceneType.TransitionScene;
    }

    /*
     * Performs cleanup operations and releases resources.
     * This method is called to dispose of the GameOver object.
     */
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /*
     * Releases managed resources if disposing is true.
     */
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        _disposed = true;
    }
}
