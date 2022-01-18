using OpenTK.Graphics.OpenGL4;

namespace ElectricC.Graphics.Meshes
{
    public class VAO
    {
        private int vaoId;
        public int ElementCount { get; set; }

        public VAO(int vaoId, int elementCount)
        {
            this.vaoId = vaoId;
            ElementCount = elementCount;
        }

        public void Bind()
        {
            GL.BindVertexArray(vaoId);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }
    }
}
