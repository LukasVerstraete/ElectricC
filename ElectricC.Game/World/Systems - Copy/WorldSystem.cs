using ElectricC.ECS;
using ElectricC.Game.World.Components;
using ElectricC.Input;

namespace ElectricC.Game.World.Systems
{
    public class WorldSystem : ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<WorldComponent>(entityId);
        }

        public override void OnLoad()
        {
            int worldId = EntityManager.CreateEntity();
            WorldComponent world = new WorldComponent(254816156);
            EntityManager.AddComponent(worldId, world);

            ChunkLoader.CreateChunk(EntityManager, world, 0, 0, 0);
            ChunkLoader.CreateChunk(EntityManager, world, 1, 0, 0);
            //ChunkLoader.CreateChunk(EntityManager, world, 0, 0, 2);
            //ChunkLoader.CreateChunk(EntityManager, world, 1, 0, 1);
        }

        public override void Input(InputManager input)
        {
            if (input.IsPressed(Keys.LOCK))
            {
                input.SwitchMouseLock();
            }
        }
    }
}
