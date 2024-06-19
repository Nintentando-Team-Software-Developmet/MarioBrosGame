using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components;

public class KoopaFacingComponent : BaseComponent
{
    public KoopaSpriteStates LeftName { get; set; }
    public KoopaSpriteStates RigthName { get; set; }
    public KoopaSpriteStates KnockedName { get; set; }
    public KoopaSpriteStates RevivingName { get; set; }

    public KoopaFacingComponent(KoopaSpriteStates leftName, KoopaSpriteStates rigthName, KoopaSpriteStates knockedName, KoopaSpriteStates revivingName)
    {
        LeftName = leftName;
        RigthName = rigthName;
        KnockedName = knockedName;
        RevivingName = revivingName;
    }
}
