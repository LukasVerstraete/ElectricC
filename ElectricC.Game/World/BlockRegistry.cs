using ElectricC.Game.Exceptions;
using System.Collections.Generic;

namespace ElectricC.Game.World
{
    public static class BlockRegistry
    {
        private static Dictionary<int, BlockType> blockTypes = new Dictionary<int, BlockType>();

        public static void AddBlockType(BlockType block)
        {
            if(blockTypes.ContainsKey(block.id))
            {
                throw new DuplicateBlockTypeException(block);
            }

            blockTypes[block.id] = block;
        }

        public static BlockType GetBlockType(int id)
        {
            if(!blockTypes.ContainsKey(id))
            {
                throw new UnknownBlockTypeException(id);
            }

            return blockTypes[id];
        }
    }
}
