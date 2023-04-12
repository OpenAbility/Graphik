using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenAbility.Graphik.OpenGL;

public unsafe class GLAPI : IGraphikAPI
{
	private Window* window;
	private ErrorCallback? errorCallback;
	private GLDebugProc debugProc;
	private GLFWCallbacks.WindowSizeCallback windowSizeCallback;
	private int width;
	private int height;
	public void InitializeSystems()
	{
		GLFW.Init();
	}

	public void InitializeWindow(string title, int width, int height)
	{

		this.width = width;
		this.height = height;
		
		GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
		GLFW.WindowHint(WindowHintBool.Visible, true);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 6);
		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		
#if DEBUG
		GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, true);
#else
		GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, false);
#endif
		
		window = GLFW.CreateWindow(width, height, title, null, null);
		
		GLFW.MakeContextCurrent(window);
		GLLoader.LoadBindings(new GLFWBindingsContext());
		
		debugProc = GLDebug;
		GL.DebugMessageCallback(debugProc, IntPtr.Zero);
		GL.Enable(EnableCap.DebugOutput);


		windowSizeCallback = (_, width, height) =>
		{
			this.width = width;
			this.height = height;
			GL.Viewport(0, 0, width, height);
		};

		GLFW.SetWindowSizeCallback(window, windowSizeCallback);
	}
	private void GLDebug(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
	{
		if(errorCallback == null)
			return;

		byte[] data = new byte[length];
		Marshal.Copy(message, data, 0, length);

		errorCallback("GL_" + source + "_" + type + severity, Encoding.Default.GetString(data));
	}

	public void SetErrorCallback(ErrorCallback errorCallback)
	{
		this.errorCallback = errorCallback;
	}


	public bool WindowShouldClose()
	{
		return GLFW.WindowShouldClose(window);
	}
	
	public void InitializeFrame()
	{
		GLFW.PollEvents();
	}
	
	public void FinishFrame()
	{
		GLFW.SwapBuffers(window);
	}
	public void Clear(ClearFlags clearFlags)
	{
		ClearBufferMask clearBufferMask = 0;

		if (clearFlags.HasFlag(ClearFlags.Colour))
			clearBufferMask |= ClearBufferMask.ColorBufferBit;
		
		if (clearFlags.HasFlag(ClearFlags.Depth))
			clearBufferMask |= ClearBufferMask.DepthBufferBit;
		
		GL.Clear(clearBufferMask);
	}
	
	public ITexture CreateTexture()
	{
		return new GLTexture();
	}
	public IMesh CreateMesh()
	{
		return new GLMesh();
	}
	public IShader CreateShader()
	{
		return new GLShader();
	}

	public GLRenderTexture CreateRenderTexture()
	{
		return new GLRenderTexture();
	}

	public void ResetTarget()
	{
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
		GL.Viewport(0, 0, width, height);
	}
}
