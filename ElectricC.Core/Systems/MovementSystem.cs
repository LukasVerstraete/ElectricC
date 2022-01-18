using ElectricC.Core.Components;
using ElectricC.ECS;
using OpenTK;
using System;

namespace ElectricC.Core.Systems
{
    public class MovementSystem : ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<Transform>(entityId)
                && EntityManager.HasComponent<DynamicComponent>(entityId);
        }

        public override void Update(float deltaTime)
        {
            foreach (int entityId in validEntities)
            {
                
                DynamicComponent movementComponent = EntityManager.GetComponent<DynamicComponent>(entityId);
                Transform transform = EntityManager.GetComponent<Transform>(entityId);

                Vector3 directionNormalized = movementComponent.Direction;
                Vector3 front = transform.Forward.Normalized();
                Vector3 right = transform.Right.Normalized();
                Vector3 up = Vector3.UnitY * directionNormalized.Y;
                front *= directionNormalized.Z;
                right *= directionNormalized.X;

                if (front.Length > 0) { front.Normalize(); }
                if (right.Length > 0) { right.Normalize(); }
                if (up.Length > 0) { up.Normalize(); }

                Vector3 direction = (front + right + up);
                if(direction.Length > 0) { direction.Normalize(); }

                if (directionNormalized.Length > 0)
                {
                    directionNormalized.Normalize();
                }
                float adjustedSpeed = movementComponent.Speed * deltaTime;
                transform.Position += (direction * adjustedSpeed);
                //transform.Position += (directionNormalized * adjustedSpeed);
            }
        }
    }
}
