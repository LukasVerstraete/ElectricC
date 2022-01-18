using ElectricC.Graphics.Window;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SDL2;
using System;

namespace ElectricC.Graphics.Renderer
{
    public class Renderer
    {
        public IntPtr SDLOpenGLContext { get; set; }
        public GraphicsContext OpenTKGraphicsContext { get; set; }
        public ElectricWindow Window { get; set; }

        public Random random;

        public Renderer(ElectricWindow window)
        {
            Window = window;
            random = new Random();
        }

        public void Init()
        {
            SDLOpenGLContext = SDL.SDL_GL_CreateContext(Window.Handle);
            OpenTKGraphicsContext = new GraphicsContext(
                new ContextHandle(SDLOpenGLContext),
                (proc) => SDL.SDL_GL_GetProcAddress(proc),
                () => new ContextHandle(SDL.SDL_GL_GetCurrentContext())
            );
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.8f, 1));
            GL.Enable(EnableCap.DepthTest);
        }

        public void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Render()
        {
            //OpenTKGraphicsContext.SwapBuffers();
            SDL.SDL_GL_SwapWindow(Window.Handle);
        }
    }
}
