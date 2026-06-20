using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Earthian.GameObject.Settings;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;


namespace Earthian.Utilities
{
	public interface IKeyboardSubscriber
	{
		void RecieveTextInput(char inputChar);

		void RecieveTextInput(string text);

		void RecieveCommandInput(char command);

		void RecieveSpecialInput(Keys key);

		bool Selected { get; set; }
		//or Focused
	}

	public class KeyboardDispatcher
	{
		public KeyboardDispatcher(GameWindow window)
		{
			EventInput.Initialize(window);
			EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
			EventInput.KeyDown += new KeyEventHandler(EventInput_KeyDown);
		}

		void EventInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (_subscriber == null)
				return;

			_subscriber.RecieveSpecialInput(e.KeyCode);
		}

		void EventInput_CharEntered(object sender, CharacterEventArgs e)
		{
			if (_subscriber == null)
				return;
			if (char.IsControl(e.Character))
			{
				//ctrl-v
				if (e.Character == 0x16)
				{
					//XNA runs in Multiple Thread Apartment state, which cannot recieve clipboard
					Thread thread = new Thread(PasteThread);
					thread.SetApartmentState(ApartmentState.STA);
					thread.Start();
					thread.Join();
					_subscriber.RecieveTextInput(_pasteResult);
				}
				else
				{
					_subscriber.RecieveCommandInput(e.Character);
				}
			}
			else
			{
				_subscriber.RecieveTextInput(e.Character);
			}
		}

		IKeyboardSubscriber _subscriber;

		public IKeyboardSubscriber Subscriber
		{
			get { return _subscriber; }
			set
			{
				if (_subscriber != null)
					_subscriber.Selected = false;
				_subscriber = value;
				if (value != null)
					value.Selected = true;
			}
		}

		//Thread has to be in Single Thread Apartment state in order to receive clipboard
		string _pasteResult = "";

		[STAThread]
		void PasteThread()
		{
			_pasteResult = "";
		}
	}

	public class MLangKeyboard
	{
		const uint KLF_ACTIVATE = 1;
		//activate the layout
		const int KL_NAMELENGTH = 9;
		// length of the keyboard buffer
		const string LANG_EN_US = "00000409";
		const string LANG_HE_IL = "0001101A";

		[DllImport("user32.dll")]
		private static extern long LoadKeyboardLayout(
			string pwszKLID,  // input locale identifier
			uint Flags       // input locale identifier options
		);

		[DllImport("user32.dll")]
		private static extern long GetKeyboardLayoutName(
			System.Text.StringBuilder pwszKLID  //[out] string that receives the name of the locale identifier
		);

		public static string getName()
		{
			System.Text.StringBuilder name = new System.Text.StringBuilder(KL_NAMELENGTH);
			GetKeyboardLayoutName(name);
			return name.ToString();
		}
	}

	public class CharacterEventArgs : EventArgs
	{
		private readonly char character;
		private readonly int lParam;

		public CharacterEventArgs(char character, int lParam)
		{
			this.character = character;
			this.lParam = lParam;
		}

		public char Character
		{
			get { return character; }
		}

		public int Param
		{
			get { return lParam; }
		}

		public int RepeatCount
		{
			get { return lParam & 0xffff; }
		}

		public bool ExtendedKey
		{
			get { return (lParam & (1 << 24)) > 0; }
		}

		public bool AltPressed
		{
			get { return (lParam & (1 << 29)) > 0; }
		}

		public bool PreviousState
		{
			get { return (lParam & (1 << 30)) > 0; }
		}

		public bool TransitionState
		{
			get { return (lParam & (1 << 31)) > 0; }
		}
	}

	public class KeyEventArgs : EventArgs
	{
		private Keys keyCode;

		public KeyEventArgs(Keys keyCode)
		{
			this.keyCode = keyCode;
		}

		public Keys KeyCode
		{
			get { return keyCode; }
		}
	}

	public delegate void CharEnteredHandler(object sender,CharacterEventArgs e);
	public delegate void KeyEventHandler(object sender,KeyEventArgs e);

	public static class EventInput
	{
		/// <summary>
		/// Event raised when a character has been entered.
		/// </summary>
		public static event CharEnteredHandler CharEntered;

		/// <summary>
		/// Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.
		/// </summary>
		public static event KeyEventHandler KeyDown;

		/// <summary>
		/// Event raised when a key has been released.
		/// </summary>
		public static event KeyEventHandler KeyUp;

		delegate IntPtr WndProc(IntPtr hWnd,uint msg,IntPtr wParam,IntPtr lParam);

		static bool initialized;
		static IntPtr prevWndProc;
		static WndProc hookProcDelegate;
		static IntPtr hIMC;

		//various Win32 constants that we need
		const int GWL_WNDPROC = -4;
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;
		const int WM_CHAR = 0x102;
		const int WM_IME_SETCONTEXT = 0x0281;
		const int WM_INPUTLANGCHANGE = 0x51;
		const int WM_GETDLGCODE = 0x87;
		const int WM_IME_COMPOSITION = 0x10f;
		const int DLGC_WANTALLKEYS = 4;

		//Win32 functions that we're using
		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


		/// <summary>
		/// Initialize the TextInput with the given GameWindow.
		/// </summary>
		/// <param name="window">The XNA window to which text input should be linked.</param>
		public static void Initialize(GameWindow window)
		{
			/*if (initialized)
				throw new InvalidOperationException("TextInput.Initialize can only be called once!");

			hookProcDelegate = new WndProc(HookProc);
			prevWndProc = (IntPtr)SetWindowLong(window.Handle, GWL_WNDPROC,
				(int)Marshal.GetFunctionPointerForDelegate(hookProcDelegate));

			hIMC = ImmGetContext(window.Handle);*/
			initialized = true;
		}

		static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr returnCode = CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);

			switch (msg)
			{
				case WM_GETDLGCODE:
					returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
					break;

				case WM_KEYDOWN:
					if (KeyDown != null)
						KeyDown(null, new KeyEventArgs((Keys)wParam));
					break;

				case WM_KEYUP:
					if (KeyUp != null)
						KeyUp(null, new KeyEventArgs((Keys)wParam));
					break;

				case WM_CHAR:
					if (CharEntered != null)
						CharEntered(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
					break;

				case WM_IME_SETCONTEXT:
					if (wParam.ToInt32() == 1)
						ImmAssociateContext(hWnd, hIMC);
					break;

				case WM_INPUTLANGCHANGE:
					ImmAssociateContext(hWnd, hIMC);
					returnCode = (IntPtr)1;
					break;
			}

			return returnCode;
		}
	}

	class Input
	{
		#region Variables

		public enum MouseButtons
		{
			LEFT = 0,
			RIGHT = 1,
			MIDDLE = 2

		}

		public static bool[] oldKeyPress = new bool[255];
		public static bool[] oldMouseClick = new bool[] { false, false, false };
		public static int oldScroll = 0;

		#endregion

		#region Mouse

		public static bool GetMouseClick(MouseButtons button)
		{
			MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
			if (button == MouseButtons.LEFT)
				return (mouse.LeftButton == ButtonState.Pressed) ? true : false;
			if (button == MouseButtons.RIGHT)
				return (mouse.RightButton == ButtonState.Pressed) ? true : false;
			if (button == MouseButtons.MIDDLE)
				return (mouse.MiddleButton == ButtonState.Pressed) ? true : false;
			return false;
		}

		public static Vector2 GetMousePos()
		{
			MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
			return mouse.Position.ToVector2();
		}

		public static bool GetMouseOver(Rectangle hitbox)
		{
			Vector2 pos = GetMousePos();
			return (pos.X > hitbox.X && pos.X < hitbox.X + hitbox.Width && pos.Y > hitbox.Y && pos.Y < hitbox.Y + hitbox.Height);
		}

		public static bool MouseClickOnce(MouseButtons button)
		{
			bool result = (GetMouseClick(button) && !(oldMouseClick[(int)button]));
			oldMouseClick[(int)button] = GetMouseClick(button);
			return result;
		}

		public static int GetMouseScroll()
		{
			int scroll = Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue;
			int result = scroll - oldScroll;
			oldScroll = scroll;
			return result;
		}

		#endregion

		#region Keyboard

		public static EventHandler<TextInputEventArgs> onKeyboardEnterText = null;
		KeyboardState _prevState;

		public static void InitKeyboard()
		{
			Game1.thisGame.Window.TextInput += Game1_thisGame_Window_TextInput;
		}

		private static void Game1_thisGame_Window_TextInput(object sender, TextInputEventArgs e)
		{
			if (onKeyboardEnterText != null)
				onKeyboardEnterText.Invoke(sender, e);
		}

		public static bool IsKeyPressed(Keys key)
		{
			KeyboardState keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
			if (keyboard.IsKeyDown(key))
			{
				oldKeyPress[(int)key] = true;
				return true;
			}
			oldKeyPress[(int)key] = false;
			return false;
		}

		public static bool IsControlDown(ControlBinds.Controls control)
		{
			return IsKeyPressed(ControlBinds.GetBind(control));
		}

		public static bool IsControlPressedOnce(ControlBinds.Controls control)
		{
			return KeyPressedOnce(ControlBinds.GetBind(control));
		}

		public static bool KeyPressedOnce(Keys key)
		{
			KeyboardState keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
			if (keyboard.IsKeyDown(key) && oldKeyPress[(int)key] == false)
			{
				oldKeyPress[(int)key] = true;
				return true;
			}
			if (!keyboard.IsKeyDown(key))
				oldKeyPress[(int)key] = false;
			return false;
		}

		public static Keys[] GetPressedKeys()
		{
			return Microsoft.Xna.Framework.Input.Keyboard.GetState().GetPressedKeys();
		}

		#endregion

	}
}
