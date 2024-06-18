using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components;

public class FacingComponent : BaseComponent
{
    public EntitiesName LeftName { get; set; }
    public EntitiesName RigthName { get; set; }

    public FacingComponent(EntitiesName leftName, EntitiesName rigthName)
    {
        LeftName = leftName;
        RigthName = rigthName;
    }
}
