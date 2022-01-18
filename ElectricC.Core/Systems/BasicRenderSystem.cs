using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Graphics.Meshes;
using ElectricC.Graphics.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace ElectricC.Core.Systems
{
    public class BasicRenderSystem : ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            return EntityManager.HasComponent<RenderComponent>(entityId)
                && EntityManager.HasComponent<Transform>(entityId);
        }

        public override void Render()
        {
            Shader basicShader = ShaderLoader.GetShader("Basic");
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            basicShader.Bind();
            foreach (int entityId in validEntities)
            {
                var transform = EntityManager.GetComponent<Transform>(entityId).Transformation;
                basicShader.LoadUniform("transform", transform);
                GL.ActiveTexture(TextureUnit.Texture0);
                Mesh mesh = EntityManager.GetComponent<RenderComponent>(entityId).Mesh;
                mesh.Bind();
                GL.DrawElements(PrimitiveType.Triangles, mesh.ElementCount, DrawElementsType.UnsignedInt, 0);
                mesh.UnBind();
            }
            basicShader.Detach();
        }
    }
}
