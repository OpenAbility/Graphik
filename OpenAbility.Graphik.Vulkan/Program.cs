// See https://aka.ms/new-console-template for more information

using OpenAbility.Graphik.Vulkan;

Console.WriteLine("Hello, World!");

VulkanAPI vulkanAPI = new VulkanAPI();

vulkanAPI.Initialize();
vulkanAPI.InitializeWindow();

while (!vulkanAPI.WindowShouldClose())
{
	vulkanAPI.BeginFrame();
	
	vulkanAPI.EndFrame();
}

vulkanAPI.Cleanup();