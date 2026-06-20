using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.Block;
using Earthian.GameObject.World;
using Earthian.GameObject.Block.Blocks;
using Earthian.Utilities;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.World.Generator
{
	public class OverworldGenerator : WorldGenerator
	{


		#region Variables

		private string seed;
		//private float[,] blockPlacement = new float[Chunk.ChunkSize, Chunk.ChunkSize];

		#endregion


		#region Constructors

		public OverworldGenerator(string seed)
			: base(seed)
		{
			this.seed = seed;
		}

		#endregion


		#region Functions

		public override Chunk GenerateChunk(ChunkPos pos)
		{
			Tuple<int, int>[,] blockPlacement = new Tuple<int, int>[Chunk.ChunkSize, Chunk.ChunkSize];
			Block.Block[,] chunkTile = new Block.Block[Chunk.ChunkSize, Chunk.ChunkSize];
			Block.Block[,] wallTile = new Block.Block[Chunk.ChunkSize, Chunk.ChunkSize];
			Noise noise1 = new Noise(0.1f, 0.1f, 1);
			Noise noise2 = new Noise(0.05f, 5);
			int chunkBaseX = pos.X * Chunk.ChunkSize;
			int chunkBaseY = pos.Y * Chunk.ChunkSize;


			for (int x = 0; x < Chunk.ChunkSize; x++)
			{
				for (int y = 0; y < Chunk.ChunkSize; y++)
				{
					blockPlacement[y, x] = Magic_Algorithm_Block(noise1, noise2, chunkBaseX + x, chunkBaseY + y);

				}
			}

			//setup the chunk being generated
			for (int x = 0; x < Chunk.ChunkSize; x++)
			{
				for (int y = 0; y < Chunk.ChunkSize; y++)
				{
					if (blockPlacement[x, y].Item1 > 0)
						chunkTile[(int)y, (int)x] = Blocks.InstantiateBlock(blockPlacement[x, y].Item1, false);
					if (blockPlacement[x, y].Item2 > 0)
					{
						wallTile[(int)y, (int)x] = Blocks.InstantiateBlock(blockPlacement[x, y].Item2, true);
					}

					//Console.WriteLine(blockPlacement[x,y]);
				}
			}

			return new Chunk(chunkTile, wallTile, new ChunkPos(pos.X, pos.Y));
		}

		#endregion

		public Tuple<int, int> Magic_Algorithm_Block(Noise noise, Noise noise2, int x, int y)
		{
			int[] result = new int[2];
			int ra = 1;
			int rb = 1;
			int x2 = x - (x % 512);
			int y2 = y - (y % 480);//(int)Math.Floor(y / 1024f);
			//is there an island in this region?
			bool MainIsland = (x2 == 0 && y2 == 0);
			bool I = true;// MainIsland || noise2.GetNoise(x2, y2, 0) > 0.5;
			if (I)
			{
				//if there is, how big is the island?
				int w = 80 + (int)Math.Abs(noise.GetNoise(x2, y2, 1) * 512);
				int h = 50 + (int)Math.Abs(noise.GetNoise(x2, y2, 2) * 480);
				int Ix = 0;
				int Iy = 64;
				//randomly jiggle the island, but only if it isn't the spawn island
				if (!MainIsland)
				{
					//Ix = (int)(noise.GetNoise(x2, y2, 3) * 1500);
					//Iy = (int)Math.Abs(noise.GetNoise(x2, y2, 4) * 750);
				}
                

				//set up the dimensions of the island
				Rectangle top = new Rectangle(x2 + Ix - w / 2, y2 + Iy, w, 16);
				Rectangle bottom = new Rectangle(x2 + Ix - w / 2, y2 + Iy + h - 16, w, w / 2 + 16);
				Rectangle middle = new Rectangle(x2 + Ix - w / 2, y2 + Iy + 16, w, h - 16);

				Point p = new Point(x, y);

				//smooth the island by seeing if the block is in a circle
				if (!WithinRadius(middle.Center.ToVector2(), p.ToVector2(), w / 2.1f))
					return new Tuple<int, int>(result[0], result[1]);

				if (middle.Contains(p)) //generate the middle of the island
				{
					
					if (!(noise2.GetNoise(x, y, x2) > 0.5))
					{
						result[0] = 1;
					}
					result[1] = 3;
					
					return new Tuple<int, int>(result[0], result[1]);
				}
				else if (y <= middle.Top && x > middle.Left && x < middle.Right) //generate the top of the island
				{
					float surfaceFreq = 0.02f;
					//if (noise2.GetNoise(x, y, x2) > 0.7) return 0f;
					int surface = (int)(Math.Abs(noise.GetNoise(x * surfaceFreq)) * 32) - 32;
					if (y > y2 + Iy + surface)
					{
						result[0] = 3;
					}
					return new Tuple<int, int>(result[0], result[1]);
				}
				else if (y >= middle.Bottom && x > middle.Left && x < middle.Right) //generate the bottom of the island
				{
					int surface = (int)(Math.Abs(noise.GetNoise(x)) * 32) - 6;
					int i = w / 2 - Math.Abs(middle.Center.X - x);
					int i2 = (int)(Math.Sin(x) * 2);
					if (y < y2 + Iy + h + surface + i / 2 + i2 - 16)
					{
						if (noise2.GetNoise(x, y, x2) < 0.6)
						{
							result[0] = 1;
						}
						return new Tuple<int, int>(result[0], result[1]);
					}
					if (y < y2 + Iy + h + surface + i / 2 + i2)
					{
						result[0] = 3;
					}
					return new Tuple<int, int>(result[0], result[1]);
				}
			}
			return new Tuple<int, int>(result[0], result[1]);
		}

		public bool WithinRadius(Vector2 center, Vector2 point, float distance)
		{
			return Vector2.Distance(center, point) < distance;
		}
	}
}
