using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Earthian.GameObject.Settings;
using Earthian.Utilities;

namespace Earthian.GameObject.Entity.AI
{
	public class EntityAIPlayer : EntityAI
	{
		//the AI for the player you control.

		public override void Handle(Entity actor, Entity target)
		{
			KeyboardState keyboard = Keyboard.GetState(); //this is bad
			Entities.EntityPlayer p = (Entities.EntityPlayer)actor;

			if (Game1.thisGame.IsActive)
			{
				if (Input.IsControlDown(ControlBinds.Controls.Move_Right))
				{
					actor.doFlip = true;
					actor.MotionX += p.Speed;
				}
				if (Input.IsControlDown(ControlBinds.Controls.Move_Left))
				{
					actor.doFlip = false;
					actor.MotionX -= p.Speed;
				}
				if (Input.IsControlDown(ControlBinds.Controls.Jump))
				{
					p.IsJumping = true;
				}
				else
				{
					p.IsJumping = false;
				}
				if (keyboard.IsKeyDown(Keys.R))
				{
					p.Respawn();
				}

				// Switch hotbar slot.
				if (keyboard.IsKeyDown(Keys.D0))
					p.heldItem = 9;
				else if (keyboard.IsKeyDown(Keys.D1))
					p.heldItem = 0;
				else if (keyboard.IsKeyDown(Keys.D2))
					p.heldItem = 1;
				else if (keyboard.IsKeyDown(Keys.D3))
					p.heldItem = 2;
				else if (keyboard.IsKeyDown(Keys.D4))
					p.heldItem = 3;
				else if (keyboard.IsKeyDown(Keys.D5))
					p.heldItem = 4;
				else if (keyboard.IsKeyDown(Keys.D6))
					p.heldItem = 5;
				else if (keyboard.IsKeyDown(Keys.D7))
					p.heldItem = 6;
				else if (keyboard.IsKeyDown(Keys.D8))
					p.heldItem = 7;
				else if (keyboard.IsKeyDown(Keys.D9))
					p.heldItem = 8;
                // Open Inventory.
                else if (Input.IsControlPressedOnce(ControlBinds.Controls.Inventory))
				{
					if (p.isInventoryOpen)
						p.isInventoryOpen = false;
					else
						p.isInventoryOpen = true;
				}
				if (Utilities.Input.GetMouseClick(Utilities.Input.MouseButtons.LEFT) && !p.inInterface)
				{
					p.Swing(false);
				}
				if (Utilities.Input.GetMouseClick(Utilities.Input.MouseButtons.RIGHT) && !p.inInterface)
				{
					p.Swing(true);
				}
			}
		}
	}
}