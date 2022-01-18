using System.Collections.Generic;
using static SDL2.SDL;

namespace ElectricC.Input
{
    public class InputManager
    {
        private Dictionary<SDL_Keycode, bool> keys;
        private Dictionary<SDL_Keycode, bool> pressed;

        public bool IsMouseLocked { get; private set; }
        private float mousePositionX = 0;
        private float mousePositionY = 0;

        public float MouseDeltaX { get; private set; }
        public float MouseDeltaY { get; private set; }

        public InputManager()
        {
            keys = new Dictionary<SDL_Keycode, bool>();
            pressed = new Dictionary<SDL_Keycode, bool>();
            IsMouseLocked = false;
        }

        public void Update()
        {
            pressed.Clear();
            ResetMouseDelta();
        }

        public void SetKeyState(SDL_Keycode key, bool isDown)
        {
            keys[key] = isDown;
            if (!isDown) 
            {
                SetKeyPressed(key);
            }
        }

        public void SetKeyPressed(SDL_Keycode key)
        {
            pressed[key] = true;
        }

        public bool IsDown(SDL_Keycode key)
        {
            if(keys.ContainsKey(key))
            {
                return keys[key];
            }
            return false;
        }

        public bool IsPressed(SDL_Keycode key)
        {
            if(pressed.ContainsKey(key))
            {
                return pressed[key];
            }
            return false;
        }

        public void SetMouseDelta(float x, float y)
        {
            MouseDeltaX = x;
            MouseDeltaY = y;
        }

        private void ResetMouseDelta()
        {
            MouseDeltaX = 0;
            MouseDeltaY = 0;
        }

        public void SetMousePosition(float x, float y)
        {
            mousePositionX = x;
            mousePositionY = y;
        }

        public void SwitchMouseLock()
        {
            if(IsMouseLocked)
            {
                UnLockMouse();
            } 
            else
            {
                LockMouse();
            }
        }

        public void LockMouse()
        {
            IsMouseLocked = true;
            SDL_SetRelativeMouseMode(SDL_bool.SDL_TRUE);
        }

        public void UnLockMouse()
        {
            IsMouseLocked = false;
            SDL_SetRelativeMouseMode(SDL_bool.SDL_FALSE);
        }
    }

    public static class Keys
    {
        public static readonly SDL_Keycode FORWARD = SDL_Keycode.SDLK_z;
        public static readonly SDL_Keycode BACKWARD = SDL_Keycode.SDLK_s;
        public static readonly SDL_Keycode LEFT = SDL_Keycode.SDLK_q;
        public static readonly SDL_Keycode RIGHT = SDL_Keycode.SDLK_d;
        public static readonly SDL_Keycode UP = SDL_Keycode.SDLK_SPACE;
        public static readonly SDL_Keycode DOWN = SDL_Keycode.SDLK_LSHIFT;
        public static readonly SDL_Keycode LOCK = SDL_Keycode.SDLK_l;
    }
}
