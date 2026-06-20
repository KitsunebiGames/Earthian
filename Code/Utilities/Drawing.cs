using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.Utilities
{
	public class Drawing
	{
		private static Dictionary<string,Effect> shaders = new Dictionary<string,Effect>();
		public static bool Active = false;
		public static Texture2D Debug;

		public static Effect GetShader(string id)
		{
			return shaders[id];
		}

		public static void RegisterShader(string id, string path)
		{
			shaders.Add(id, Game1.thisGame.Content.Load<Effect>(path));
		}

		public static void NewBatch(bool camera)
		{
			Active = true;
			if (camera)
			{
				Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Runtime.Runtime.thisRuntime.mCamera.Transform);
			}
			else
			{
				Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
			}
		}

		public static void EndBatch()
		{
			Active = false;
			Game1.thisGame.spriteBatch.End();
		}

		public static void Init()
		{
			//RegisterShader("Damage", "Shaders/PlayerEffects/Damage");
			Debug = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/DAYGrad");
		}

		public static void ApplyShader(string id)
		{
			foreach (EffectPass e in GetShader(id).CurrentTechnique.Passes)
			{
				e.Apply();
			}
		}
	}
}
