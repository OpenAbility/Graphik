namespace OpenAbility.Graphik.Audio;

public interface IAudioBuffer
{	
	/// <summary>
	/// Set the data of the buffer
	/// </summary>
	/// <param name="buffer">The data buffer to read from. Floats are expected</param>
	/// <param name="sampleRate">The sample rate of the data</param>
	/// <param name="channels">The amount of channels</param>
	/// <typeparam name="T">The type of data</typeparam>
	/// <remarks>Please do not expect full support for non-mono or stereo channels.</remarks>
	public void SetData<T>(T[] buffer, int sampleRate, int channels) where T : unmanaged;
	/// <summary>
	/// Get the underlying buffer handle
	/// </summary>
	/// <returns>The underlying buffer handle</returns>
	public uint GetHandle();
}
