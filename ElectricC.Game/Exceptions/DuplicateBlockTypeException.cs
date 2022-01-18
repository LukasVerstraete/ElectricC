using ElectricC.Game.World;

namespace ElectricC.Game.Exceptions
{
    public class DuplicateBlockTypeException: ElectricException
    {
        public DuplicateBlockTypeException(BlockType block): base(CreateExceptionMessage(block))
        {}

        private static string CreateExceptionMessage(BlockType block)
        {
            return $"Error while trying to add blockType with id: {block.id}, there is already a blockType with that id.";
        }
    }
}
