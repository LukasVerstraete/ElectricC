using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ElectricC.Graphics.Shaders
{
    public static class ShaderLoader
    {
        private static readonly string shaderBasePath = "../../../../ElectricC.Graphics/Assets/Shaders/";

        private static Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();

        public static Shader LoadShader(string shaderName)
        {
            string shaderNameNormalized = shaderName.ToLower();

            string vertexShaderPath = shaderBasePath + shaderName + "/" + shaderNameNormalized + "_vert.shader";
            string vertexShaderSource = LoadShaderSource(vertexShaderPath);
            int vertexShaderHandle = CreateShaderProgram(ShaderType.VertexShader, vertexShaderSource);

            string fragmentShaderPath = shaderBasePath + shaderName + "/" + shaderNameNormalized + "_frag.shader";
            string fragmentShaderSource = LoadShaderSource(fragmentShaderPath);
            int fragmentShaderHandle = CreateShaderProgram(ShaderType.FragmentShader, fragmentShaderSource);

            int shaderHandle = GL.CreateProgram();
            GL.AttachShader(shaderHandle, vertexShaderHandle);
            GL.AttachShader(shaderHandle, fragmentShaderHandle);
            GL.LinkProgram(shaderHandle);

            CleanUp(shaderHandle, vertexShaderHandle, fragmentShaderHandle);
            Shader shader = new Shader(shaderHandle);

            Shaders.Add(shaderName, shader);

            return shader;
        }

        public static Shader GetShader(string shaderName)
        {
            Shaders.TryGetValue(shaderName, out Shader shader);
            return shader;
        }

        private static string LoadShaderSource(string filePath)
        {
            string shaderSource;
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }
            return shaderSource;
        }

        private static int CreateShaderProgram(ShaderType shaderType, string source)
        {
            int shaderHandle = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderHandle, source);
            GL.CompileShader(shaderHandle);

            string compileErrors = GL.GetShaderInfoLog(shaderHandle);
            if (compileErrors != string.Empty)
            {
                Console.WriteLine("Error occured while trying to load shader.");
                throw new Exception(compileErrors);
            }
            return shaderHandle;
        }

        private static void CleanUp(int shaderHandle, int vertexShaderHandle, int fragmentShaderHandle)
        {
            GL.DetachShader(shaderHandle, vertexShaderHandle);
            GL.DetachShader(shaderHandle, fragmentShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);
        }
    }
}
