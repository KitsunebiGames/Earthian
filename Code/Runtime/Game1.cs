using Earthian.Code.Runtime.Modding;
using Earthian.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Earthian
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public ContentManager content;
		public Runtime.Runtime runtime;
		public Earthian.Mouse mouseCursor;
		public KeyboardDispatcher disp;
		Texture2D cursor;
		MouseState mouse;
		List<Camera> cameras = new List<Camera>();
		

		public static Game1 thisGame;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			runtime = new Runtime.Runtime();
			thisGame = this;
			Earthian.GameObject.Item.ItemLookup iL = new Earthian.GameObject.Item.ItemLookup();
			disp = new KeyboardDispatcher(this.Window);
			//Mod m = new Mod("AwesomeMod.mod");
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			Window.AllowUserResizing = true;
			this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);           
			mouseCursor = new Mouse("Cursor\\Cursor");
			runtime.Init();
			Input.InitKeyboard();
			base.Initialize();
		}

		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			//graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			//graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
			//graphics.ApplyChanges();
			foreach (Camera c in cameras)
				c.ChangeViewport(Game1.thisGame.GraphicsDevice.Viewport);
		}

		public void AddCamera(Camera cam)
		{
			cameras.Add(cam);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			// TODO: use this.Content to load your game content here
            
			cursor = this.Content.Load<Texture2D>("Cursor/Cursor");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}


		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

			runtime.Update(gameTime);
			mouseCursor.Update(gameTime, mouse);

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyDown(Keys.Home))
				Exit();


			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(new Color(0, 0, 0));
			runtime.Draw(gameTime);
            

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
