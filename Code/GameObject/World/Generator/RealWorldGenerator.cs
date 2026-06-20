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
	public class RealWorldGenerator : WorldGenerator
	{


		#region Variables

		private string seed;
		//private float[,] blockPlacement = new float[Chunk.ChunkSize, Chunk.ChunkSize];

		#endregion


		#region Constructors

		public RealWorldGenerator(string seed)
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

			try
			{
				if (y > -32)
				{
					float surfaceFreq = 0.01f;
					//if (noise2.GetNoise(x, y, x2) > 0.7) return 0f;

					int surface = (int)(Math.Abs(noise.GetNoise(x * surfaceFreq)) * 32)+32;
					surfaceFreq = 0.7f;
					int surfaceB = (int)(Math.Abs(noise.GetNoise(x * surfaceFreq)) * 2);
					surfaceFreq = 1f;
					int surfaceC = (int)(Math.Abs(noise.GetNoise(x * surfaceFreq)) * 4);
					if (y > surface + surfaceB)
					{
						result[0] = 3;
						if (y > (surface + surfaceB) + 1)
						{
							result[1] = 3;
						}
					}
					if (y > ((surface + surfaceB) - surfaceC) + 8)
					{
						result[0] = 1;
						result[1] = 1;
					}
					if (y > (surface + 32))
					{
						if (!(noise2.GetNoise(x, y, x2) > 0.5))
						{
							result[0] = 1;
						}
						result[1] = 1;
					}
					return new Tuple<int, int>(result[0], result[1]);
				}
				return new Tuple<int, int>(result[0], result[1]);
			}
			catch (Exception ex) {
				throw ex;
			}
		}
	}
}
