using OpenTK.Audio.OpenAL;
using System.Runtime.InteropServices;

namespace OpenAbility.Graphik.Audio.OpenAL;

[Serializable]
public unsafe class OpenAlAPI : IGraphikAudioAPI
{
	private readonly int source;
	private readonly int[] audioBuffers = new int[2];
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
		
		source = AL.GenSource();
		AL.Source(source, ALSourceb.Looping, false);

		audioBuffers = AL.GenBuffers(audioBuffers.Length);
		ErrorCheck("GenBuffers");

		foreach (var buffer in audioBuffers)
		{
			AL.BufferData(buffer, ALFormat.StereoFloat32Ext, Array.Empty<float>(), 44100);
			ErrorCheck("BufferData " + buffer);
		}
		
		AL.DistanceModel(ALDistanceModel.InverseDistanceClamped);
		AL.Listener(ALListenerfv.Orientation, new float[] {0, 0, 0, 0, 1, 0});
		
		AL.Source(source, ALSourcef.ReferenceDistance, 10.0f);
		AL.Source(source, ALSourcef.MaxDistance, 50.0f);
		AL.Source(source, ALSourcef.RolloffFactor, 2.0f);
		AL.Source(source, ALSource3f.Position, 0, 0, 0);
		
		AL.SourceQueueBuffers(source, audioBuffers);
		ErrorCheck("QueueBuffers");
		AL.SourcePlay(source);
		ErrorCheck("SourcePlay");
	}

	private void ErrorCheck(string loc)
	{
		ALError error = AL.GetError();
		if (error != ALError.NoError)
		{
			Console.Error.WriteLine("AL ERROR: " + error + ": " + AL.GetErrorString(error) + " at " + loc);
		}
	}

	public void Close()
	{
		ALC.CloseDevice(device);
		ALC.DestroyContext(context);
		ErrorCheck("Close");
	}

	public void EnqueueData(float[] data, int sampleFrequency)
	{
		int buffer = AL.SourceUnqueueBuffer(source);
		AL.BufferData(buffer, ALFormat.StereoFloat32Ext, data, sampleFrequency);
		AL.SourceQueueBuffer(source, buffer);
		AL.GetSource(source, ALGetSourcei.BuffersQueued, out int queue);
		ResumePlayback();
	}
	
	public void PausePlayback()
	{
		AL.SourcePause(source);
	}
	
	public void ResumePlayback()
	{
		if(AL.GetSourceState(source) != ALSourceState.Playing)
			AL.SourcePlay(source);
	}

	public bool RequiresNewData()
	{
		AL.GetSource(source, ALGetSourcei.BuffersProcessed, out int p);
		return p > 0;
	}
}