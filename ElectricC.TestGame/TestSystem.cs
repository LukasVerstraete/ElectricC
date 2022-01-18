using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Input;
using System;

namespace ElectricC.TestGame
{
    public class TestSystem: ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<TestComponent>(entityId)
                && EntityManager.HasComponent<Transform>(entityId);
        }

        public override void Input(InputManager input)
        {
            if(input.IsPressed(Keys.LOCK))
            {
                input.SwitchMouseLock();
            }
        }

        public override void Update(float deltaTime)
        {
            foreach(int entityId in validEntities)
            {
                Transform transform = EntityManager.GetComponent<Transform>(entityId);
                transform.RotateX(5f * deltaTime);
                transform.RotateY(7f * deltaTime);
                transform.RotateZ(3f * deltaTime);
            }
        }
    }
}
