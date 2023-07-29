using OpenAbility.Graphik.Vulkan;
using Silk.NET.GLFW;
using Silk.NET.Vulkan;

HelloTriangleApplication application = new HelloTriangleApplication();
try
{
	application.Run();
}
catch (Exception e)
{
	Console.Error.WriteLine(e);
	Environment.Exit(1);
}

unsafe class HelloTriangleApplication
{

	private Glfw glfw;
	private Vk vk;

	private WindowHandle* window;

	private const int Width = 800;
	private const int Height = 600;
	
	public void Run()
	{
		InitVulkan();
		MainLoop();
		Cleanup();
	}

	private void InitVulkan()
	{
		glfw = Glfw.GetApi();
		vk = Vk.GetApi();
		
		glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
		glfw.WindowHint(WindowHintBool.Resizable, false);

		window = glfw.CreateWindow(Width, Height, "Hello, Vulkan!", null, null);
		
		CreateInstance();
	}

	private void CreateInstance()
	{
		ApplicationInfo appInfo = new ApplicationInfo(pApplicationName: "Hello Triangle".Ptr());
	}

	private void MainLoop()
	{
		while (!glfw.WindowShouldClose(window))
		{
			glfw.PollEvents();
		}
	}

	private void Cleanup()
	{
		glfw.DestroyWindow(window);
		glfw.Terminate();
	}
}