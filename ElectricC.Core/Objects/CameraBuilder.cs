using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Graphics.Window;

namespace ElectricC.Core.Objects
{
    public static class CameraBuilder
    {
        public static int Camera(
            EntityManager entityManager,
            float fov,
            float aspectRatio,
            float zNear,
            float zFar
        )
        {
            int cameraId = entityManager.CreateEntity();
            Transform transform = new Transform();

            entityManager.AddComponent(cameraId, new CameraComponent(fov, aspectRatio, zNear, zFar));
            entityManager.AddComponent(cameraId, transform);

            return cameraId;
        }

        public static int PerspectiveCamera(EntityManager entityManager, ElectricWindow window)
        {
            return Camera(entityManager, 45f, window.Width / (float)window.Height, 0.01f, 100.0f);
        }
    }
}
