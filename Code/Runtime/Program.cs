using System;

namespace Earthian
{
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		private enum GameMode
		{
			ModEdit,
			BaseGame,
			GlitchMode
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			GameMode mode = GameMode.BaseGame;
			foreach (string arg in args)
			{
				if (arg == "-editm")
					mode = GameMode.ModEdit;

				if (arg == "-fuckoff")
					throw new Exception("Fuck off exception called, game crashed to test crash handler.");

				if (arg == "--forceglitchmodeifdevbuild")
					mode = GameMode.GlitchMode;

			}

			try
			{
				if (mode == GameMode.BaseGame)
				{
					//ModCodeWindow win = new ModCodeWindow();
					//System.Windows.Forms.MessageBox.Show("Notice: This is a development build and may *NOT* be distributed. Press OK to start the game. :)", "EarthainEngine Notification", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
					using (var game = new Game1())
						game.Run();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadLine();
			}
		}
	}
}
