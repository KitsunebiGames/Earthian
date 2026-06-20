using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Earthian.Utilities;
using Earthian.GameObject.World;
using Earthian.GameObject.Item;
using Earthian.Runtime.Screens;
using EarthianTagFormat;
using System.Threading.Tasks;

namespace Earthian.GameObject.Block
{
	enum FaceDirection
	{
		NORTH,
		SOUTH,
		EAST,
		NORTHEAST,
		WEST,
		SOUTHEAST,
		NORTHWEST,
		SOUTHWEST,
		TAKEN
	}


	public abstract class Block : ISaveable
	{
		public Color c = Color.White;

		public virtual TileTexturePosition target { get; set; }

		public virtual Texture2D texture { get; set; }

		public virtual Chunk chunkIn { get; set; }

		public virtual Rectangle hitbox
		{
			get
			{
				if (!isWall)
					return new Rectangle(this.PosX, this.PosY, 16, 16);
				else

					return new Rectangle(this.PosX-6, this.PosY-6, 16+12, 16+12);
			}
		}

		public virtual int TileX { get { return BlockPosition.X; } }

		public virtual int TileY { get { return BlockPosition.Y; } }

		public virtual BlockPos BlockPosition
		{
			get
			{
				if (_blockpos == null)
				{
					_blockpos = GetIndex().add(chunkIn.GetPos().ToBlockPos());
				}
				return _blockpos;
			}
			set
			{
				_blockpos = value;
			}
		}

		private BlockPos _blockpos;

		public virtual int blockSize { get; set; }

		public virtual string name { get; set; }

		public virtual int id { get; set; }

		public virtual byte[] data { get; set; }

		public virtual bool shouldDraw { get; set; }

		public int textureSize = 8;
		public byte state = 15;
		//private FaceDirection[] sideDirs = new FaceDirection[9];
		//private int posX, posY;
		public virtual int PosX { get { return TileX * 16; } }

		public virtual int PosY { get { return TileY * 16; } }

		private World.World parentWorld;
		public Blocks.BlockMaterial material = Blocks.BlockMaterial.NONE;
		private int hardness, damage, crack;
		public List<BlockOverlay> overlays = new List<BlockOverlay>();
		private BlockOverlayBreakAnim crackAnimation;
		public int darkness = 255;
		public bool hasChecked = false;
		public bool isWall;

		public Block(int id, bool wall)
		{
			this.id = id;
			this.damage = 0;
			this.crack = 0;
			this.isWall = wall;
			if (wall)
				darkness = 134;
			crackAnimation = new BlockOverlayBreakAnim(this, Color.White);
			this.target = TileFace.Mappings[15]; //this should not be here, eventually
		}

		public virtual void AssignWorld(World.World world)
		{
			this.parentWorld = world;
			BlockPosition = GetIndex().add(chunkIn.GetPos().ToBlockPos());
		}

		public virtual void InitOverlays()
		{

		}

		public virtual void InitWorld(World.World world)
		{
			AssignWorld(world);
			//UpdateHitbox();
			this.BlockSideUpdate(world);
			this.InitOverlays();
			this.InitShadowing(new BlockPos(this.BlockPosition.X, this.BlockPosition.Y), world);
			this.DoShadowing(new BlockPos(this.BlockPosition.X, this.BlockPosition.Y), world);
			this.CleanupShadowing(new BlockPos(this.BlockPosition.X, this.BlockPosition.Y), world);
		}

		public void InitShadowing(BlockPos bpa, World.World w)
		{
			BlockPos bp = new BlockPos(bpa.X, bpa.Y + 1);
			Block b = w.GetBlock(bp, 0);
			if (b != null)
			{
				this.darkness = 255;
				b.InitShadowing(bp, w);
			}
		}

		public void DoShadowing(BlockPos bpa, World.World w)
		{
			//This needs to be a functional thing later...
			/*foreach (BlockPos bp in BlockPos.GetAdjacentSides(this.BlockPosition))
			{
				Block b = w.GetBlock(bp, 0);
				if (b != null)
				{
					if (!b.hasChecked)
					{
						this.darkness = b.darkness - 31;
						this.hasChecked = true;
						b.InitShadowing(bp, w);
					}
				}
			}*/
			

			BlockPos bp = new BlockPos(bpa.X, bpa.Y + 1);
			Block b = w.GetBlock(bp, 0);
			BlockPos bpb = new BlockPos(bpa.X, bpa.Y - 1);
			Block bb = w.GetBlock(bpb, 0);
			if (b != null && !b.hasChecked)
			{
				this.hasChecked = true;
				if (bb != null)
					this.darkness = bb.darkness - 31;
				b.DoShadowing(bp, w);
			}
		}

		public void CleanupShadowing(BlockPos bpa, World.World w)
		{
			BlockPos bp = new BlockPos(bpa.X, bpa.Y + 1);
			Block b = w.GetBlock(bp, 0);
			if (b != null)
			{
				this.hasChecked = false;
				b.CleanupShadowing(bp, w);
			}
		}

		public abstract void Update(GameTime gameTime);

		public virtual void Draw(GameTime gameTime)
		{
			//Game1.thisGame.spriteBatch.End();
			//Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Runtime.Runtime.thisRuntime.mCamera.Transform);
			if (!shouldDraw)
				return;
			if (!isWall)
				Game1.thisGame.spriteBatch.Draw(texture, hitbox, new Rectangle(target.X * textureSize, target.Y * textureSize, textureSize, textureSize), new Color(darkness, darkness, darkness, 255));
			else
				Game1.thisGame.spriteBatch.Draw(texture, hitbox, new Rectangle((target.X * textureSize), (target.Y * textureSize), textureSize, textureSize), new Color(darkness, darkness, darkness, 255));
			foreach (BlockOverlay o in this.overlays)
			{
				o.Draw(gameTime, PosX, PosY, target);
			}
			crackAnimation.Draw(gameTime, PosX, PosY, crack);

		}

		public Item.Item GetThisAsItem()
		{
			return new Item.Items.ItemBlock(this);
		}

		public virtual void Destroy(World.World world, byte wallflag = 0)
		{
			if (chunkIn != null && BlockPosition != null)
			{
				Block u = world.GetBlock(new BlockPos(this.BlockPosition.X, this.BlockPosition.Y + 1));
				if (u != null)
					u.InitWorld(world);
				world.SpawnEntity(new Entity.Entities.EntityItem(new ItemStack(GetThisAsItem(), 1)), new Vector2(PosX, PosY));
				world.SetBlock(BlockPosition, null, wallflag);
				foreach (BlockOverlay o in overlays)
					o.OnParentDestroyed();
				overlays.Clear();
			}
		}

		public World.World GetWorld()
		{
			return this.parentWorld;
		}

		public virtual void MineBlock(Entity.Entities.EntityPlayer player, World.World world)
		{
			damage += (int)(player.GetMiningPower(this.material) * player.GetMiningStrength() + 1);
			UpdateMiningProgress();
			if (damage >= hardness)
				this.Destroy(world, 0);
		}

		public virtual void MineWall(Entity.Entities.EntityPlayer player, World.World world)
		{
			damage += (int)(player.GetMiningPower(this.material) * player.GetMiningStrength() + 1);
			UpdateMiningProgress();
			if (damage >= hardness)
				this.Destroy(world, 1);
		}

		public virtual void UpdateMiningProgress()
		{
			this.crack = (int)Math.Floor((float)damage / (float)hardness * 5f);
		}

		public virtual void DrawUpdate(GameTime gameTime, Rectangle screen)
		{
			shouldDraw = this.hitbox.Intersects(screen);
		}

		public virtual void BlockUpdate(GameTime gameTime, World.World worldIn)
		{

		}

		public virtual void BlockSideUpdate(World.World worldIn)
		{
			Block b;
			byte state = 0;
			byte flag = (byte)(isWall ? 1 : 0);
			foreach (BlockPos p in BlockPos.GetAdjacentSides(this.BlockPosition))
			{
				b = worldIn.GetBlock(p, flag);
				state <<= 1;
				if (b != null)
				{
					state += 1;
				}
			}
			this.state = state;
			this.SetTextureFace(state);
		}

		public virtual void SetTextureName(string name)
		{
			texture = Game1.thisGame.Content.Load<Texture2D>("Tiles/" + name);
		}

		public virtual void SetTextureFace(byte state)
		{
			if (state <= 15)
			{
				this.target = TileFace.Mappings[state];
				return;
			}
		}

		public virtual void SetMaterial(Blocks.BlockMaterial material)
		{
			this.material = material;
			this.hardness = Blocks.BlockMaterials.GetBaseHardness(material);
		}

		public virtual void SetHardness(int hardness)
		{
			this.hardness = hardness;
		}

		public int GetID()
		{
			return this.id;
		}

		public override void readFromTag(TagCompound data)
		{
			loadBlock(data);
		}

		public async void loadBlock(TagCompound data)
		{
			this.state = (byte)(data.GetTag("Join").GetValue());
			this.darkness = (int)(data.GetTag("LightVal").GetValue());
			this.SetTextureFace(this.state);
		}

		public BlockPos GetIndex()
		{
			if (isWall)
			{
				return chunkIn.GetWallIndex(this);
			}
			return chunkIn.GetBlockIndex(this);
		}

		public override void writeToTag(TagCompound data)
		{
			saveBlock(data);
		}

		public async void saveBlock(TagCompound data)
		{
			BlockPos inChunkPos = GetIndex();
			data.AddTag("PosX", new TagInt(inChunkPos.X));
			data.AddTag("PosY", new TagInt(inChunkPos.Y));
			data.AddTag("ID", new TagInt(this.id));
			data.AddTag("LightVal", new TagInt(this.darkness));
			data.AddTag("IsWall", new TagBool(this.isWall));
			data.AddTag("Join", new TagByte((byte)this.state));
		}
	}
}
