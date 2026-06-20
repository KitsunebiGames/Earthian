using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Earthian.GameObject.World;
using Earthian.Runtime.Screens;
using Earthian.GameObject.Container;
using Earthian.GameObject.Item;
using Earthian.GameObject.Settings;
using Earthian.Utilities;
using Earthian.GameObject.Entity.AI;

namespace Earthian.GameObject.Entity.Entities
{
	public class EntityPlayer : Entity
	{
		public static int id = 0;
		string playerName;
		string look;
		public bool isInventoryOpen = false;
		public Vector2 respawnPoint = new Vector2(0, 0);
		public Container.Container Inventory;
		public Texture2D texTest;
		public float pickupRange;
		public int heldItem;
		public int swingTime;
		public bool inInterface = false;
		public Rectangle swingingHitbox;
		public bool allowBlockPlace = true;

		#region Constructor

		public EntityPlayer(string playerName, string lookCode, World.World parent)
			: base(EntityPlayer.id)
		{
			this.World = parent;
			this.playerName = playerName;
			this.Speed = 1f;
			this.look = lookCode;
			this.TextureTarget = new Rectangle(-16, 0, 48, 64);
			width = 16;
			height = 60;
			this.Health = 100;
			this.MaxHealth = 100; //change me!
			this.Inventory = new Container.Container(45);
			this.MotionX = 0;
			this.MotionY = 0;
			this.pickupRange = 200f;
			this.heldItem = 0;
			this.swingTime = 0;
			this.PhysicsData = new EntityPhysicsData();
			this.PhysicsData.CanNoClip = false;
			this.PhysicsData.GravityOn = true;
			this.UpdatePhysicsData();
			this.ai.Add(new AI.EntityAIPlayer());
			//this.ai.Add(new AI.EntityAIGravity());
			this.ai.Add(new AI.EntityAIDeceleration(0.75f, 0.0f, 0.95f, 0.1f, true));
		}

		public override void Init()
		{
			anim = new Utilities.EntityAnimationHandler(3);
			anim.addAnim(0, Game1.thisGame.Content.Load<Texture2D>("Entities/Player/TestPlayerIdle"), 3, new Utilities.EntityTexturePosition(48, 64), 200);
			anim.addAnim(1, Game1.thisGame.Content.Load<Texture2D>("Entities/Player/TestPlayerRun"), 5, new Utilities.EntityTexturePosition(48, 64), 100);
			anim.addAnim(2, Game1.thisGame.Content.Load<Texture2D>("Entities/Player/TestPlayerRun"), 0, new Utilities.EntityTexturePosition(48, 64), 1000);
			anim.changeState(0, false);
			texTest = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/DAYGrad");
			this.Inventory.AddItemStack(new ItemStack(ItemLookup.items.ConstructItem(5), 1));
			this.Inventory.AddItemStack(new ItemStack(ItemLookup.items.ConstructItemBlock(5), 256));
		}

		#endregion


		#region XNA Runtime

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			UpdatePhysicsData();
			HandlePhysics(gameTime);
			// \/ \/ \/ This stuff needs a proper object managing the logic or something similar.
			/*Effect blegh = Utilities.Drawing.GetShader("Damage");
			if (blegh.Parameters["damagetime"].GetValueSingle() > 0.01f)
				blegh.Parameters["damagetime"].SetValue(blegh.Parameters["damagetime"].GetValueSingle() - 0.025f);
			if (blegh.Parameters["damagetime"].GetValueSingle() > 1f)
				blegh.Parameters["damagetime"].SetValue(1f);
			if (hurtTime >= 0)
			{
				blegh.Parameters["damagetime"].SetValue(blegh.Parameters["damagetime"].GetValueSingle() + 0.1f + ((hurtTime) / 200));
				hurtTime -= 1;
			}*/
			Runtime.Runtime.thisRuntime.SetCameraPos(new Vector2(this.Position.X + this.TextureTarget.Width / 2, this.Position.Y + this.TextureTarget.Height / 2));
			HandleAnimation(gameTime);
			anim.Update(gameTime);
			swingTime -= 1;

			//If user has mouse in open inventory GUI, will be extended later to work across all GUI's
			if (!IngameScreen.thisScreen.guis.IsInGUI())
			{
				allowBlockPlace = true;
			}
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
			//Utilities.Drawing.ApplyShader("Damage");
			if (swingTime > 0)
			{
				ItemStack i = GetHeldItem();
				if (i != null)
					this.swingingHitbox = i.item.DrawSwingAnimation(gameTime, swingTime, Centre(), doFlip, this);
			}
			//Game1.thisGame.spriteBatch.Draw(texTest, this.Hitbox, Color.Blue);

			anim.Draw(gameTime, new Rectangle((int)posX + TextureTarget.X, (int)posY + TextureTarget.Y, TextureTarget.Width, TextureTarget.Height), doFlip);

		}

		#endregion

		/*
        public new void HandlePhysics(GameTime gameTime)
        {
            HandleAI();
            Physics.HandleEntityPhysics(gameTime, this);
        }*/

		public void HandleAnimation(GameTime gameTime)
		{
			if (this.MotionY > 4.6f || IsJumping)
			{
				anim.changeState(2, false);
			}
			else if (Math.Abs(MotionX) > Speed)
			{
				anim.changeState(1, false);
			}
			else
			{
				anim.changeState(0, false);
			}
		}

		public void Swing(bool right)
		{
			if (this.swingTime <= 0)
			{
				ItemStack item = GetHeldItem();
				if (item != null)
					this.swingTime = item.item.swingTime;
				else
					return;
				if (!right)
					item.item.Use(this, this.World, item, false);
				else
					item.item.AltUse(this, this.World, item, false);
			}
		}

		public void MineBlock(BlockPos pos, byte wallflag, World.World world)
		{
			Block.Block b = world.GetBlock(pos, wallflag);
			if (b != null)
			{
				if (wallflag == 0)
					b.MineBlock(this, world);
				else
					b.MineWall(this, world);
			}
		}

		public bool PlaceBlock(Block.Block block, BlockPos pos, byte wallFlag = 0)
		{
			var ablock = this.World.GetBlock(pos, wallFlag);
			if (wallFlag == 1 && ablock != null)
			{
				if (ablock.id != block.id) this.World.GetBlock(pos, wallFlag).Destroy(World, 1);
			}
			if (ablock == null)
			{
				if (!new Rectangle(pos.ToBlockPos().X, pos.ToBlockPos().Y, 16, 16).Intersects(Hitbox))
				{
					this.World.SetBlock(pos, block, wallFlag);
					return true;
				}
				return false;
			}
			else
			{
				return false;
			}
		}

		public override void SpawnEntity(Entity entity, World.World world, Vector2 position)
		{
			entity.World = world;
			entity.IsDead = false;
			entity.Position = position;
		}

		public override void UseBlock(Block.Block block, int x, int y, int z, World.World world)
		{

		}

		public float GetMiningPower(Block.Blocks.BlockMaterial material)
		{
			ItemStack i = GetHeldItem();
			if (i != null)
				return i.item.GetToolPowerVsMaterial(material);
			else
				return 1f;
		}

		public int GetMiningStrength()
		{
			ItemStack i = GetHeldItem();
			if (i != null)
				return i.item.toolPower;
			else
				return 0;
		}

		public ItemStack GetHeldItem()
		{
			return this.Inventory.GetItem(this.heldItem); //held item on mouse in inventory will be an option later
		}

		public void Respawn()
		{
			Respawn(respawnPoint);
		}
	}
}
