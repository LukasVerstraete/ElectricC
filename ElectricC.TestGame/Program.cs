using ElectricC.Core;

namespace ElectricC.TestGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Engine(new TestGame(), "TestGame", 1080, 720);
            engine.Init();
        }
    }
}
