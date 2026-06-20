using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.Block.Blocks;
using Earthian.GameObject.Block;
using Earthian.GameObject.World.Generator;
using Microsoft.Xna.Framework;
using System.Threading;
using EarthianTagFormat;
using Earthian.GameObject.World.Generator.Feature;
using System.Threading.Tasks;

namespace Earthian.GameObject.World
{
    public class ChunkProvider
    {
        private WorldFile worldFile;
        private WorldGenerator worldGen;
        public Dictionary<int[], Chunk> loadedChunks = new Dictionary<int[], Chunk>();
        private World parent;
        public BiomeDecorator decorator;

        public ChunkProvider(World parent, WorldFile file)
        {
            decorator = new BiomeDecorator();
            decorator.AddFeature(new OreGenerator(5,Blocks.InstantiateBlock(3,false)));

            this.parent = parent;
            this.worldFile = file;
            this.worldGen = new RealWorldGenerator("Q");

            ChunkWorkerThread();
        }

        public bool ShouldGenerate(ChunkPos pos)
        {
            if (loadedChunks.Keys.Contains(new int[2] { (int)pos.X, (int)pos.Y })) 
                return false;
            else
                return true;
        }

        public Chunk GetChunk(ChunkPos pos)
        {
            Chunk c = findIndexedChunk(pos);
            if (c != null) return c;
            Chunk chunkData = new Chunk(pos);
            lock (loadedChunks)
            {
                this.loadedChunks.Add(new int[2] { (int)pos.X, (int)pos.Y }, chunkData);
            }
            chunkData.AssignWorld(parent);
            return chunkData;
        }

        public async Task<bool> ChunkWorkerThread()
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                List<Chunk> l;
                Chunk d;
                while (true)
                { //shouldRun
                    l = new List<Chunk>();
                    lock (loadedChunks)
                    {
                        foreach (KeyValuePair<int[],Chunk> k in loadedChunks)
                        {
                            d = k.Value;
                            if (d != null && d.empty)
                            {
                                l.Add(d);
                            }
                        }
                    }
                    foreach (Chunk c in l)
                    {
                        ChunkPos pos = c.GetPos();
                        if (!worldFile.ChunkExists(pos))
                        {
                            Chunk chunkData = this.GenerateChunk(pos);
                            chunkData.Modified = true;
                            c.Blocks = chunkData.Blocks;
                            c.Walls = chunkData.Walls;
						}
                        else
                        {
                            TagCompound data = this.worldFile.GetChunkData(pos);
                            c.LoadChunk(data);
                        }
                        c.empty = false;
                    }
                }
                return true;
            });
        }

        public Chunk LoadChunkData(ChunkPos pos, TagCompound Data)
        {
            Chunk c = new Chunk(pos);
            c.readFromTag(Data);
            return c;
        }

        public ChunkPos getChunkPlayerPos(Entity.Entities.EntityPlayer player)
        {
            foreach (Chunk c in loadedChunks.Values)
                if (c.ChunkBorder.Intersects(player.Hitbox))
                    return new ChunkPos(c.GetPos().X, c.GetPos().Y);
            return new ChunkPos(0, 0);
        }

        public Chunk findIndexedChunk(ChunkPos pos)
        {
            foreach (int[] key in loadedChunks.Keys)
            {
                if (key[0] == pos.X && key[1] == pos.Y)
                {
                    return loadedChunks[key];
                }
            }
            return null;
        }

        public void DropChunk(ChunkPos pos)
        {
            int[] keyToRemove = null;
            foreach (int[] k in loadedChunks.Keys)
            {
                if (k[0] == pos.X)
                    if (k[1] == pos.Y)
                    {
                        keyToRemove = k;
                    }
            }
            if (keyToRemove != null)
            {
                Chunk c = loadedChunks[keyToRemove];
                loadedChunks.Remove(keyToRemove);
                if (c.Modified)
                {
                    this.SaveChunk(pos, c);
                }
            }
        }

        public Chunk GenerateChunk(ChunkPos pos)
        {
            return worldGen.GenerateChunk(pos);
        }

        public async void SaveChunk(ChunkPos pos, Chunk chunk)
        {
            TagCompound ChunkData = new TagCompound();
            chunk.writeToTag(ChunkData);
            worldFile.SaveChunkData(pos,ChunkData);
        }

        public void DecorateChunk(Chunk c)
        {
            BlockPos p = c.GetPos().ToBlockPos();
        }
    }
}
