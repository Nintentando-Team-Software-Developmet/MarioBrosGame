namespace SuperMarioBros.Source.Components
{

    public class GravityComponent : BaseComponent
    {
        public float gravity { get; set; }

        public GravityComponent(float gravity)
        {
            this.gravity = gravity;
        }
    }
}
