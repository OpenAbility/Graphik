using System.Numerics;

namespace OpenAbility.Graphik.Audio;

public struct ListenerOrientation
{
	public Vector3 Forward;
	public Vector3 Up;
	
	public ListenerOrientation(Vector3 forward, Vector3 up)
	{
		Forward = forward;
		Up = up;
	}

}
