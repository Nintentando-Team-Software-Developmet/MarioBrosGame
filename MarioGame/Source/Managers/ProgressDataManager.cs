using Microsoft.Xna.Framework;

using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Managers;

public class ProgressDataManager
{
    private ProgressData data;
    private const int DEFAULT_TIME = 300;

    public ProgressDataManager()
    {
        data = new ProgressData(DEFAULT_TIME, 456, 987654);
    }

    public void Update(GameTime gameTime)
    {
        if (gameTime != null) data.Time -= gameTime.ElapsedGameTime.TotalSeconds;
    }

    public ProgressData Data
    {
        get => data;
        set => data = value;
    }
}
