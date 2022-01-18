using ElectricC.ECS;
using OpenTK;
using System.Collections.Generic;

namespace ElectricC.Game.World.Components
{
    public class WorldComponent: Component
    {
        public Dictionary<Vector3, int> Chunks { get; set; }
        public PerlinNoiseGenerator NoiseGenerator { get; set; }

        public WorldComponent(int seed)
        {
            Chunks = new Dictionary<Vector3, int>();
            NoiseGenerator = new PerlinNoiseGenerator(seed);
        }
    }
}
