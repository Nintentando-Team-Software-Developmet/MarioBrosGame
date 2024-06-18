using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components;

public class FacingComponent : BaseComponent
{
    public EntitiesName LeftName { get; set; }
    public EntitiesName RigthName { get; set; }
    public EntitiesName KnockedName { get; set; }
    public EntitiesName RevivingName { get; set; }

    public FacingComponent(EntitiesName leftName, EntitiesName rigthName, EntitiesName knockedName, EntitiesName revivingName)
    {
        LeftName = leftName;
        RigthName = rigthName;
        KnockedName = knockedName;
        RevivingName = revivingName;
    }
}
