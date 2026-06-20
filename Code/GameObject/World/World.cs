using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.Utilities;
using EarthianTagFormat;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.World
{
    public class World : ISaveable
    {
        private Random rand = new Random();
        private ChunkProvider chunks;
        private BlockPos spawnPoint;
        public List<Entity.Entity> entities;
        public ChunkPos chunkPIn;
        public Entity.Entity player;
        private Rectangle screenHitbox;
        private Chunk lastChunk;
        private ChunkPos lastChunkPos;
        private Texture2D debug;

        List<Chunk> nearChunks;

        public World()
        {
            entities = new List<Entity.Entity>();
            this.spawnPoint = new BlockPos(0, 0);
            this.entities.Add(new Entity.Entities.EntityPlayer("Player", null, this));
            this.SpawnPlayer(this.entities[0]);
            this.player = this.entities[0];
            nearChunks = new List<Chunk>();
        }

        public void Init()
        {
            this.chunks = new ChunkProvider(this, new WorldFile("TestWorld"));
            this.PrepareSpawn();
            this.debug = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/DAYGrad");
        }

        public void PrepareSpawn()
        {
            this.SpawnEntity(new Entity.Entities.EntityFloater(), new Vector2(0, 0));
            foreach (ChunkPos c in new ChunkPos(0, 0).GetAdjacentChunks(3, 3))
            {
                nearChunks.Add(this.GetChunk(c));
            }
        }

        public void Update(GameTime gameTime)
        {
                float zoom = Runtime.Runtime.thisRuntime.mCamera.Zoom;
                int sw = Game1.thisGame.Window.ClientBounds.Width;
                int sh = Game1.thisGame.Window.ClientBounds.Height;
                Vector2 cp = Runtime.Runtime.thisRuntime.mCamera.Pos;
                this.screenHitbox = new Rectangle((int)(cp.X - (sw / 2) / zoom), (int)(cp.Y - (sh / 2) / zoom), (int)(sw / zoom * 1.05), (int)(sh / zoom * 1.05));
                chunkPIn = chunks.getChunkPlayerPos((Entity.Entities.EntityPlayer)player);
                foreach (ChunkPos p in chunkPIn.GetAdjacentChunks(3, 3))
                {
                    Chunk c = this.GetChunk(p);
                    if (chunks.ShouldGenerate(p))
                    {
                        nearChunks.Add(c);
                    }
                }
                lock (this.chunks.loadedChunks)
                {
                    List<Chunk> chunks_ = this.chunks.loadedChunks.Values.ToList<Chunk>();
                    foreach (Chunk c in chunks_)
                    {
                        c.Update(gameTime, screenHitbox);
                        if (!nearChunks.Contains(c))
                        {
                            chunks.DropChunk(c.GetPos());
                        }
                    }
                    nearChunks = new List<Chunk>();
                    UpdateEntities(gameTime);
                }
        }

        public void Draw(GameTime gameTime)
        {
            Drawing.NewBatch(true);
            foreach (Chunk c in chunks.loadedChunks.Values)
            {
                c.PreDraw(gameTime);
            }
            Drawing.EndBatch();
            DrawEntities(gameTime);
            Drawing.NewBatch(true);
            foreach (Chunk c in chunks.loadedChunks.Values)
            {
                c.Draw(gameTime);
            }
            BlockPos p = this.GetMouseOver();
            Game1.thisGame.spriteBatch.Draw(this.debug, new Rectangle(p.X * 16, p.Y * 16, 16, 16), Color.Fuchsia);
            Drawing.EndBatch();

        }

        public Chunk GetChunk(ChunkPos pos)
        {
            if (lastChunkPos != null && pos.X == lastChunkPos.X && pos.Y == lastChunkPos.Y)
            {
                return lastChunk;
            }
            lastChunkPos = pos;
            lastChunk = this.chunks.GetChunk(pos);
            CheckInit(lastChunk);
            return lastChunk;
        }

        public void CheckInit(Chunk c)
        {
            if (c.Initiated)
                return;
            foreach (BlockPos p in BlockPos.GetAdjacentSides(c.GetPos().ToBlockPos(true)))
            {
                if (!IsChunkLoaded(p))
                    return;
            }
            c.Initiated = true;
            c.InitWorld(this);
            chunks.DecorateChunk(c);
        }

        public Block.Block GetBlock(BlockPos pos, byte wallFlag = 0)
        {
            Chunk c = this.GetLoadedChunk(pos.ToChunkPos());
            if (c != null)
                return c.GetTile(pos, wallFlag);
            else
                return null;
        }

        public void SetBlock(BlockPos pos, Block.Block block, byte wallFlag = 0)
        {
            Chunk c = this.GetLoadedChunk(pos.ToChunkPos());
            if (c != null)
            {
                c.Modified = true;
                c.SetTile(pos, block, wallFlag);
            }
            if (block != null)
            {
                block.BlockPosition = pos;
                block.chunkIn = c;
                block.InitWorld(this);
            }
            foreach (BlockPos p in BlockPos.GetAdjacentSides(pos))
            {
                Block.Block b = this.GetBlock(p, wallFlag);
                if (b != null)
                {
                    b.BlockSideUpdate(this);
                }
            }
        }

        public bool IsChunkLoaded(ChunkPos pos)
        {
            return (this.chunks.findIndexedChunk(pos) != null);
        }

        public Chunk GetLoadedChunk(ChunkPos pos)
        {
            return chunks.findIndexedChunk(pos);
        }

        public Chunk GetChunkFromBlock(BlockPos pos)
        {
            return this.GetChunk(pos.ToChunkPos());
        }

        public void SpawnPlayer(Entity.Entity player)
        {
            player.SpawnEntity(player, this, this.spawnPoint.GetPosition());
        }

        public void SpawnEntity(Entity.Entity ent, Vector2 position)
        {
            ent.Init();
            ent.SpawnEntity(ent, this, position);
            AddEntity(ent);
        }

        public void AddEntity(Entity.Entity ent)
        {
            entities.Add(ent);
        }

        public void UpdateEntities(GameTime gameTime)
        {
            List<Entity.Entity> toRemove = new List<Entity.Entity>();
            Entity.Entity e;
            for (int i = 0; i < entities.Count; i++)
            {
                e = entities[i];
                if (e.IsDead)
                {
                    toRemove.Add(e);
                }
                else
                {
                    e.Update(gameTime);
                }
            }
            foreach (Entity.Entity e_ in toRemove)
            {
                entities.Remove(e_);
            }
        }

        public void DrawEntities(GameTime gameTime)
        {
            foreach (Entity.Entity entity in entities)
            {
                Drawing.NewBatch(true);
                entity.Draw(gameTime);
                Drawing.EndBatch();
            }
        }

        public BlockPos GetPositionBlock(Vector2 position)
        {
            int x = (int)Math.Floor((position.X) / 16.0);
            int y = (int)Math.Floor((position.Y) / 16.0);
            return new BlockPos(x + 1, y + 2);
        }

        public float GetGravity()
        {
            return 0.5f;
        }

        public BlockPos GetMouseOver()
        {
            Vector2 mp = Input.GetMousePos();
            int x = (int)Math.Floor((mp.X - Game1.thisGame.Window.ClientBounds.Width / 2) / (16 * Runtime.Runtime.thisRuntime.mCamera.Zoom) + (this.player.Position.X + 8) / 16); //replace player with camera focus.
            int y = (int)Math.Floor((mp.Y - Game1.thisGame.Window.ClientBounds.Height / 2) / (16 * Runtime.Runtime.thisRuntime.mCamera.Zoom) + this.player.Position.Y / 16);
            return new BlockPos(x + 1, y + 2);
        }

        public override void readFromTag(TagCompound data)
        {
            throw new NotImplementedException();
        }

        public override void writeToTag(TagCompound data)
        {
            throw new NotImplementedException();
        }
    }
}
