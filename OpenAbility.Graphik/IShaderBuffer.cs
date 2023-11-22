namespace OpenAbility.Graphik;

// Basically, an SSBO
public unsafe interface IShaderBuffer
{
	public void PushData<T>(ReadOnlySpan<T> data) where T : unmanaged
	{
		fixed(T* pointer = data)
			PushData(pointer, data.Length);
	}

	public void PushData<T>(Span<T> data) where T : unmanaged
	{
		PushData((ReadOnlySpan<T>)data);
	}
	
	public void PushData<T>(T* data, int length) where T : unmanaged
	{
		PushData((void*)data, length * sizeof(T));
	}
	public long GetDataSize<T>() where T : unmanaged
	{
		return GetDataSize() / sizeof(T);
	}
	public long GetDataSize();
	public void PushData(void* data, int size);
	public void Bind(uint id);
	public void Dispose();
}
