using System;

namespace ElectricC.Game.Exceptions
{
    public class ElectricException: Exception
    {
        public ElectricException()
        {}

        public ElectricException(string message): base(message)
        {}

        public ElectricException(string message, Exception inner): base(message, inner)
        {}
    }
}
