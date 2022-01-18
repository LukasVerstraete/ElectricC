using ElectricC.Core.Components;
using ElectricC.ECS;
using ElectricC.Game.World.Components;
using ElectricC.Graphics.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;

namespace ElectricC.Game.World
{
    public class ChunkLoader
    {
        public static readonly int CHUNK_WIDTH = 3;
        public static readonly int CHUNK_HEIGHT = 5;
        public static readonly int WATER_HEIGHT = 60;

        public static int CreateChunk(EntityManager entityManager, WorldComponent world, float x, float y, float z)
        {
            int chunkId = entityManager.CreateEntity();
            Transform transform = new Transform();
            transform.Position = new Vector3(
                x * CHUNK_WIDTH * Block.BLOCK_WIDTH, 
                y * CHUNK_HEIGHT * Block.BLOCK_HEIGHT, 
                z * CHUNK_WIDTH * Block.BLOCK_WIDTH
            );
            entityManager.AddComponent(chunkId, transform);
            world.Chunks[new Vector3(x, y, z)] = chunkId;

            ChunkComponent chunkComponent = new ChunkComponent();
            chunkComponent.BlockIds = CreateChunkData(world, x * CHUNK_WIDTH, y * CHUNK_HEIGHT, z * CHUNK_WIDTH);
            entityManager.AddComponent(chunkId, chunkComponent);

            Mesh chunkMesh = CreateChunkMesh(entityManager, world, chunkComponent, x, y, z);
            RenderComponent renderComponent = new RenderComponent(chunkMesh);
            entityManager.AddComponent(chunkId, renderComponent);

            return chunkId;
        }

        private static int[] CreateChunkData(WorldComponent world, float worldX, float worldY, float worldZ)
        {
            int[] blockIds = new int[CHUNK_WIDTH * CHUNK_WIDTH * CHUNK_HEIGHT];
            for(int x = 0; x < CHUNK_WIDTH; x++)
            {
                for(int z = 0; z < CHUNK_WIDTH; z++)
                {
                    for(int y = 0; y < CHUNK_HEIGHT; y++)
                    {
                        blockIds[x + (y * CHUNK_WIDTH) + (z * CHUNK_WIDTH * CHUNK_HEIGHT)] = 1;
                    }
                }
            }
            return blockIds;
        }

        private static Mesh CreateChunkMesh(EntityManager entityManager, WorldComponent world, ChunkComponent chunk, float x, float y, float z)
        {
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();
            uint indexCounter = 0;

            float realChunkWidth = CHUNK_WIDTH * Block.BLOCK_WIDTH;
            float realChunkHeight = CHUNK_HEIGHT * Block.BLOCK_HEIGHT;

            float chunkX = 0;
            float chunkY;
            float chunkZ;

            float worldX = x * CHUNK_WIDTH;
            float worldY = y * CHUNK_HEIGHT;
            float worldZ = z * CHUNK_WIDTH;

            for (float meshX = 0; meshX < realChunkWidth; meshX += Block.BLOCK_WIDTH)
            {
                chunkZ = 0;
                for (float meshZ = 0; meshZ < realChunkWidth; meshZ += Block.BLOCK_WIDTH)
                {
                    chunkY = 0;
                    for (float meshY = 0; meshY < realChunkHeight; meshY += Block.BLOCK_HEIGHT)
                    {
                        BlockType block = chunk.GetBlock(chunkX, chunkY, chunkZ);
                        BlockType blockFront = GetBlockType(entityManager, world, worldX + chunkX, worldY + chunkY, worldZ + chunkZ - 1);
                        BlockType blockBack = GetBlockType(entityManager, world, worldX + chunkX, worldY + chunkY, worldZ + chunkZ + 1);
                        BlockType blockLeft = GetBlockType(entityManager, world, worldX + chunkX - 1, worldY + chunkY, worldZ + chunkZ);
                        BlockType blockRight = GetBlockType(entityManager, world, worldX + chunkX + 1, worldY + chunkY, worldZ + chunkZ);
                        BlockType blockTop = GetBlockType(entityManager, world, worldX + chunkX, worldY + chunkY + 1, worldZ + chunkZ);
                        BlockType blockBottom = GetBlockType(entityManager, world, worldX + chunkX, worldY + chunkY - 1, worldZ + chunkZ);

                        if (block.solid)
                        {
                            if (!blockFront.solid)
                                indexCounter = CreateFrontFace(meshX, meshY, meshZ, indexCounter, vertices, indices);

                            if (!blockBack.solid)
                                indexCounter = CreateBackFace(meshX, meshY, meshZ, indexCounter, vertices, indices);

                            if (!blockLeft.solid)
                                indexCounter = CreateLeftFace(meshX, meshY, meshZ, indexCounter, vertices, indices);

                            if (!blockRight.solid)
                                indexCounter = CreateRightFace(meshX, meshY, meshZ, indexCounter, vertices, indices);

                            if (!blockBottom.solid)
                                indexCounter = CreateBottomFace(meshX, meshY, meshZ, indexCounter, vertices, indices);

                            if (!blockTop.solid)
                                indexCounter = CreateTopFace(meshX, meshY, meshZ, indexCounter, vertices, indices);
                        }
                        chunkY++;
                    }
                    chunkZ++;
                }
                chunkX++;
            }

            Texture girlTexture = TextureLoader.LoadTexture("./Assets/Textures/Leen.jpg");
            return new Mesh(vertices.ToArray(), indices.ToArray(), girlTexture);
        }

        private static uint CreateFrontFace(float x, float y, float z, uint indexCounter, List<float> vertices, List<uint> indices)
        {
            vertices.AddRange(new float[] { x, y, z, 0, 1 });
            vertices.AddRange(new float[] { x, y + Block.BLOCK_HEIGHT, z, 0, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y + Block.BLOCK_HEIGHT, z, 1, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y, z, 1, 1 });

            indices.AddRange(new uint[] { 
                indexCounter, 
                indexCounter + 1, 
                indexCounter + 2, 
                indexCounter, 
                indexCounter + 2, 
                indexCounter + 3 
            });

            return indexCounter + 4;
        }

        private static uint CreateBackFace(float x, float y, float z, uint indexCounter, List<float> vertices, List<uint> indices)
        {
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y, z + Block.BLOCK_WIDTH, 0, 1 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y + Block.BLOCK_HEIGHT, z + Block.BLOCK_WIDTH, 0, 0 });
            vertices.AddRange(new float[] { x, y + Block.BLOCK_HEIGHT, z + Block.BLOCK_WIDTH, 1, 0 });
            vertices.AddRange(new float[] { x, y, z + Block.BLOCK_WIDTH, 1, 1 });

            indices.AddRange(new uint[] {
                indexCounter,
                indexCounter + 1,
                indexCounter + 2,
                indexCounter,
                indexCounter + 2,
                indexCounter + 3
            });

            return indexCounter + 4;
        }

        private static uint CreateLeftFace(float x, float y, float z, uint indexCounter, List<float> vertices, List<uint> indices)
        {
            vertices.AddRange(new float[] { x, y, z + Block.BLOCK_WIDTH, 0, 1 });
            vertices.AddRange(new float[] { x, y + Block.BLOCK_HEIGHT, z + Block.BLOCK_WIDTH, 0, 0 });
            vertices.AddRange(new float[] { x, y + Block.BLOCK_HEIGHT, z, 1, 0 });
            vertices.AddRange(new float[] { x, y, z, 1, 1 });

            indices.AddRange(new uint[] {
                indexCounter,
                indexCounter + 1,
                indexCounter + 2,
                indexCounter,
                indexCounter + 2,
                indexCounter + 3
            });

            return indexCounter + 4;
        }

        private static uint CreateRightFace(float x, float y, float z, uint indexCounter, List<float> vertices, List<uint> indices)
        {
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y, z, 0, 1 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y + Block.BLOCK_HEIGHT, z, 0, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y + Block.BLOCK_HEIGHT, z + Block.BLOCK_WIDTH, 1, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y, z + Block.BLOCK_WIDTH, 1, 1 });

            indices.AddRange(new uint[] {
                indexCounter,
                indexCounter + 1,
                indexCounter + 2,
                indexCounter,
                indexCounter + 2,
                indexCounter + 3
            });

            return indexCounter + 4;
        }

        private static uint CreateTopFace(float x, float y, float z, uint indexCounter, List<float> vertices, List<uint> indices)
        {
            vertices.AddRange(new float[] { x, y + Block.BLOCK_HEIGHT, z, 0, 1 });
            vertices.AddRange(new float[] { x, y + Block.BLOCK_HEIGHT, z + Block.BLOCK_WIDTH, 0, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y + Block.BLOCK_HEIGHT, z + Block.BLOCK_WIDTH, 1, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y + Block.BLOCK_HEIGHT, z, 1, 1 });

            indices.AddRange(new uint[] {
                indexCounter,
                indexCounter + 1,
                indexCounter + 2,
                indexCounter,
                indexCounter + 2,
                indexCounter + 3
            });

            return indexCounter + 4;
        }

        private static uint CreateBottomFace(float x, float y, float z, uint indexCounter, List<float> vertices, List<uint> indices)
        {
            vertices.AddRange(new float[] { x, y, z + Block.BLOCK_WIDTH, 0, 1 });
            vertices.AddRange(new float[] { x, y, z, 0, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y, z, 1, 0 });
            vertices.AddRange(new float[] { x + Block.BLOCK_WIDTH, y, z + Block.BLOCK_WIDTH, 1, 1 });

            indices.AddRange(new uint[] {
                indexCounter,
                indexCounter + 1,
                indexCounter + 2,
                indexCounter,
                indexCounter + 2,
                indexCounter + 3
            });

            return indexCounter + 4;
        }

        public static BlockType GetBlockType(EntityManager entityManager, WorldComponent world, float x, float y, float z)
        {
            float chunkX = (float)Math.Floor(x / CHUNK_WIDTH);
            float chunkY = (float)Math.Floor(y / CHUNK_HEIGHT);
            float chunkZ = (float)Math.Floor(z / CHUNK_WIDTH);

            if(world.Chunks.TryGetValue(new Vector3(chunkX, chunkY, chunkZ), out int chunkId))
            {
                ChunkComponent chunk;
                chunk = entityManager.GetComponent<ChunkComponent>(chunkId);

                return chunk.GetBlock(
                    x - (chunkX * CHUNK_WIDTH),
                    y - (chunkY * CHUNK_HEIGHT),
                    z - (chunkZ * CHUNK_WIDTH)
                );
            }
            return BlockRegistry.GetBlockType(0);
        }
    }
}
