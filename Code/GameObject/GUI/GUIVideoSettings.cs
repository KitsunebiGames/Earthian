using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.GUI.Elements;

namespace Earthian.GameObject.GUI
{
	public class GUIVideoSettings : GUIScrollPanel
	{
		public List<Tuple<string,GUITextLabel,GUISlider>> sliders = new List<Tuple<string,GUITextLabel,GUISlider>>();

		public GUIVideoSettings(int x, int y)
			: base(400, 300)
		{
			AddSlider("Zoom");
		}

		private void AddSlider(string name)
		{
			GUITextLabel t = new GUITextLabel(name, 200, 32);
			this.Add(t);
			GUISlider s = (GUISlider)(new GUISlider(0.5f, 300, 32).SetPosition(205, 5));
			this.Add(s);
			sliders.Add(new Tuple<string, GUITextLabel, GUISlider>(name, t, s));
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (Tuple<string, GUITextLabel, GUISlider> t in sliders)
			{
				t.Item3.Update(gameTime);
			}

			//zoom
			float zv = 0.5f + sliders[0].Item3.GetSliderPosition() * 2.5f;
			sliders[0].Item2.SetText(String.Format("Zoom: {0}x", zv));
			Runtime.Runtime.thisRuntime.mCamera.Zoom = zv;
		}
	}
}
