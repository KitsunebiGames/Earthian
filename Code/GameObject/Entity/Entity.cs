using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Earthian.GameObject.World;
using System.Threading;
using Earthian.GameObject.Entity.AI;


namespace Earthian.GameObject.Entity
{
	public abstract class Entity
	{
		#region Variables

		public int width, height;
		public float posX, posY;

		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual bool IsGrounded { get; set; }

		public virtual int Health { get; set; }

		public virtual int MaxHealth { get; set; }

		public virtual int Damage { get; set; }

		public virtual bool IsDead { get; set; }

		public virtual World.World World { get; set; }

		public virtual Entity Target { get; set; }

		public virtual EntityTextureAnimation TexturePos { get; set; }

		public Rectangle TextureTarget { get; set; }

		public virtual Texture2D[] Texture { get; set; }

		public virtual float MotionX { get; set; }

		public virtual float MotionY { get; set; }

		public virtual bool IsFalling { get; set; }

		public virtual bool IsJumping { get; set; }

		public virtual float Speed { get; set; }

		protected Container.LootTable lootTable;

		//to use later?
		protected List<EntityAI> ai = new List<EntityAI>();
		public EntityPhysicsData PhysicsData;
		public bool Noclip = false;
		protected Utilities.EntityAnimationHandler anim;
		protected int hurtTime = -1;
		public bool doFlip;

		public virtual Rectangle Hitbox
		{
			get
			{
				return new Rectangle((int)posX, (int)posY, width, height);
			}
		}

		public virtual Vector2 Position
		{
			get
			{
				return new Vector2(posX, posY);
			}
			set
			{
				posX = value.X;
				posY = value.Y;
			}
		}

		#endregion

		#region Constructors

		public Entity(int id)
		{
			this.Id = id;
			PhysicsData = new EntityPhysicsData();
			PhysicsData.CanNoClip = false;
			PhysicsData.Gravity = 0.0f;
			PhysicsData.GravityOn = false;
		}

		#endregion

		#region Functions

		public virtual void Update(GameTime gameTime)
		{
			UpdatePhysicsData();
			if (hurtTime >= 0)
			{
				hurtTime -= 1;
			}
			HandlePhysics(gameTime);
		}

		public abstract void Draw(GameTime gameTime);

		public abstract void Init();

		public virtual void Knockback(Entity source, Object damageType)
		{
			this.MotionX += MathHelper.Clamp((this.posX - source.posX) / 10f, -1f, 1f);
			this.MotionY += MathHelper.Clamp((this.posY - source.posY) / 10f, -1f, 1f);
		}

		public void UpdatePhysicsData()
		{
			if (PhysicsData != null)
				PhysicsData.Gravity = this.World.GetGravity();
		}

		public virtual void Hit(int baseDmg, Entity source, Object damageSource)
		{
			if (hurtTime <= 0)
			{
				hurtTime = 5; //affectable by stuff???
				this.Health -= baseDmg; //affectable by armour and stuff.
				//thorns armour, attack!
				if (source != null)
				{
					Knockback(source, damageSource);
				}
			}
		}

		public abstract void UseBlock(Block.Block block, int x, int y, int z, World.World world);

		public abstract void SpawnEntity(Entity entity, World.World world, Vector2 position);


		public void SetMaxHealth(int health)
		{
			MaxHealth = health;
		}

		public bool ShouldLoad()
		{
			ChunkPos chunkIn = World.GetPositionBlock(this.Position).ToChunkPos();
			return World.IsChunkLoaded(chunkIn);
		}

		public void Respawn(Vector2 pos)
		{
			this.IsDead = false;
			this.Health = this.MaxHealth;
			this.Position = pos;
		}

		public void Heal(int amount)
		{
			Health += amount;
			if (Health > MaxHealth)
				Health = MaxHealth;
		}

		public void HandlePhysics(GameTime gameTime)
		{
			HandleAI();
			Physics.HandleEntityPhysics(gameTime, this);
		}

		/* Deprecated for directional collision elsewhere
         
        public List<Vector2> GetCollisionPositions(GameTime gameTime)
        {
            int index = 0;
            List<Vector2> vectors = new List<Vector2>();
            foreach (BlockPos pos in World.GetPositionBlock(this.Position).GetAdjacentSides(4, 4))
            {
                Block.Block b = World.GetBlock(pos);
                bool isColliding = false;
                if (b != null)
                    isColliding = b.UpdateCollision(gameTime, this);
                if (isColliding)
                    vectors.Add(new Vector2((float)Math.Round(MathHelper.Clamp(this.Position.Y - pos.X, -1, 1), 1), (float)Math.Round(MathHelper.Clamp(this.Position.Y - pos.Y, -1, 1))));
            }
            return vectors;
        }

        public bool GetCollision(GameTime gameTime)
        {
            foreach (BlockPos pos in World.GetPositionBlock(this.Position).GetAdjacentSides(4, 4))
            {
                Block.Block b = World.GetBlock(pos);
                bool isColliding = false;
                if (b != null)
                    isColliding = b.UpdateCollision(gameTime, this);
                if (isColliding)
                    return true;
            }
            return false;
        }
         */

		public void HandleAI()
		{
			foreach (EntityAI ai in this.ai)
			{
				ai.Handle(this, this.Target);
			}
		}

		public bool CheckIfDead()
		{
			if (Health < 0)
				return true;
			else
				return false;
		}

		public Vector2 Centre()
		{
			return Hitbox.Center.ToVector2();
		}

		public void Attack(Entity entity)
		{
			entity.Hit(this.Damage, this, null);
		}

		#endregion


	}
}
