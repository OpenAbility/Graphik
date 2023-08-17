using OpenTK.Audio.OpenAL;
using System.Numerics;

namespace OpenAbility.Graphik.Audio.OpenAL;

public class ALAudioSource : IAudioSource
{
	private int handle;
	public ALAudioSource()
	{
		handle = AL.GenSource();
	}

	public uint GetHandle()
	{
		return (uint)handle;
	}
	
	public void SetBuffer(IAudioBuffer buffer)
	{
		AL.Source(handle, ALSourcei.Buffer, (int)buffer.GetHandle());
	}
	
	public void EnqueueBuffer(IAudioBuffer buffer)
	{
		AL.SourceQueueBuffer(handle, (int)buffer.GetHandle());
	}
	
	public IAudioBuffer DequeueBuffer()
	{
		return new ALAudioBuffer(AL.SourceUnqueueBuffer(handle));
	}
	
	public void Play()
	{
		AL.SourcePlay(handle);
	}
	public void Pause()
	{
		AL.SourcePause(handle);
	}
	public void Stop()
	{
		AL.SourceStop(handle);
	}
	public void Restart()
	{
		Stop();
		Play();
	}
	public Vector3 Position
	{
		get
		{
			AL.GetSource(handle, ALSource3f.Position, out OpenTK.Mathematics.Vector3 pos);
			return new Vector3(pos.X, pos.Y, pos.Z);
		}
		set
		{
			AL.Source(handle, ALSource3f.Position, value.X, value.Y, value.Z);
		}
	}
	public Vector3 Direction
	{
		get
		{
			AL.GetSource(handle, ALSource3f.Direction, out OpenTK.Mathematics.Vector3 pos);
			return new Vector3(pos.X, pos.Y, pos.Z);
		}
		set
		{
			AL.Source(handle, ALSource3f.Direction, value.X, value.Y, value.Z);
		}
	}
	public Vector3 Velocity 
	{
		get
		{
			AL.GetSource(handle, ALSource3f.Velocity, out OpenTK.Mathematics.Vector3 pos);
			return new Vector3(pos.X, pos.Y, pos.Z);
		}
		set
		{
			AL.Source(handle, ALSource3f.Velocity, value.X, value.Y, value.Z);
		}
	}
	public bool Looping
	{
		get
		{
			AL.GetSource(handle, ALSourceb.Looping, out bool value);
			return value;
		}
		set
		{
			AL.Source(handle, ALSourceb.Looping, value);
		}
	}
	public bool Relative	
	{
		get
		{
			AL.GetSource(handle, ALSourceb.SourceRelative, out bool value);
			return value;
		}
		set
		{
			AL.Source(handle, ALSourceb.SourceRelative, value);
		}
	}
	public float Pitch	
	{
		get
		{
			AL.GetSource(handle, ALSourcef.Pitch, out float value);
			return value;
		}
		set
		{
			AL.Source(handle, ALSourcef.Pitch, value);
		}
	}
	public float Gain
	{
		get
		{
			AL.GetSource(handle, ALSourcef.Gain, out float value);
			return value;
		}
		set
		{
			AL.Source(handle, ALSourcef.Gain, value);
		}
	}
	
	public float Distance
	{
		get
		{
			AL.GetSource(handle, ALSourcef.MaxDistance, out float value);
			return value;
		}
		set
		{
			AL.Source(handle, ALSourcef.MaxDistance, value);
		}
	}
	
	public float ReferenceDistance
	{
		get
		{
			AL.GetSource(handle, ALSourcef.ReferenceDistance, out float value);
			return value;
		}
		set
		{
			AL.Source(handle, ALSourcef.ReferenceDistance, value);
		}
	}

	public SourceState State
	{
		get
		{
			ALSourceState state = AL.GetSourceState(handle);
			return state switch
			{
				ALSourceState.Initial => SourceState.Initial,
				ALSourceState.Paused => SourceState.Paused,
				ALSourceState.Playing => SourceState.Playing,
				ALSourceState.Stopped => SourceState.Stopped,
				_ => 0
			};
		}
	}

	public int FinishedBuffers
	{
		get
		{
			AL.GetSource(handle, ALGetSourcei.BuffersProcessed, out int value);
			return value;
		}
	}
}
