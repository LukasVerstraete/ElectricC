using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Input;
using System;

namespace ElectricC.Core.Systems
{
    public class CameraLookSystem: ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<Transform>(entityId)
                && EntityManager.HasComponent<CameraComponent>(entityId);
        }

        public override void Input(InputManager input)
        {
            if(input.IsMouseLocked)
            {
                foreach(int entityId in validEntities)
                {
                    Transform transform = EntityManager.GetComponent<Transform>(entityId);
                    float yRotation = input.MouseDeltaX;
                    float xRotation = input.MouseDeltaY;
                    transform.RotateX(-xRotation);
                    transform.RotateY(yRotation);
                }
            }
        }
    }
}
