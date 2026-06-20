using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.GameObject.Block;
using Microsoft.Xna.Framework.Graphics;
using EarthianTagFormat;
using System.Threading.Tasks;

namespace Earthian.GameObject.World
{
	public class Chunk : ISaveable
	{
		private Block.Block[][,] blockData = new Block.Block[2][,];

		public virtual Block.Block[,] Blocks
		{
			get { return blockData[0]; }
			set { blockData[0] = value; }
		}

		public virtual Block.Block[,] Walls
		{
			get { return blockData[1]; }
			set { blockData[1] = value; }
		}

		private byte[,] lighting;
		public Rectangle ChunkBorder;
		private ChunkPos position;
		public static int ChunkSize = 16;
		public bool Highlight = false;
		public bool Modified = false;
		public bool Initiated = false;
		public bool ShouldDraw = true;
		public World parent;
        public bool empty = true;

		public Chunk(Block.Block[,] blockData, Block.Block[,] wallData, ChunkPos position)
		{
			this.Blocks = blockData;
			this.Walls = wallData;
			this.position = position;
			this.lighting = new byte[Chunk.ChunkSize, Chunk.ChunkSize];

			foreach (Block.Block i in BlockIterator())
			{
				if (i != null)
				{
					i.chunkIn = this;
				}
			}

			ChunkBorder = new Rectangle(position.X * 256, position.Y * 256, 256, 256);
            empty = false;
		}

		public System.Collections.IEnumerable BlockIterator()
		{
			for (int i = 0; i < 2; i++)
			{
				for (int x = 0; x < Chunk.ChunkSize; x++)
				{
					for (int y = 0; y < Chunk.ChunkSize; y++)
					{
						yield return blockData[i][x, y];
					}
				}
			}
		}

		public Chunk(ChunkPos position)
		{
			ChunkBorder = new Rectangle(position.X * 256, position.Y * 256, 256, 256);
			this.position = position;
			this.Blocks = new Block.Block[Chunk.ChunkSize, Chunk.ChunkSize];
			this.Walls = new Block.Block[Chunk.ChunkSize, Chunk.ChunkSize];
		}

		public void AssignWorld(World world)
		{
			parent = world;
		}

		public void InitWorld(World world)
		{
			parent = world;
			foreach (Block.Block i in BlockIterator())
			{
				if (i != null)
				{
					i.InitWorld(world);

				}
			}
		}

		public Block.Block GetTile(BlockPos pos, byte wallFlag = 0)
		{
			int c = Chunk.ChunkSize;
			int x = pos.X;
			int y = pos.Y;
			x %= c;
			y %= c;
			if (x < 0)
				x = c + x;
			if (y < 0)
				y = c + y;
			if (wallFlag == 0)
				return this.Blocks[x, y];
			else
				return this.Walls[x, y];
		}

		public void SetTile(BlockPos pos, Block.Block block, byte wallFlag)
		{
			int c = Chunk.ChunkSize;
			int x = pos.X;
			int y = pos.Y;
			x %= c;
			y %= c;
			if (x < 0)
				x = c + x;
			if (y < 0)
				y = c + y;
			if (block != null)
			{
				block.isWall = wallFlag == 1;
			}
			this.blockData[wallFlag][x, y] = block;
		}

		public ChunkPos GetPos()
		{
			return position;
		}

		/*
        public BlockPos GetBlockScreenPosition(Block.Block block)
        {
            for (int x = 0; x < ChunkSize; x++)
                for (int y = 0; y < ChunkSize; y++)
                    if (this.blocks[x, y] == block)
                        return new BlockPos(x * 8, y * 8);
            return null;
        }
         */

		public BlockPos GetBlockIndex(Block.Block block)
		{
			for (int x = 0; x < ChunkSize; x++)
				for (int y = 0; y < ChunkSize; y++)
					if (this.Blocks[x, y] == block)
						return new BlockPos(x, y);
			return null;
		}

		public BlockPos GetWallIndex(Block.Block block)
		{
			for (int x = 0; x < ChunkSize; x++)
				for (int y = 0; y < ChunkSize; y++)
					if (this.Walls[x, y] == block)
						return new BlockPos(x, y);
			return null;
		}

		public void PreDraw(GameTime t)
		{
			if (!ShouldDraw)
				return;
			if (!Initiated)
				return;

			foreach (Block.Block b in BlockIterator())
			{
				if (b != null)
				{
					b.chunkIn = this;
					if (b.isWall)
						b.Draw(t);
				}
			}
		}

		public void Draw(GameTime t)
		{
			if (!ShouldDraw)
				return;
			if (!Initiated)
				return;

			foreach (Block.Block b in BlockIterator())
			{
				if (b != null)
				{
					b.chunkIn = this;
					if (!b.isWall)
						b.Draw(t);
				}
			}
		}

		public void Update(GameTime gameTime, Rectangle screenHitbox)
		{
			ShouldDraw = (ChunkBorder.Intersects(screenHitbox));
			if (!ShouldDraw)
				return;
			foreach (Block.Block b in BlockIterator())
			{
				if (b != null)
				{
					b.DrawUpdate(gameTime, screenHitbox);
					b.Update(gameTime);
				}
			}
		}

		public override void readFromTag(TagCompound data)
		{
			LoadChunk(data);
		}

		public void LoadChunk(TagCompound data)
		{
			Initiated = (bool)data.GetTag("Decorated").GetValue();
            Block.Block tempBlock;
			int x, y;

			foreach (TagCompound blockData in (data.GetTag("Blocks").children()))
			{
				tempBlock = Block.Blocks.Blocks.InstantiateBlock((int)blockData.GetTag("ID").GetValue(), false);
				tempBlock.readFromTag(blockData);
				x = (int)blockData.GetTag("PosX").GetValue();
				y = (int)blockData.GetTag("PosY").GetValue();
				this.Blocks[x, y] = tempBlock;
				tempBlock.chunkIn = this;
				tempBlock.isWall = (bool)blockData.GetTag("IsWall").GetValue();
			}

			foreach (TagCompound blockData in (data.GetTag("Walls").children()))
			{
				tempBlock = Block.Blocks.Blocks.InstantiateBlock((int)blockData.GetTag("ID").GetValue(), true);
				tempBlock.readFromTag(blockData);
				x = (int)blockData.GetTag("PosX").GetValue();
				y = (int)blockData.GetTag("PosY").GetValue();
				this.Walls[x, y] = tempBlock;
				tempBlock.chunkIn = this;
			}
		}

		public void RunBlockUpdates()
		{
			foreach (var b in this.Blocks)
			{
				b.BlockSideUpdate(this.parent);
			}
		}

		public override void writeToTag(TagCompound data)
		{
			SaveChunk(data);
		}

		public void SaveChunk(TagCompound data)
		{
			TagList walls = new TagList();
			TagList blocks = new TagList();
			TagList lighting = new TagList();

			TagCompound temp;

			foreach (Block.Block b in this.BlockIterator())
			{
				if (b != null)
				{
					temp = new TagCompound();
					b.writeToTag(temp);
					if (b.isWall)
						walls.AddTag(temp);
					else
						blocks.AddTag(temp);
				}
			}

			/*
            foreach (Block.Block b in this.wallblocks)
            {
                if (b != null)
                {
                    temp = new TagCompound();
                    b.writeToTag(temp);
                    walls.AddTag(temp);
                }
            }
             */

			data.AddTag("Blocks", blocks);
			data.AddTag("Walls", walls);
			data.AddTag("Lighting", lighting);
			data.AddTag("Decorated", new TagBool(Initiated));
		}
	}
}
