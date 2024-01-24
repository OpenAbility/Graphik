using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.GLFW;
using Silk.NET.Vulkan;
using System.Runtime.InteropServices;

namespace OpenAbility.Graphik.Vulkan;

public unsafe class VulkanAPI
{

	public static Version32 Version = new Version32(1, 0, 0);

	public static Glfw glfw = Glfw.GetApi();
	public static Vk vk = Vk.GetApi();

	private WindowHandle* window;

	private Func<Dictionary<string, object>, int>? rater = null;

	private Instance vkInstance;
	private PhysicalDevice vkPhysicalDevice;
	private Device vkDevice;
	private Queue vkGraphicsQueue;
	private SurfaceKHR vkSurface;
	private DebugUtilsMessengerEXT vkDebugMessenger;
	
	private readonly List<string> RequiredLayers = new List<string>()
	{

	};
	private readonly List<string> RequestedLayers = new List<string>()
	{
		"VK_LAYER_KHRONOS_validation"
	};
	private readonly List<string> NotFoundLayers = new List<string>();
	private string[] AvailableLayers;
	private bool LayersRequired = true;

	public void Initialize()
	{

	}

	private Silk.NET.GLFW.GlfwCallbacks.ErrorCallback ErrorCallback = (error, description) =>
	{
		Console.Error.WriteLine("GLFW: " + error + ": " + description);
	};
	
	public void InitializeWindow()
	{
		glfw.Init();

		glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
		glfw.WindowHint(WindowHintBool.Resizable, false);
		glfw.WindowHint(WindowHintBool.OpenGLDebugContext, true);

		glfw.SetErrorCallback(ErrorCallback);

		window = glfw.CreateWindow(800, 600, "VulkanAPI", null, null);

		ApplicationInfo application = new ApplicationInfo()
		{
			SType = StructureType.ApplicationInfo,
			PApplicationName = (byte*)SilkMarshal.StringToPtr(Graphik.ApplicationName),
			PEngineName = (byte*)SilkMarshal.StringToPtr("Graphik w/ VulkanAPI"),
			ApiVersion = Vk.Version10,
			ApplicationVersion = Vk.MakeVersion((uint)Graphik.ApplicationVersion.Major,
				(uint)Graphik.ApplicationVersion.Minor,
				(uint)Graphik.ApplicationVersion.Build),
			EngineVersion = Version
		};

		InstanceCreateInfo createInfo = new InstanceCreateInfo()
		{
			SType = StructureType.InstanceCreateInfo, PApplicationInfo = &application
		};

		uint requiredExtCount;
		byte** requiredInstanceExtensions = glfw.GetRequiredInstanceExtensions(out requiredExtCount);

		createInfo.EnabledExtensionCount = requiredExtCount;
		createInfo.PpEnabledExtensionNames = requiredInstanceExtensions;

		string[] ext = SilkMarshal.PtrToStringArray((IntPtr)requiredInstanceExtensions, (int)requiredExtCount);
		Console.WriteLine("GLFW required: " + String.Join(", ", ext));
		bool supported = !(LayersRequired && !GetLayersSupported());

		if (NotFoundLayers.Count > 0)
		{
			Console.Error.WriteLine("Could not find layers:\n\t" + String.Join(",\n\t", NotFoundLayers));
		}

		if (!supported)
		{
			throw new Exception("Could not create Vk instance: Could not find required layers!");
		}
		
		Console.WriteLine("Using Layers {0}", String.Join(", ", AvailableLayers.Select(l => "'" + l + "'")));

		if (LayersRequired)
		{
			createInfo.EnabledLayerCount = (uint)AvailableLayers.Length;
			createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(AvailableLayers);
		}
		else
		{
			createInfo.EnabledLayerCount = 0;
		}

		Result createResult = vk.CreateInstance(createInfo, null, out vkInstance);

		if (createResult != Result.Success)
		{
			throw new Exception("Could not create Vk instance: " + createResult);
		}

		// TODO: Debug callbacks

		if (LayersRequired)
		{
		}
		
		CreateSurface();

		PickPhysicalDevice();
		
		// Time to create our device(logical one!)
		CreateLogicalDevice();
	}

	private void CreateLogicalDevice()
	{
		DeviceQueueDescription queueDescription = GetQueueDescription(vkPhysicalDevice);

		float priority = 1.0f;
		DeviceQueueCreateInfo queueCreateInfo = new DeviceQueueCreateInfo()
		{
			SType = StructureType.DeviceQueueCreateInfo,
			QueueFamilyIndex = queueDescription.GraphicsFamily!.Value, 
			QueueCount = 1,
			PQueuePriorities = &priority
		};

		PhysicalDeviceFeatures deviceFeatures = new PhysicalDeviceFeatures();
		
		DeviceCreateInfo createInfo = new DeviceCreateInfo()
		{
			SType = StructureType.DeviceCreateInfo,
			PQueueCreateInfos = &queueCreateInfo,
			QueueCreateInfoCount = 1,
			PEnabledFeatures = &deviceFeatures,
			EnabledExtensionCount = 0
		};
		
		if (LayersRequired)
		{
			createInfo.EnabledLayerCount = (uint)AvailableLayers.Length;
			createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(AvailableLayers);
		}

		Result result = vk.CreateDevice(vkPhysicalDevice, &createInfo, null, out vkDevice);
		if (result != Result.Success)
			throw new Exception("Could not create logical Vk device: " + result);

		// Queue fetching
		vk.GetDeviceQueue(vkDevice, queueDescription.GraphicsFamily!.Value, 0, out vkGraphicsQueue);
	}

	private void CreateSurface()
	{
		Result result;
		fixed(SurfaceKHR* surfacePtr = &vkSurface)
			result = (Result)glfw.CreateWindowSurface(vkInstance.ToHandle(), window, null, (VkNonDispatchableHandle*)surfacePtr);
		if (result != Result.Success)
		{
			throw new Exception("Could not create GLFW window surface: " + result);
		}
	}

	private void PickPhysicalDevice()
	{
		uint deviceCount = 0;
		vk.EnumeratePhysicalDevices(vkInstance, ref deviceCount, null);

		if (deviceCount == 0)
		{
			throw new Exception("No VK devices found! Are you missing a driver or GPU?");
		}

		PhysicalDevice[] physicalDevices = new PhysicalDevice[deviceCount];
		fixed (PhysicalDevice* dev = physicalDevices)
			vk.EnumeratePhysicalDevices(vkInstance, ref deviceCount, dev);

		List<(int score, PhysicalDevice device)> devices = new List<(int, PhysicalDevice)>();

		foreach (var d in physicalDevices)
		{
			vk.GetPhysicalDeviceProperties(d, out PhysicalDeviceProperties props);
			devices.Add((RateDevice(d, props), d));
		}
		devices.Sort((a, b) => a.score - b.score);

		if (devices[0].score <= 0)
		{
			throw new Exception("No suitable VK devices found! Are you satisfying minimal specs?");
		}

		vkPhysicalDevice = devices[0].device;
		
        vk.GetPhysicalDeviceProperties(vkPhysicalDevice, out PhysicalDeviceProperties selectedProps);

        Console.WriteLine("Using physical device '{0}'", SilkMarshal.PtrToString((IntPtr)selectedProps.DeviceName));
	}

	public DeviceQueueDescription GetQueueDescription(PhysicalDevice device)
	{
		QueueFamilyProperties[] familyProperties =
			Utilities.GetEnumerator<PhysicalDevice, QueueFamilyProperties>(device, 
				vk.GetPhysicalDeviceQueueFamilyProperties);
		DeviceQueueDescription description = new DeviceQueueDescription();
		uint index = 0;
		foreach (var property in familyProperties)
		{
			if (property.QueueFlags.HasFlag(QueueFlags.GraphicsBit))
				description.GraphicsFamily = index;
			index++;
		}
		return description;
	}

	private int RateDevice(PhysicalDevice deviceHandle, PhysicalDeviceProperties device)
	{
		DeviceQueueDescription description = GetQueueDescription(deviceHandle);
		if (rater != null)
		{
			Dictionary<string, object> info = new Dictionary<string, object>();
			info["handle"] = deviceHandle;
			info["queues"] = description;
			return rater(info);
		}

		int score = 0;

		// Discrete GPUs have a significant performance advantage
		if (device.DeviceType == PhysicalDeviceType.DiscreteGpu)
		{
			score += 8000;
		}

		if (description.GraphicsFamily == null)
			return 0;

		// Maximum possible size of textures affects graphics quality
		score += (int)device.Limits.MaxImageDimension2D;
		
		return score;
	}
	
	private bool GetLayersSupported()
	{
		uint layerCount = 0;
		vk.EnumerateInstanceLayerProperties(ref layerCount, null);

		LayerProperties[] propertiesArray = new LayerProperties[layerCount];
		fixed (LayerProperties* props = propertiesArray)
			vk.EnumerateInstanceLayerProperties(ref layerCount, props);

		string[] properties = new string[layerCount];

		for (int i = 0; i < layerCount; i++)
		{
			LayerProperties props = propertiesArray[i];
			properties[i] = SilkMarshal.PtrToString((IntPtr)props.LayerName) ?? "";
			
			Console.WriteLine("Found Layer: " + properties[i]);
		}

		bool found = true;
		List<string> layers = new List<string>();

		foreach (var layer in RequiredLayers)
		{
			if (!properties.Contains(layer))
			{
				found = false;
				NotFoundLayers.Add(layer);
			}
			else
			{
				layers.Add(layer);
			}
		}
		
		foreach (var layer in RequestedLayers)
		{
			if (!properties.Contains(layer))
			{
				NotFoundLayers.Add(layer);
			}
			else
			{
				layers.Add(layer);
			}
		}

		AvailableLayers = layers.ToArray();

		return found;
	}

	public bool WindowShouldClose()
	{
		return glfw.WindowShouldClose(window);
	}

	public void BeginFrame()
	{
		glfw.PollEvents();
	}

	public void EndFrame()
	{
		
	}

	public void RequireLayer(string layer)
	{
		RequiredLayers.Add(layer);
	}
	
	public void RequestLayer(string layer)
	{
		RequestedLayers.Add(layer);
	}
	
	public void RequireLayers(bool required)
	{
		LayersRequired = required;
	}

	public void Cleanup()
	{
		vk.DestroyDevice(vkDevice, null);
		vk.DestroyInstance(vkInstance, null);
		
		glfw.DestroyWindow(window);
		glfw.Terminate();
	}
}
