using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenAbility.Graphik.OpenGL;

internal static unsafe class CallbackHandler
{
	private static GLAPI glapi = null!;
	
	public static ErrorCallback? ErrorCallback;
	public static DebugCallback? DebugCallback;
	public static KeyCallback? KeyCallback;
	public static ResizeCallback? ResizeCallback;
	public static TypeCallback? TypeCallback;
	public static MouseCallback? MouseCallback;
	public static CursorCallback? CursorCallback;
	public static ScrollCallback? ScrollCallback;
	
	private static readonly GLDebugProc DebugProc = (source, type, _, severity, messageLength, messagePointer, _) =>
	{

		byte[] messageReadBuffer = ArrayPool<byte>.Shared.Rent(messageLength);
		Marshal.Copy(messagePointer, messageReadBuffer, 0, messageLength);

		string message = Encoding.Default.GetString(messageReadBuffer);

		ArrayPool<byte>.Shared.Return(messageReadBuffer);
		
		if (type == DebugType.DebugTypeError)
		{
			if (ErrorCallback != null)
			{
				ErrorCallback($"[GL ERROR, T: {type}, S: {severity}]", message);
				return;
			}
		}

		if(DebugCallback != null)
			DebugCallback($"[GL MESSAGE, T: {type}, S: {severity}]", message);
		
	};

	private static readonly GLFWCallbacks.ErrorCallback GLFWErrorCallback = (error, description) =>
	{
		if (ErrorCallback == null)
			return;

		ErrorCallback("GLFW_" + error + "", description);
	};
	
	private static readonly GLFWCallbacks.WindowSizeCallback GLFWWindowSizeCallback = (_, width, height) =>
	{
		glapi.Width = width;
		glapi.Height = height;
		GL.Viewport(0, 0, width, height);
		
		if(ResizeCallback == null)
			return;
		ResizeCallback(width, height);
	};

	private static readonly GLFWCallbacks.KeyCallback GLFWKeyCallback = (_, key, _, action, _) =>
	{
		if(KeyCallback == null)
			return;

		KeyCallback(KeyMappings.GetKey(key), GetInputAction(action));
	};

	private static readonly GLFWCallbacks.CharCallback GLFWCharCallback = (_, character) =>
	{
		if (TypeCallback == null)
			return;
		TypeCallback(character);
	};
	
	private static readonly GLFWCallbacks.MouseButtonCallback GLFWMouseCallback = (_, button, action, _) =>
	{
		if (MouseCallback == null)
			return;

		MouseButton mouseButton = button switch
		{
			OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button1 => MouseButton.Left,
			OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button2 => MouseButton.Right,
			OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Button3 => MouseButton.Wheel,
			_ => MouseButton.Unknown
		};
		
		MouseCallback(mouseButton, GetInputAction(action));
	};
	
	private static readonly GLFWCallbacks.CursorPosCallback GLFWCursorCallback = (_, x, y) =>
	{
		if (CursorCallback == null)
			return;

		CursorCallback((float)x, (float)y);
	};
	
	private static readonly GLFWCallbacks.ScrollCallback GLFWScrollCallback = (_, x, y) =>
	{
		if (ScrollCallback == null)
			return;

		ScrollCallback((float)x, (float)y);
	};

	private static InputAction GetInputAction(OpenTK.Windowing.GraphicsLibraryFramework.InputAction inputAction)
	{
		return inputAction switch
		{
			OpenTK.Windowing.GraphicsLibraryFramework.InputAction.Press => InputAction.Press,
			OpenTK.Windowing.GraphicsLibraryFramework.InputAction.Release => InputAction.Release,
			OpenTK.Windowing.GraphicsLibraryFramework.InputAction.Repeat => InputAction.Hold,
			_ => 0
		};
	}
	
	public static void Initialize(GLAPI api, Window* window)
	{
		glapi = api;
		GL.DebugMessageCallback(DebugProc, IntPtr.Zero);
		GL.Enable(EnableCap.DebugOutput);
		
		GLFW.SetWindowSizeCallback(window, GLFWWindowSizeCallback);
		GLFW.SetKeyCallback(window, GLFWKeyCallback);
		GLFW.SetCharCallback(window, GLFWCharCallback);
		GLFW.SetMouseButtonCallback(window, GLFWMouseCallback);
		GLFW.SetCursorPosCallback(window, GLFWCursorCallback);
		GLFW.SetScrollCallback(window, GLFWScrollCallback);
		GLFW.SetErrorCallback(GLFWErrorCallback);
	}
}
