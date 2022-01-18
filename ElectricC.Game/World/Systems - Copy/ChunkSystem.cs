using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Game.World.Components;
using System;

namespace ElectricC.Game.World.Systems
{
    public class ChunkSystem: ComponentSystem
    {
        private float time = 0;

        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<ChunkComponent>(entityId)
                && EntityManager.HasComponent<Transform>(entityId);
        }

        public override void Update(float deltaTime)
        {
            foreach(int entityId in validEntities)
            {
                Transform transform = EntityManager.GetComponent<Transform>(entityId);
                //transform.RotateY(30f * deltaTime);
                //transform.RotateX(30f * deltaTime);
                //transform.RotateZ(30f * deltaTime);

                //transform.TranslateZ((float)Math.Sin((time + transform.Position.X)) * deltaTime);
            }
            time += deltaTime;
        }
    }
}
