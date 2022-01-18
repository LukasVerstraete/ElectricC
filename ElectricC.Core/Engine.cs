using ElectricC.Core.Objects;
using ElectricC.Core.Systems;
using ElectricC.ECS;
using ElectricC.Graphics.Renderer;
using ElectricC.Graphics.Shaders;
using ElectricC.Graphics.Window;
using ElectricC.Input;
using SDL2;

namespace ElectricC.Core
{
    public class Engine
    {
        public ElectricWindow Window { get; set; }
        public Renderer Renderer { get; set; }
        public EventLoop SDLEventHandler { get; set; }
        public EntityManager EntityManager { get; set; }
        public InputManager InputManager { get; set; }

        public int CameraId;

        private bool running = false;
        private IGame game;

        private int FPSCounter = 0;
        private float FPSTimeCounter = 0;

        public Engine(IGame game, string name, int width, int height)
        {
            this.game = game;
            Window = new ElectricWindow(name, width, height);
            Renderer = new Renderer(Window);
            InputManager = new InputManager();
            EntityManager = new EntityManager();
        }

        public void Init()
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            Window.Init();
            Renderer.Init();
            SDLEventHandler = new EventLoop(this);
            InitObjects();
            game.Initialize(this);
            EntityManager.OnLoad();
            running = true;
            Run();
        }

        public void InitObjects()
        {
            ShaderLoader.LoadShader("Basic");
            CameraSystem cameraSystem = new CameraSystem();
            BasicRenderSystem renderer = new BasicRenderSystem();
            MovementSystem movementSystem = new MovementSystem();
            PlayerControlSystem playerControlSystem = new PlayerControlSystem();
            EntityManager.AddSystem(cameraSystem);
            EntityManager.AddSystem(renderer);
            EntityManager.AddSystem(movementSystem);
            EntityManager.AddSystem(playerControlSystem);

            CameraId = CameraBuilder.PerspectiveCamera(EntityManager, Window);
        }

        private void Run()
        {
            FrameTimer.Timer.EndFrame();
            while(running)
            {
                Input();
                Update(FrameTimer.Timer.DeltaTime());
                Render();
                FrameTimer.Timer.EndFrame();
            }
            Window.Close();
            SDL.SDL_Quit();
        }

        public void Input()
        {
            SDLEventHandler.Update();
            EntityManager.Input(InputManager);
        }

        public void Update(float deltaTime)
        {
            InputManager.Update();
            EntityManager.Update(deltaTime);
        }

        public void Render()
        {
            Renderer.Prepare();
            EntityManager.Render();
            Renderer.Render();
        }

        public void Stop()
        {
            running = false;
        }
    }
}
