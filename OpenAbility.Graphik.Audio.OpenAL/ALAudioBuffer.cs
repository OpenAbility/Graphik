using OpenTK.Audio.OpenAL;

namespace OpenAbility.Graphik.Audio.OpenAL;

public class ALAudioBuffer : IAudioBuffer
{
	internal readonly int handle;
	public ALAudioBuffer()
	{
		handle = AL.GenBuffer();
	}

	internal ALAudioBuffer(int handle)
	{
		this.handle = handle;
	}

	public void SetData<T>(T[] buffer, int sampleRate, int channels) where T : unmanaged
	{
		ALFormat format = ALFormat.StereoFloat32Ext;
		if (channels == 1)
			format = ALFormat.MonoFloat32Ext;
		
		AL.BufferData(handle, format, buffer, sampleRate);
	}
	
	public uint GetHandle()
	{
		return (uint)handle;
	}
}
