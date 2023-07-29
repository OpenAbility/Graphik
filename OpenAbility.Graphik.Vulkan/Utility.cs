namespace OpenAbility.Graphik.Vulkan;

public static unsafe class Utility
{
	public static byte* Ptr(this string s)
	{
		fixed (char* ptr = s.ToCharArray())
		{
			return (byte*)ptr;
		}
	}
	
}
