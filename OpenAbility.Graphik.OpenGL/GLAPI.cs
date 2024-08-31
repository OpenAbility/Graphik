using OpenAbility.Graphik.Selection;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Shaderc;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OpenAbility.Graphik.OpenGL;


[Serializable]
public unsafe class GLAPI : IGraphikAPI
{
	private Window* window;
	private HLSLCompiler compiler = new HLSLCompiler();
	private string[] extensions;
	private bool hasBindless;
	
	public int Width;
	public int Height;
	public void InitializeSystems()
	{
		GLFW.Init();
		if (InvokeLibraryFunction("__func_check", Array.Empty<object>()) is not true)
		{
			throw new Exception("Could not load library functions!");
		}
	}

	[LibraryFunction("__func_check")]
	bool NullLibFunc()
	{
		return true;
	}
	
	public IGraphikWindow InitializeWindow(string title, int width, int height)
	{

		Width = width;
		Height = height;
		
		GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
		GLFW.WindowHint(WindowHintBool.Visible, true);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 6);
		GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true); // If we somehow run on Mac, this is needed.
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

		GL.FrontFace(FrontFaceDirection.Cw);
		GL.Enable(EnableCap.DebugOutput);

		GL.Enable(EnableCap.PolygonSmooth);
		GL.Hint(HintTarget.PolygonSmoothHint, HintMode.DontCare);
		GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
		
		int extensionCount = 0;
		GL.GetInteger(GetPName.NumExtensions, ref extensionCount);
		CallbackHandler.DebugCallback?.Invoke("GLInfo", "Found " + extensionCount + " extensions!");
		extensions = new string[extensionCount];

		for (uint i = 0; i < extensionCount; i++)
		{
			extensions[i] = GL.GetStringi(StringName.Extensions, i) ?? "null";
			CallbackHandler.DebugCallback?.Invoke("GLInfo", "Found " + extensions[i]);

			if (extensions[i] == "GL_ARB_bindless_texture")
				hasBindless = true;
			if (extensions[i] == "GL_NV_bindless_texture")
				hasBindless = true;
		}
		CallbackHandler.DebugCallback?.Invoke("GLInfo", "Bindless: " + hasBindless);
		
		return new GLWindow()
		{
			handle = window
		};
	}
	public void SetWindowCurrent(IGraphikWindow window)
	{
		this.window = ((GLWindow)window).handle;
		GLFW.MakeContextCurrent(this.window);
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
		
		if (clearFlags.HasFlag(ClearFlags.Stencil))
			clearBufferMask |= ClearBufferMask.StencilBufferBit;
		
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
		return new RenderBufferRndTexture();
	}
	public IRenderBuffer CreateRenderBuffer()
	{
		return new GLRenderBuffer();
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

	public IShaderObject CreateShaderObject(ShaderType type)
	{
		return new GLShaderObject(type);
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
			case Feature.Culling:
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
			Feature.Stencil => EnableCap.StencilTest,
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
	public void SetDepthFunction(CompareFunction compareFunction)
	{
		GL.DepthFunc(compareFunction switch
		{
			CompareFunction.GrEqual => DepthFunction.Gequal,
			CompareFunction.Less => DepthFunction.Less,
			CompareFunction.Greater => DepthFunction.Greater,
			CompareFunction.LessEqual => DepthFunction.Lequal,
			CompareFunction.Always => DepthFunction.Always,
			CompareFunction.Equal => DepthFunction.Equal,
			CompareFunction.Never => DepthFunction.Never,
			CompareFunction.NotEqual => DepthFunction.Notequal,
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
	public IShaderCompiler GetCompiler()
	{
		return compiler;
	}

	private static readonly string[] SupportedLanguages = new string[]
	{
		"glsl",
		"hlsl"
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
			case ".hlsl":
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

	private static IncludeResult includeResult;
	private static byte[] includeDataBuffer = Array.Empty<byte>();
	
	public void SetIncludeCallback(OpenAbility.Graphik.IncludeCallback includeCallback)
	{
		HLSLCompiler.IncludeResolve = (data, source, type, requestingSource, depth) =>
		{
			string sourcePath = Utility.GetString(requestingSource);
			string requestedPath = Utility.GetString(source);

			includeResult = new IncludeResult();

			Include result = includeCallback(requestedPath, sourcePath);
			includeDataBuffer = new byte[result.ResultingPath.Length + result.Code.Length];
			
			result.Code.EncodeInto(includeDataBuffer, false);
			result.ResultingPath.EncodeInto(includeDataBuffer.AsSpan()[result.Code.Length..], false);

			fixed (byte* dataPtr = includeDataBuffer)
			{
				includeResult.Content = dataPtr;
				includeResult.SourceName = dataPtr + result.Code.Length;
					
				includeResult.ContentLength = new UIntPtr((uint)result.Code.Length);
				includeResult.SourceNameLength = new UIntPtr((uint)result.ResultingPath.Length);
			}
			
			fixed(IncludeResult* ptr = &includeResult)
				return ptr;
		};

		HLSLCompiler.Releaser = (data, result) =>
		{

		};
	}


	private static readonly Dictionary<string, MethodInfo> libraryFunctions = new Dictionary<string, MethodInfo>();
	private static bool searchedFunctions;

	private static MethodInfo? GetLibraryFunction(string name)
	{
		if (!searchedFunctions)
		{
			searchedFunctions = true;
			foreach (var method in typeof(GLAPI).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
			{
				LibraryFunctionAttribute? lfa = method.GetCustomAttribute<LibraryFunctionAttribute>();
				if (lfa != null)
				{
					libraryFunctions.Add(lfa.Name, method);
				}
			}
		}

		return libraryFunctions.GetValueOrDefault(name);
	}

	private static bool CheckArgs(MethodInfo info, object[] parameters)
	{
		if (info.GetParameters().Length != parameters.Length)
			return false;

		for (int i = 0; i < parameters.Length; i++)
		{
			if (!info.GetParameters()[i].ParameterType.IsInstanceOfType(parameters[i]))
				return false;
		}
		return true;
	}
	
	public object? InvokeLibraryFunction(string function, object[] parameters)
	{

		MethodInfo? methodInfo = GetLibraryFunction(function);

		if (methodInfo == null)
			return null;

		if (!CheckArgs(methodInfo, parameters))
			return null;

		if (methodInfo.IsStatic)
			return methodInfo.Invoke(null, parameters);
		
		return methodInfo.Invoke(this, parameters);
	}

	[LibraryFunction(LibraryFunctions.SetClipboardString)]
	private void SetClipboardString(string str)
	{
		GLFW.SetClipboardString(window, str);
	}
	
	[LibraryFunction(LibraryFunctions.GetClipboardString)]
	private string GetClipboardString()
	{
		return GLFW.GetClipboardString(window);
	}

	[LibraryFunction(LibraryFunctions.FinishProcessing)]
	private void FinishProcessing()
	{
		GL.Finish();
	}

	[LibraryFunction("get_vendor")]
	private string GetVendor()
	{
		return GL.GetString(StringName.Vendor) ?? "NULL";
	}
	
	[LibraryFunction("get_renderer")]
	private string GetRenderer()
	{
		return GL.GetString(StringName.Renderer) ?? "NULL";
	}

		
	[LibraryFunction("__fbo_test")]
	private string TestFBO()
	{

		const int Width = 800;
		const int Height = 600;
		
		FramebufferHandle handle = GL.CreateFramebuffer();

		TextureHandle colourTexture0 = GL.CreateTexture(TextureTarget.Texture2d);
		TextureHandle colourTexture1 = GL.CreateTexture(TextureTarget.Texture2d);
		TextureHandle dsTexture = GL.CreateTexture(TextureTarget.Texture2d);
		
		SetTextureDefaults(colourTexture0);
		GL.TextureStorage2D(colourTexture0, 1, SizedInternalFormat.Rgba8, Width, Height);
		
		SetTextureDefaults(colourTexture1);
		GL.TextureStorage2D(colourTexture1, 1, SizedInternalFormat.Rgba8, Width, Height);
		
		SetTextureDefaults(dsTexture);
		GL.TextureStorage2D(dsTexture, 1, SizedInternalFormat.Depth24Stencil8, Width, Height);
		
		
		GL.NamedFramebufferTexture(handle, FramebufferAttachment.ColorAttachment0, colourTexture0, 0);
		GL.NamedFramebufferTexture(handle, FramebufferAttachment.ColorAttachment1, colourTexture1, 0);
		
		GL.NamedFramebufferTexture(handle, FramebufferAttachment.DepthStencilAttachment, dsTexture, 0);
		
		/*
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
		GL.BindTexture(TextureTarget.Texture2d, dsTexture);
		GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2d, dsTexture, 0);
		*/
		GL.NamedFramebufferDrawBuffers(handle, new [] { ColorBuffer.ColorAttachment0, ColorBuffer.ColorAttachment1 });
		
		string res = GL.CheckNamedFramebufferStatus(handle, FramebufferTarget.Framebuffer).ToString();
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
		return res;
	}

	private void SetTextureDefaults(TextureHandle handle)
	{
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
	}
	
	public object? GetLibraryValue(string value)
	{
		return value switch
		{
			"threaded" => false,
			_ => null
		};

	}
	
	public string GetLibraryIdentifier()
	{
		return "OpenAbility.Graphik.OpenGL";
	}

	internal static APISpecification Specifier()
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

	internal static ulong Rate(APIRequest request)
	{
		return 1;
	}

	internal static IGraphikAPI Create() => new GLAPI();

	private static uint MarkerID = 0;
	public void LogMarker(string marker)
	{
		GL.DebugMessageInsert(DebugSource.DebugSourceApplication, DebugType.DebugTypeMarker, MarkerID++, DebugSeverity.DebugSeverityNotification, marker.Length, marker);
	}
	public void ClearColour(float r, float g, float b, float a)
	{
		GL.ClearColor(r, g, b, a);
	}
	
	private StencilOp GetOp(StencilOperation operation)
	{
		return operation switch
		{

			StencilOperation.Keep => StencilOp.Keep,
			StencilOperation.Zero => StencilOp.Zero,
			StencilOperation.Increment => StencilOp.Incr,
			StencilOperation.Decrement => StencilOp.Decr,
			StencilOperation.Invert => StencilOp.Invert,
			StencilOperation.Replace => StencilOp.Replace,
			StencilOperation.IncrementWarp => StencilOp.IncrWrap,
			StencilOperation.DecrementWarp => StencilOp.DecrWrap,
			_ => 0
		};
	}

	public void SetStencilOperation(CullFace face, StencilOperation stencilFail, StencilOperation depthFail, StencilOperation pass)
	{
		TriangleFace triangleFace = face switch
		{

			CullFace.Front => TriangleFace.Front,
			CullFace.Back => TriangleFace.Back,
			CullFace.Both => TriangleFace.FrontAndBack,
			_ => TriangleFace.FrontAndBack
		};
		
		GL.StencilOpSeparate(triangleFace, GetOp(stencilFail), GetOp(depthFail), GetOp(pass));
	}
	public bool Supports(SupportCap cap)
	{
		if (cap == SupportCap.SeparateStencil)
			return false;
		return true; // We assume we support it haha
	}

	public void SetStencilMask(CullFace face, byte mask)
	{
		TriangleFace triangleFace = face switch
		{

			CullFace.Front => TriangleFace.Front,
			CullFace.Back => TriangleFace.Back,
			CullFace.Both => TriangleFace.FrontAndBack,
			_ => TriangleFace.FrontAndBack
		};
		
		GL.StencilMaskSeparate(triangleFace, mask);
	}
    
	public void SetStencilFunction(CullFace face, CompareFunction function, byte compareValue, byte mask = 0xFF)
	{

		TriangleFace triangleFace = face switch
		{

			CullFace.Front => TriangleFace.Front,
			CullFace.Back => TriangleFace.Back,
			CullFace.Both => TriangleFace.FrontAndBack,
			_ => TriangleFace.FrontAndBack
		};

		StencilFunction stencilFunction = function switch
		{
			CompareFunction.GrEqual => StencilFunction.Gequal,
			CompareFunction.Less => StencilFunction.Less,
			CompareFunction.Greater => StencilFunction.Greater,
			CompareFunction.LessEqual => StencilFunction.Lequal,
			CompareFunction.Always => StencilFunction.Always,
			CompareFunction.Equal => StencilFunction.Equal,
			CompareFunction.Never => StencilFunction.Never,
			CompareFunction.NotEqual => StencilFunction.Notequal,
			_ => 0
		};
		
		GL.StencilFuncSeparate(triangleFace, stencilFunction, compareValue, mask);
	}
}