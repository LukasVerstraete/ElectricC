using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Input;
using OpenTK;

namespace ElectricC.Core.Systems
{
    public class PlayerControlSystem : ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<DynamicComponent>(entityId)
                && EntityManager.HasComponent<MovementControlComponent>(entityId);
        }

        public override void Input(InputManager input)
        {
            foreach(int entityId in validEntities)
            {
                DynamicComponent dynamic = EntityManager.GetComponent<DynamicComponent>(entityId);
                dynamic.Direction = new Vector3(0, 0, 0);
                if(input.IsDown(Keys.FORWARD)) { dynamic.Direction.Z += 1; }
                if(input.IsDown(Keys.BACKWARD)) { dynamic.Direction.Z -= 1; }
                if(input.IsDown(Keys.LEFT)) { dynamic.Direction.X -= 1; }
                if(input.IsDown(Keys.RIGHT)) { dynamic.Direction.X += 1; }
                if(input.IsDown(Keys.UP)) { dynamic.Direction.Y += 1; }
                if(input.IsDown(Keys.DOWN)) { dynamic.Direction.Y -= 1; }
            }
        }
    }
}
