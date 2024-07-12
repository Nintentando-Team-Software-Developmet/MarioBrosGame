using System;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Scenes;

public class WinScene : IScene, IDisposable
{
    private bool _disposed;
    private ProgressDataManager _progressDataManager;
    private double _displayTime;
    public const double MaxDisplayTime = 10.0;

    public WinScene(ProgressDataManager progressDataManager)
    {
        _progressDataManager = progressDataManager;
        _displayTime = 0;
    }

    public void Load(SpriteData spriteData)
    {
        Sprites.Load(spriteData?.content);
        MediaPlayer.Play(spriteData?.content.Load<Song>("Sounds/end"));
    }

    public void Unload()
    {
        MediaPlayer.Stop();
        _progressDataManager.ResetLevel();
        _displayTime = 0;
    }

    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        if (spriteData != null)
        {
            spriteData.spriteBatch.Begin();
            CommonRenders.DrawText("Thank you for playing!", 310, 180, spriteData);
            CommonRenders.DrawText("Developed by: NINTENTANDO", 270, 330, spriteData);
            CommonRenders.DrawText("Please insert your credit ", 270, 430, spriteData);
            CommonRenders.DrawText("to play again", 400, 480, spriteData);
            spriteData.spriteBatch.End();
        }
    }

    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
        if (gameTime != null) _displayTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (_displayTime >= MaxDisplayTime)
        {
            _progressDataManager.UpdateHighScore();
            sceneManager.ChangeScene(SceneName.MainMenu);
        }
    }

    public SceneType GetSceneType()
    {
        return SceneType.TransitionScene;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _progressDataManager = null;
        }

        _disposed = true;
    }
}
