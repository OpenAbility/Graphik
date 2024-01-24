namespace OpenAbility.Graphik.Vulkan;

internal static class Utilities
{
	public static unsafe T2[] GetEnumerator<T1, T2>(T1 accessed, VkEnumerator<T1, T2> enumerator) where T2 : unmanaged
	{
		uint count = 0;
		enumerator(accessed, ref count, null);

		T2[] data = new T2[count];
		fixed(T2* dataPtr = data)
			enumerator(accessed, ref count, dataPtr);
		return data;
	}
	
	public unsafe delegate void VkEnumerator<T1, T2>(T1 accessed, ref uint count, T2* pointer);
}
