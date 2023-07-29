using System.Numerics;

namespace OpenAbility.Graphik.Audio;

public interface IAudioSource
{
	/// <summary>
	/// Get the handle to the underlying audio source
	/// </summary>
	/// <returns>The handle</returns>
	public uint GetHandle();
	/// <summary>
	/// Set the buffer of this source
	/// </summary>
	/// <param name="buffer">The buffer to set it to</param>
	public void SetBuffer(IAudioBuffer buffer);
	/// <summary>
	/// Enqueue a buffer for playback
	/// </summary>
	/// <param name="buffer">The buffer to enqueue</param>
	public void EnqueueBuffer(IAudioBuffer buffer);
	/// <summary>
	/// Dequeue a buffer
	/// </summary>
	/// <returns>The dequeued buffer</returns>
	public IAudioBuffer DequeueBuffer();
	/// <summary>
	/// Start playing
	/// </summary>
	public void Play();
	/// <summary>
	/// Pause playback
	/// </summary>
	public void Pause();
	/// <summary>
	/// Stop playback(pauses and rewinds to beginning)
	/// </summary>
	public void Stop();
	/// <summary>
	/// Restart playback
	/// </summary>
	public void Restart();
	
	/// <summary>
	/// The position of the source
	/// </summary>
	public Vector3 Position { get; set; }
	/// <summary>
	/// The direction of the source
	/// </summary>
	public Vector3 Direction { get; set; }
	/// <summary>
	/// The velocity of the source
	/// </summary>
	public Vector3 Velocity { get; set; }
	/// <summary>
	/// If the source loops its buffers(this will break streaming sources)
	/// </summary>
	public bool Looping { get; set; }
	/// <summary>
	/// If all transformation(Position, Direction, Velocity etc) are relative to the listener
	/// </summary>
	public bool Relative { get; set; }
	/// <summary>
	/// The pitch of the source, also changes speed.
	/// </summary>
	public float Pitch { get; set; }
	/// <summary>
	/// The gain of the source. In other words, the volume.
	/// </summary>
	public float Gain { get; set; }
	/// <summary>
	/// The maximum distance at which the source is heard
	/// </summary>
	public float Distance { get; set; }
	/// <summary>
	/// The reference distance(for attenuation calculations)
	/// </summary>
	public float ReferenceDistance { get; set; }
	/// <summary>
	/// The state of this source
	/// </summary>
	public SourceState State { get; }
	/// <summary>
	/// How many buffers has the source finished?
	/// </summary>
	public int FinishedBuffers { get; }
}
