using ElectricC.ECS;
using ElectricC.Graphics.Meshes;

namespace ElectricC.Core.Components
{
    public class RenderComponent: Component
    {
        public Mesh Mesh;

        public RenderComponent(Mesh mesh)
        {
            Mesh = mesh;
        }
    }
}
