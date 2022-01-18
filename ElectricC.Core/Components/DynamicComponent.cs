using ElectricC.ECS;
using OpenTK;

namespace ElectricC.Core.Components
{
    public class DynamicComponent: Component
    {
        public Vector3 Direction;
        public float Speed;

        public DynamicComponent(): this(1f)
        {}

        public DynamicComponent(float speed)
        {
            Speed = speed;
            Direction = new Vector3(0, 0, 0);
        }
    }
}
