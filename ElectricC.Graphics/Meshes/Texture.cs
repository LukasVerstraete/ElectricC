using OpenTK.Graphics.OpenGL4;

namespace ElectricC.Graphics.Meshes
{
    public class Texture
    {
        public readonly int TextureId;

        public Texture(int textureId)
        {
            TextureId = textureId;
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
        }

        public void UnBind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
