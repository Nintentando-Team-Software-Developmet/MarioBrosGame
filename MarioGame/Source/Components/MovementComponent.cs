using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class MovementComponent : BaseComponent
    {
        public MovementType Direction { get; set; }

        public MovementComponent(MovementType direction)
        {
            this.Direction = direction;
        }

    }
}
