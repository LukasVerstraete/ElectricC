using ElectricC.ECS;

namespace ElectricC.Game.World.Components
{
    public class ChunkComponent: Component
    {
        public int[] BlockIds { get; set; }

        public BlockType GetBlock(float x, float y, float z)
        {
            int blockId = BlockIds[(int)x + ((int)y * ChunkLoader.CHUNK_WIDTH) + ((int)z * ChunkLoader.CHUNK_WIDTH * ChunkLoader.CHUNK_HEIGHT)];
            //x + y*dx + z*dx*dy
            //x + y * max_x + z * max_x * max_y
            return BlockRegistry.GetBlockType(blockId);
        }
    }
}
