using ElectricC.Core;
using ElectricC.Core.Components;
using ElectricC.Core.Systems;
using ElectricC.Game.World;
using ElectricC.Game.World.Systems;

namespace ElectricC.Game
{
    public class Electric : IGame
    {
        public void Initialize(Engine engine)
        {
            DynamicComponent dynamicComponent = new DynamicComponent(5f);
            MovementControlComponent controlComponent = new MovementControlComponent();
            engine.EntityManager.AddComponents(engine.CameraId, dynamicComponent, controlComponent);

            engine.EntityManager.AddSystem(new CameraLookSystem());

            engine.EntityManager.AddSystem(new WorldSystem());
            engine.EntityManager.AddSystem(new ChunkSystem());

            BlockRegistry.AddBlockType(new BlockType { id = 0, solid = false, transparent = true });
            BlockRegistry.AddBlockType(new BlockType { id = 1, solid = true, transparent = false });
        }
    }
}
