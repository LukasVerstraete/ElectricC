using ElectricC.ECS;
using OpenTK;

namespace ElectricC.Core.Components
{
    public class CameraComponent: Component
    {
        public Vector3 Target;
        public float FOV;
        public float AspectRatio;
        public float ZNear;
        public float ZFar;

        public CameraComponent(float fov, float aspectRatio, float zNear, float zFar)
        {
            FOV = fov;
            AspectRatio = aspectRatio;
            ZNear = zNear;
            ZFar = zFar;
            Target = new Vector3(0, 0, -1);
        }
    }
}
