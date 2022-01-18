using static SDL2.SDL;

namespace ElectricC.Core
{
    public class EventLoop
    {
        public Engine Engine { get; set; }

        public EventLoop(Engine engine)
        {
            Engine = engine;
        }

        public void Update()
        {
            while(SDL_PollEvent(out SDL_Event sdlEvent) != 0)
            {
                switch (sdlEvent.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        Engine.Stop();
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        Engine.InputManager.SetKeyState(sdlEvent.key.keysym.sym, true);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        Engine.InputManager.SetKeyState(sdlEvent.key.keysym.sym, false);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION:
                        Engine.InputManager.SetMouseDelta(sdlEvent.motion.xrel, sdlEvent.motion.yrel);
                        Engine.InputManager.SetMousePosition(sdlEvent.motion.x, sdlEvent.motion.y);
                        break;
                }
            }
        }
    }
}
