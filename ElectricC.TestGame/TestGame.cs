using ElectricC.Core;
using ElectricC.Core.Components;
using ElectricC.Core.Systems;
using ElectricC.ECS;
using ElectricC.Graphics.Meshes;
using OpenTK;

namespace ElectricC.TestGame
{
    public class TestGame : IGame
    {
        public void Initialize(Engine engine)
        {
            DynamicComponent dynamicComponent = new DynamicComponent(2f);
            MovementControlComponent controlComponent = new MovementControlComponent();
            engine.EntityManager.AddComponents(engine.CameraId, dynamicComponent, controlComponent);

            engine.EntityManager.AddSystem(new TestSystem());
            engine.EntityManager.AddSystem(new CameraLookSystem());

            int entity1 = TestObjectBuilder.TestObject(engine.EntityManager);
            engine.EntityManager.GetComponent<Transform>(entity1).TranslateX(-0.5f);
            engine.EntityManager.GetComponent<Transform>(entity1).TranslateZ(0.0f);
            engine.EntityManager.GetComponent<Transform>(entity1).Scale = new Vector3(0.3f, 0.3f, 0.3f);
            int entity2 = TestObjectBuilder.TestObject(engine.EntityManager);
            engine.EntityManager.GetComponent<Transform>(entity2).TranslateX(0.5f);
            engine.EntityManager.GetComponent<Transform>(entity2).TranslateZ(0.0f);
            engine.EntityManager.GetComponent<Transform>(entity2).Scale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }

    public static class TestObjectBuilder
    {
        public static int TestObject(EntityManager manager)
        {
            int entityId = manager.CreateEntity();

            Texture girlTexture = TextureLoader.LoadTexture("./Assets/Leen.jpg");
            Mesh mesh = new Mesh(GetVertices(), GetIndices(), girlTexture);
            RenderComponent renderComponent = new RenderComponent(mesh);
            Transform transform = new Transform();
            TestComponent testComponent = new TestComponent();
            //MovementComponent movementComponent = new MovementComponent(2.0f, Vector3.Zero);

            manager.AddComponent(entityId, renderComponent);
            manager.AddComponent(entityId, transform);
            //manager.AddComponent(entityId, testComponent);
            //manager.AddComponent(entityId, movementComponent);

            return entityId;
        }

        private static float[] GetVertices()
        {
            return new float[]
            {
                -0.5f, -0.5f, -0.5f, 0f, 1f,
                -0.5f, 0.5f, -0.5f, 0f, 0f,
                0.5f, 0.5f, -0.5f, 1f, 0f,
                0.5f, -0.5f, -0.5f, 1f, 1f,
                -0.5f, -0.5f, 0.5f, 1f, 1f,
                -0.5f, 0.5f, 0.5f, 1f, 0f,
                0.5f, 0.5f, 0.5f, 1f, 0f,
                0.5f, -0.5f, 0.5f, 0f, 1f
            };
        }

        private static uint[] GetIndices()
        {
            return new uint[]
            {
                0, 1, 2,
                0, 2, 3,

                7, 6, 5,
                7, 5, 4,

                1, 5, 6,
                1, 6, 2,

                4, 0, 3,
                4, 3, 7,

                4, 5, 1,
                4, 1, 0,

                3, 2, 6,
                3, 6, 7
            };
        }
    }
}
