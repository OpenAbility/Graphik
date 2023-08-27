using OpenTK.Audio.OpenAL;
using System.Numerics;

namespace OpenAbility.Graphik.Audio.OpenAL;

[Serializable]
public unsafe class OpenAlAPI : IGraphikAudioAPI
{
	private readonly ALDevice device;
	private readonly ALContext context;

	public OpenAlAPI()
	{
		
		device = ALC.OpenDevice(null);
		context = ALC.CreateContext(device, (int*)null);
		
		string deviceName = ALC.GetString(device, AlcGetString.AllDevicesSpecifier) ?? "unavailable";
		string vendor = AL.Get(ALGetString.Vendor) ?? "unavailable";
		string version = AL.Get(ALGetString.Version) ?? "unavailable";
		string renderer = AL.Get(ALGetString.Renderer) ?? "unavailable";
		
		ALC.MakeContextCurrent(context);
		ALC.ProcessContext(context);
		ALBase.RegisterOpenALResolver();

		Console.WriteLine("OpenAL initialized on device '{0}', by '{1}', with OpenAL version '{2}', and renderer '{3}'", deviceName, vendor, version, renderer);

		bool floatSupported = AL.IsExtensionPresent("AL_EXT_float32");
		if(floatSupported == false)
			throw new Exception("AL_EXT_float32 not supported!");
		
	}

	public IAudioBuffer GenerateBuffer()
	{
		return new ALAudioBuffer();
	}
	public void Close()
	{
		ALC.DestroyContext(context);
		ALC.CloseDevice(device);
	}
	public Vector3 ListenerPosition
	{
		get
		{
			AL.GetListener(ALListener3f.Position, out OpenTK.Mathematics.Vector3 value);
			return new Vector3(value.X, value.Y, value.Z);
		}
		set
		{
			AL.Listener(ALListener3f.Position, value.X, value.Y, value.Z);
		}
	}
	public ListenerOrientation ListenerOrientation
	{
		get
		{
			AL.GetListener(ALListenerfv.Orientation, out OpenTK.Mathematics.Vector3 fwd, out OpenTK.Mathematics.Vector3 up);
			return new ListenerOrientation(new Vector3(fwd.X, fwd.Y, fwd.Z), new Vector3(up.X, up.Y, up.Z));
		}
		set
		{
			AL.Listener(ALListenerfv.Orientation, new float[]
			{
				value.Forward.X, value.Forward.Y, value.Forward.Z,
				value.Up.X, value.Up.Y, value.Up.Z
			});
		}
	}
	public Vector3 ListenerVelocity
	{
		get
		{
			AL.GetListener(ALListener3f.Velocity, out OpenTK.Mathematics.Vector3 value);
			return new Vector3(value.X, value.Y, value.Z);
		}
		set
		{
			AL.Listener(ALListener3f.Velocity, value.X, value.Y, value.Z);
		}
	}
	public float ListenerGain
	{
		get
		{
			AL.GetListener(ALListenerf.Gain, out float value);
			return value;
		}
		set
		{
			AL.Listener(ALListenerf.Gain, value);
		}
	}
	
	public IAudioSource GenerateSource()
	{
		return new ALAudioSource();
	}
}