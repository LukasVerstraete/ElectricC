using ElectricC.Core;
using System;

namespace ElectricC.Game
{
    public class Program
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine(new Electric(), "Electric", 1080, 720);
            engine.Init();
        }
    }
}
