using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class MovementComponent: BaseComponent
    {
        public MovementType direcction { get; set; }
        
        public MovementComponent(MovementType direcction)
        {
            this.direcction = direcction;
        }

    }
}