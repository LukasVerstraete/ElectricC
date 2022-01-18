using ElectricC.Game.World;

namespace ElectricC.Game.Exceptions
{
    public class UnknownBlockTypeException: ElectricException
    {
        public UnknownBlockTypeException(int blockId): base(CreateExceptionMessage(blockId)) { }

        private static string CreateExceptionMessage(int blockId)
        {
            return $"Error while trying to fetch blockType with id: {blockId}. There is no blockType registered with that id.";
        }
    }
}
