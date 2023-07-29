// This wasn't generated, but maticulously hand crafted because properties suck balls.
using System.Numerics;

namespace OpenAbility.Graphik.Audio;

public partial class GraphikAudio
{
	public static IAudioSource GenerateSource() => audioAPI.GenerateSource();
	public static IAudioBuffer GenerateBuffer() => audioAPI.GenerateBuffer();
	public static void Close() => audioAPI.Close();
	public static Vector3 ListenerPosition
	{
		get => audioAPI.ListenerPosition;
		set
		{
			audioAPI.ListenerPosition = value;
		}
	}
	public static  ListenerOrientation ListenerOrientation	
	{
		get => audioAPI.ListenerOrientation;
		set
		{
			audioAPI.ListenerOrientation = value;
		}
	}
	public static Vector3 ListenerVelocity 	
	{
		get => audioAPI.ListenerVelocity;
		set
		{
			audioAPI.ListenerVelocity = value;
		}
	}
	public static float ListenerGain 	
	{
		get => audioAPI.ListenerGain;
		set
		{
			audioAPI.ListenerGain = value;
		}
	}
}
