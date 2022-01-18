using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Graphics.Shaders;
using OpenTK;
using System;

namespace ElectricC.Core.Systems
{
    public class CameraSystem : ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<CameraComponent>(entityId)
                && EntityManager.HasComponent<Transform>(entityId);
        }

        public override void Update(float deltaTime)
        {
            if (validEntities.Count > 0)
            {
                int cameraId = validEntities[0];
                Transform transform = EntityManager.GetComponent<Transform>(cameraId);
                CameraComponent camera = EntityManager.GetComponent<CameraComponent>(cameraId);

                Vector3 front = new Vector3(0, 0, 0);
                front.X = (float)Math.Cos(MathHelper.DegreesToRadians(transform.Rotation.X)) * (float)Math.Cos(MathHelper.DegreesToRadians(transform.Rotation.Y));
                front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(transform.Rotation.X));
                front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(transform.Rotation.X)) * (float)Math.Sin(MathHelper.DegreesToRadians(transform.Rotation.Y));
                front = Vector3.Normalize(front);

                Matrix4 viewMatrix = Matrix4.LookAt(transform.Position, transform.Position + front, Vector3.UnitY);
                //Matrix4 viewMatrix = Matrix4.C
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(camera.FOV),
                    camera.AspectRatio,
                    camera.ZNear,
                    camera.ZFar
                );

                Shader shader = ShaderLoader.GetShader("Basic");
                shader.Bind();
                shader.LoadUniform("view", viewMatrix);
                shader.LoadUniform("projection", projection);
                shader.Detach();
            }
        }
    }
}
