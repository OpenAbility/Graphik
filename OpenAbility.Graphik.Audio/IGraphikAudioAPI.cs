using System.Numerics;

namespace OpenAbility.Graphik.Audio;

public interface IGraphikAudioAPI
{
	public IAudioSource GenerateSource();
	public IAudioBuffer GenerateBuffer();
	public void Close();
	public Vector3 ListenerPosition { get; set; }
	public ListenerOrientation ListenerOrientation { get; set; }
	public Vector3 ListenerVelocity { get; set; }
	public float ListenerGain { get; set; }
}
