namespace ElectricC.Graphics.Meshes
{
    public class Mesh
    {
        public static readonly uint VERTEX_SIZE = 5;

        protected VAO vao;
        protected Texture texture;

        public int ElementCount { 
            get 
            {
                return vao.ElementCount;
            } 
        }

        public Mesh(): this(new float[] {}, new uint[] {}, new Texture(0))
        {}

        public Mesh(float[] vertices, uint[] indexes, Texture texture)
        {
            this.texture = texture;
            UpdateMesh(vertices, indexes);
        }

        public void UpdateMesh(float[] vertices, uint[] indexes)
        {
            vao = VAOFactory.CreateVAO(vertices, indexes);
        }

        public void Bind()
        {
            texture.Bind();
            vao.Bind();
        }

        public void UnBind()
        {
            vao.UnBind();
            texture.UnBind();
        }
    }
}
