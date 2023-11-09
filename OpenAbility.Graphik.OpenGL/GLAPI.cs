using OpenAbility.Graphik.Selection;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OpenAbility.Graphik.OpenGL;

[Serializable]
public unsafe class GLAPI : IGraphikAPI
{
	private Window* window;
	private HLSLCompiler compiler = new HLSLCompiler();
	
	public int Width;
	public int Height;
	public void InitializeSystems()
	{
		GLFW.Init();
	}
	
	public static void LoadAssembly()
	{
		
	}
	
	public void InitializeWindow(string title, int width, int height)
	{

		Width = width;
		Height = height;
		
		GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
		GLFW.WindowHint(WindowHintBool.Visible, true);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 6);
		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Compat);
		
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
		GL.Enable(EnableCap.DebugOutput);
	}

	#region Callback Functions
	public void SetErrorCallback(ErrorCallback errorCallback)
	{
		CallbackHandler.ErrorCallback = errorCallback;
	}
	public void SetDebugCallback(DebugCallback debugCallback)
	{
		CallbackHandler.DebugCallback = debugCallback;
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
	public void SetScrollCallback(ScrollCallback scrollCallback)
	{
		CallbackHandler.ScrollCallback = scrollCallback;
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
		GLRenderTexture.Bound = null;
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
			GLFW.SwapInterval(enabled ? 1 : 0);
			return;
		}
		
		if (enabled)
			EnableFeature(feature);
		else
			DisableFeature(feature);
	}

	private void DisableFeature(Feature feature)
	{
		if (feature == Feature.Wireframe)
		{
			GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
			return;
		}
		if (feature == Feature.DepthWrite)
		{
			GL.DepthMask(false);
			return;
		}
		if (feature == Feature.DebugOutput)
		{
			GL.Disable(EnableCap.DebugOutput);
			GL.Disable(EnableCap.DebugOutputSynchronous);
			return;
		}
		GL.Disable(GetFeatureCap(feature));	
	}

	private void EnableFeature(Feature feature)
	{
		switch (feature)
		{
			case Feature.Wireframe:
				GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
				return;
			case Feature.DepthWrite:
				GL.DepthMask(true);
				break;
			case Feature.DebugOutput:
				GL.Enable(EnableCap.DebugOutput);
				GL.Enable(EnableCap.DebugOutputSynchronous);
				return;
			default:
				GL.Enable(GetFeatureCap(feature));
				return;
		}
	}

	private EnableCap GetFeatureCap(Feature feature)
	{
		return feature switch
		{
			Feature.Blending => EnableCap.Blend,
			Feature.Culling => EnableCap.CullFace,
			Feature.DepthTesting => EnableCap.DepthTest,
			Feature.HDR => EnableCap.FramebufferSrgb,
			Feature.Scissor => EnableCap.ScissorTest,
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

	public void SetScissorArea(int x, int y, int width, int height)
	{
		GL.Scissor(x, y, width, height);
	}
	public ITexture2D FromNative(uint handle)
	{
		return new GLTexture(handle);
	}
	public void SetBlending(BlendMode blendMode)
	{

		BlendEquationModeEXT blendEquationModeExt = blendMode switch
		{
			BlendMode.Additive => BlendEquationModeEXT.FuncAdd,
			BlendMode.Subtractive => BlendEquationModeEXT.FuncSubtract,
			BlendMode.Max => BlendEquationModeEXT.Max,
			BlendMode.Min => BlendEquationModeEXT.Min,
			_ => 0
		};
		
		GL.BlendEquation(blendEquationModeExt);
	}
	public void SetBlendFunction(BlendFactor a, BlendFactor b)
	{
		GL.BlendFunc(GetBlendingFactor(a), GetBlendingFactor(b));
	}
	public void SetDepthFunction(DepthFunction depthFunction)
	{
		GL.DepthFunc(depthFunction switch
		{
			DepthFunction.Greater => OpenTK.Graphics.OpenGL.DepthFunction.Greater,
			DepthFunction.Less => OpenTK.Graphics.OpenGL.DepthFunction.Less,
			DepthFunction.GrEqual => OpenTK.Graphics.OpenGL.DepthFunction.Gequal,
			DepthFunction.LEqual => OpenTK.Graphics.OpenGL.DepthFunction.Lequal,
			_ => 0
		});
	}
	
	public IController GetController(int controllerID)
	{
		return new GLFWController(controllerID);
	}
	
	public void UnbindTextures()
	{
		GL.BindTexture(TextureTarget.Texture2d, TextureHandle.Zero);
	}
	public Vector2 ContentScale()
	{
		GLFW.GetWindowContentScale(window, out float x, out float y);
		return new Vector2(x, y);
	}
	
	public void LineWidth(float width)
	{
		GL.LineWidth(width);
	}
	public void PointSize(float size)
	{
		GL.PointSize(size);
	}
	public IShaderBuffer CreateShaderBuffer()
	{
		return new GLShaderBuffer();
	}

	private BlendingFactor GetBlendingFactor(BlendFactor factor)
	{
		return factor switch
		{
			BlendFactor.Zero => BlendingFactor.Zero,
			BlendFactor.One => BlendingFactor.One,
			BlendFactor.SrcColor => BlendingFactor.SrcColor,
			BlendFactor.OneMinusSrcColor => BlendingFactor.OneMinusSrcColor,
			BlendFactor.SrcAlpha => BlendingFactor.SrcAlpha,
			BlendFactor.OneMinusSrcAlpha => BlendingFactor.OneMinusSrcAlpha,
			BlendFactor.DstAlpha => BlendingFactor.DstAlpha,
			BlendFactor.OneMinusDstAlpha => BlendingFactor.OneMinusDstAlpha,
			BlendFactor.DstColor => BlendingFactor.DstColor,
			BlendFactor.OneMinusDstColor => BlendingFactor.OneMinusDstColor,
			BlendFactor.SrcAlphaSaturate => BlendingFactor.SrcAlphaSaturate,
			BlendFactor.ConstantColor => BlendingFactor.ConstantColor,
			BlendFactor.OneMinusConstantColor => BlendingFactor.OneMinusConstantColor,
			BlendFactor.ConstantAlpha => BlendingFactor.ConstantAlpha,
			BlendFactor.OneMinusConstantAlpha => BlendingFactor.OneMinusConstantAlpha,
			BlendFactor.Src1Alpha => BlendingFactor.Src1Alpha,
			BlendFactor.Src1Color => BlendingFactor.Src1Color,
			BlendFactor.OneMinusSrc1Color => BlendingFactor.OneMinusSrc1Color,
			BlendFactor.OneMinusSrc1Alpha => BlendingFactor.OneMinusSrc1Alpha,
			_ => 0
		};
	}

	internal static void SetLabel(ObjectIdentifier identifier, int handle, string label)
	{
		GL.ObjectLabel(identifier, (uint)handle, label.Length, label);
	}

	public ICubemapTexture CreateCubemap()
	{
		return new GLCubemap();
	}
	public IRenderTexture? GetBoundTarget()
	{
		return GLRenderTexture.Bound;
	}
	public IShaderCompiler GetCompiler()
	{
		return compiler;
	}

	private static readonly string[] SupportedLanguages = new string[]
	{
		"glsl"
	};
	
	public string[] GetSupportedLanguages()
	{
		return SupportedLanguages;
	}
	public bool IsExtensionSupported(string extension)
	{
		switch (extension)
		{
			case ".glsl":
			case ".vert":
			case ".frag":
				return true;
			default:
				return false;
		}
	}

	public void SetWindowIcons(params WindowIcon[] icons)
	{
		Image[] images = new Image[icons.Length];
		for (int i = 0; i < icons.Length; i++)
		{
			fixed (byte* dataPointer = icons[i].WindowData)
				images[i] = new Image(icons[i].Width, icons[i].Height, dataPointer);
		}
		GLFW.SetWindowIcon(window, images);
	}
	

#pragma warning disable CA2255
	[ModuleInitializer]
#pragma warning restore CA2255
	
	internal static void InitializeModule()
	{
		GraphikAPIProvider graphikAPIProvider = new GraphikAPIProvider(Create, Rate, Specifier, 20);
		GraphikAPISelector.RegisterProvider(graphikAPIProvider);
	}
	private static APISpecification Specifier()
	{
		Dictionary<string, string> spec = new Dictionary<string, string>();

		spec["name"] = "OpenAbility.Graphik.OpenGL";
		spec["description"] = "The original OpenGL backend for Graphik";
		spec["version"] = "2.0.0";
		spec["author"] = "OpenAbility";
		spec["api"] = "opengl";
		spec["stability"] = "stable";
		
		return new APISpecification(spec);
	}
	
	private static ulong Rate(APIRequest request)
	{
		return 1;
	}

	private static IGraphikAPI Create() => new GLAPI();
}