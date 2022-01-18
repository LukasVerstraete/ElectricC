using SDL2;
using System;

namespace ElectricC.Graphics.Window
{
    public class ElectricWindow
    {
        public IntPtr Handle { get; set; }
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ElectricWindow(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
        }

        public void Init()
        {
            Handle = SDL.SDL_CreateWindow(
                Title,
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                Width,
                Height,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL
            );
        }

        public void Close()
        {
            SDL.SDL_DestroyWindow(Handle);
        }
    }
}
