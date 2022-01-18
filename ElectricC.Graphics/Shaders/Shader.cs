using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ElectricC.Graphics.Shaders
{
    public class Shader
    {
        public int Handle { get; protected set; }

        public Shader(int handle)
        {
            Handle = handle;
        }

        public void Bind()
        {
            GL.UseProgram(Handle);
        }

        public void Detach()
        {
            GL.UseProgram(0);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(Handle, uniformName);
        }

        public void LoadUniform(string uniformName, Matrix4 matrix)
        {
            GL.UniformMatrix4(GetUniformLocation(uniformName), true, ref matrix);
        }

        public void EnableVertexAttribArray(string name)
        {
            int attribLocation = GL.GetAttribLocation(Handle, name);
            GL.EnableVertexAttribArray(attribLocation);
        }
    }
}
