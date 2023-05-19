using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenAbility.Graphik.OpenGL;

public unsafe class GLAPI : IGraphikAPI
{
	private Window* window;

	public int Width;
	public int Height;
	public void InitializeSystems()
	{
		GLFW.Init();
	}

	public void InitializeWindow(string title, int width, int height)
	{

		Width = width;
		Height = height;
		
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
		
		CallbackHandler.Initialize(this, window);
		
		GL.FrontFace(FrontFaceDirection.Ccw);
	}

	#region Callback Functions
	public void SetErrorCallback(ErrorCallback errorCallback)
	{
		CallbackHandler.ErrorCallback = errorCallback;
	}
	
	public void SetResizeCallback(ResizeCallback resizeCallback)
	{
		CallbackHandler.ResizeCallback = resizeCallback;
	}
	
	public void SetKeyCallback(KeyCallback keyCallback)
	{
		CallbackHandler.KeyCallback = keyCallback;
	}
	
	public void SetMouseCallback(MouseCallback mouseCallback)
	{
		CallbackHandler.MouseCallback = mouseCallback;
	}
	
	public void SetCursorCallback(CursorCallback cursorCallback)
	{
		CallbackHandler.CursorCallback = cursorCallback;
	}
	public void SetTypeCallback(TypeCallback typeCallback)
	{
		CallbackHandler.TypeCallback = typeCallback;
	}
	#endregion

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
	
	public ITexture2D CreateTexture()
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

	public IRenderTexture CreateRenderTexture()
	{
		return new GLRenderTexture();
	}

	public void ResetTarget()
	{
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
		GL.Viewport(0, 0, Width, Height);
	}

	public void SetMouseState(MouseState mouseState)
	{
		if(mouseState == MouseState.Free)
			GLFW.SetInputMode(window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
		if(mouseState == MouseState.Hidden)
			GLFW.SetInputMode(window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
		if(mouseState == MouseState.Captured)
			GLFW.SetInputMode(window, CursorStateAttribute.Cursor, CursorModeValue.CursorDisabled);

	}

	public IShaderObject CreateShaderObject()
	{
		return new GLShaderObject();
	}

	public void SetFeature(Feature feature, bool enabled)
	{
		if (feature == Feature.VSync)
		{
			GLFW.SwapInterval(enabled ? 0 : 1);
			return;
		}
		
		
		if (enabled)
			EnableFeature(feature);
		else
			DisableFeature(feature);
	}

	private void DisableFeature(Feature feature)
	{
		GL.Disable(GetFeatureCap(feature));	
	}

	private void EnableFeature(Feature feature)
	{
		GL.Enable(GetFeatureCap(feature));	
	}

	private EnableCap GetFeatureCap(Feature feature)
	{
		return feature switch
		{
			Feature.Blending => EnableCap.Blend,
			Feature.Culling => EnableCap.CullFace,
			Feature.DepthTesting => EnableCap.DepthTest,
			Feature.HDR => EnableCap.FramebufferSrgb,
			_ => 0
		};
	}

	public void SetCullMode(CullFace cullFace)
	{
		if(cullFace == CullFace.Back)
			GL.CullFace(TriangleFace.Back);
		if(cullFace == CullFace.Front)
			GL.CullFace(TriangleFace.Front);
		if(cullFace == CullFace.Both)
			GL.CullFace(TriangleFace.FrontAndBack);
	}

	public void SetTexturePixelAlignment(int alignment)
	{
		GL.PixelStorei(PixelStoreParameter.UnpackAlignment, alignment);
	}
	public void SetWindowTitle(string title)
	{
		GLFW.SetWindowTitle(window, title);
	}
}
