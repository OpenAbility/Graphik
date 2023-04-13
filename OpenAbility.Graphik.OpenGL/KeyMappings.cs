using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenAbility.Graphik.OpenGL;

internal static class KeyMappings
{

	#region KeyMapDefinition
	private static readonly Dictionary<Keys, Key> Map = new Dictionary<Keys, Key>()
	{
		{
			Keys.Space, Key.Space
		},
		{
			Keys.Apostrophe, Key.Apostrophe
		},
		{
			Keys.Comma, Key.Comma
		},
		{
			Keys.Minus, Key.Minus
		},
		{
			Keys.Period, Key.Period
		},
		{
			Keys.Slash, Key.Slash
		},
		{
			Keys.D0, Key.D0
		},
		{
			Keys.D1, Key.D1
		},
		{
			Keys.D2, Key.D2
		},
		{
			Keys.D3, Key.D3
		},
		{
			Keys.D4, Key.D4
		},
		{
			Keys.D5, Key.D5
		},
		{
			Keys.D6, Key.D6
		},
		{
			Keys.D7, Key.D7
		},
		{
			Keys.D8, Key.D8
		},
		{
			Keys.D9, Key.D9
		},
		{
			Keys.Semicolon, Key.Semicolon
		},
		{
			Keys.Equal, Key.Equal
		},
		{
			Keys.A, Key.A
		},
		{
			Keys.B, Key.B
		},
		{
			Keys.C, Key.C
		},
		{
			Keys.D, Key.D
		},
		{
			Keys.E, Key.E
		},
		{
			Keys.F, Key.F
		},
		{
			Keys.G, Key.G
		},
		{
			Keys.H, Key.H
		},
		{
			Keys.I, Key.I
		},
		{
			Keys.J, Key.J
		},
		{
			Keys.K, Key.K
		},
		{
			Keys.L, Key.L
		},
		{
			Keys.M, Key.M
		},
		{
			Keys.N, Key.N
		},
		{
			Keys.O, Key.O
		},
		{
			Keys.P, Key.P
		},
		{
			Keys.Q, Key.Q
		},
		{
			Keys.R, Key.R
		},
		{
			Keys.S, Key.S
		},
		{
			Keys.T, Key.T
		},
		{
			Keys.U, Key.U
		},
		{
			Keys.V, Key.V
		},
		{
			Keys.W, Key.W
		},
		{
			Keys.X, Key.X
		},
		{
			Keys.Y, Key.Y
		},
		{
			Keys.Z, Key.Z
		},
		{
			Keys.LeftBracket, Key.LeftBracket
		},
		{
			Keys.Backslash, Key.Backslash
		},
		{
			Keys.RightBracket, Key.RightBracket
		},
		{
			Keys.GraveAccent, Key.GraveAccent
		},
		{
			Keys.Escape, Key.Escape
		},
		{
			Keys.Enter, Key.Enter
		},
		{
			Keys.Tab, Key.Tab
		},
		{
			Keys.Backspace, Key.Backspace
		},
		{
			Keys.Insert, Key.Insert
		},
		{
			Keys.Delete, Key.Delete
		},
		{
			Keys.Right, Key.Right
		},
		{
			Keys.Left, Key.Left
		},
		{
			Keys.Down, Key.Down
		},
		{
			Keys.Up, Key.Up
		},
		{
			Keys.PageUp, Key.PageUp
		},
		{
			Keys.PageDown, Key.PageDown
		},
		{
			Keys.Home, Key.Home
		},
		{
			Keys.End, Key.End
		},
		{
			Keys.CapsLock, Key.CapsLock
		},
		{
			Keys.ScrollLock, Key.ScrollLock
		},
		{
			Keys.NumLock, Key.NumLock
		},
		{
			Keys.PrintScreen, Key.PrintScreen
		},
		{
			Keys.Pause, Key.Pause
		},
		{
			Keys.F1, Key.F1
		},
		{
			Keys.F2, Key.F2
		},
		{
			Keys.F3, Key.F3
		},
		{
			Keys.F4, Key.F4
		},
		{
			Keys.F5, Key.F5
		},
		{
			Keys.F6, Key.F6
		},
		{
			Keys.F7, Key.F7
		},
		{
			Keys.F8, Key.F8
		},
		{
			Keys.F9, Key.F9
		},
		{
			Keys.F10, Key.F10
		},
		{
			Keys.F11, Key.F11
		},
		{
			Keys.F12, Key.F12
		},
		{
			Keys.F13, Key.F13
		},
		{
			Keys.F14, Key.F14
		},
		{
			Keys.F15, Key.F15
		},
		{
			Keys.F16, Key.F16
		},
		{
			Keys.F17, Key.F17
		},
		{
			Keys.F18, Key.F18
		},
		{
			Keys.F19, Key.F19
		},
		{
			Keys.F20, Key.F20
		},
		{
			Keys.F21, Key.F21
		},
		{
			Keys.F22, Key.F22
		},
		{
			Keys.F23, Key.F23
		},
		{
			Keys.F24, Key.F24
		},
		{
			Keys.F25, Key.F25
		},
		{
			Keys.KeyPad0, Key.KeyPad0
		},
		{
			Keys.KeyPad1, Key.KeyPad1
		},
		{
			Keys.KeyPad2, Key.KeyPad2
		},
		{
			Keys.KeyPad3, Key.KeyPad3
		},
		{
			Keys.KeyPad4, Key.KeyPad4
		},
		{
			Keys.KeyPad5, Key.KeyPad5
		},
		{
			Keys.KeyPad6, Key.KeyPad6
		},
		{
			Keys.KeyPad7, Key.KeyPad7
		},
		{
			Keys.KeyPad8, Key.KeyPad8
		},
		{
			Keys.KeyPad9, Key.KeyPad9
		},
		{
			Keys.KeyPadDecimal, Key.KeyPadDecimal
		},
		{
			Keys.KeyPadDivide, Key.KeyPadDivide
		},
		{
			Keys.KeyPadMultiply, Key.KeyPadMultiply
		},
		{
			Keys.KeyPadSubtract, Key.KeyPadSubtract
		},
		{
			Keys.KeyPadAdd, Key.KeyPadAdd
		},
		{
			Keys.KeyPadEnter, Key.KeyPadEnter
		},
		{
			Keys.KeyPadEqual, Key.KeyPadEqual
		},
		{
			Keys.LeftShift, Key.LeftShift
		},
		{
			Keys.LeftControl, Key.LeftControl
		},
		{
			Keys.LeftAlt, Key.LeftAlt
		},
		{
			Keys.LeftSuper, Key.LeftSuper
		},
		{
			Keys.RightShift, Key.RightShift
		},
		{
			Keys.RightControl, Key.RightControl
		},
		{
			Keys.RightAlt, Key.RightAlt
		},
		{
			Keys.RightSuper, Key.RightSuper
		},
		{
			Keys.Menu, Key.Menu
		}
	};
	#endregion

	public static Key GetKey(Keys key)
	{
		if (Map.TryGetValue(key, out Key mappedValue))
			return mappedValue;
		return Key.Invalid;
	}
}
