using OpenTK.Graphics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenAbility.Graphik.OpenGL;

public unsafe class GLAPI : IGraphikAPI
{
	private Window* window;
	public void InitializeSystems()
	{
		GLFW.Init();
	}

	public void InitializeWindow(string title, int width, int height) {
		GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
		GLFW.WindowHint(WindowHintBool.Visible, true);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		
#if DEBUG
		GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, true);
#else
		GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, false);
#endif
		
		window = GLFW.CreateWindow(width, height, title, null, null);
		
		GLFW.MakeContextCurrent(window);
		GLLoader.LoadBindings(new GLFWBindingsContext());
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
}
