using OpenTK.Graphics.OpenGL4;

namespace ElectricC.Graphics.Meshes
{
    public static class VAOFactory
    {
        public static VAO CreateVAO(float[] vertices, uint[] indices)
        {
            int vboVertices = BindVertices(vertices);
            int ibo = BindIndices(indices);

            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertices);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(
                0,
                3,
                VertexAttribPointerType.Float,
                false,
                (int)Mesh.VERTEX_SIZE * sizeof(float),
                0
            );

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(
                1, 
                2,
                VertexAttribPointerType.Float,
                false,
                (int)Mesh.VERTEX_SIZE * sizeof(float),
                3 * sizeof(float)
            );

            return new VAO(vao, indices.Length);
        }

        private static int BindVertices(float[] vertices)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                BufferUsageHint.StaticDraw
            );
            return vbo;
        }

        private static int BindIndices(uint[] indices)
        {
            int ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint),
                indices,
                BufferUsageHint.StaticDraw
            );
            return ibo;
        }
    }
}
