namespace OpenAbility.Graphik;

// Basically, an SSBO
public unsafe interface IShaderBuffer
{
	public virtual void PushData<T>(ReadOnlySpan<T> data) where T : unmanaged
	{
		fixed(T* pointer = data)
			PushData(pointer, data.Length);
	}

	public virtual void PushData<T>(Span<T> data) where T : unmanaged
	{
		PushData((ReadOnlySpan<T>)data);
	}
	
	public virtual void PushData<T>(T* data, int size) where T : unmanaged
	{
		PushData((void*)data, size);
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
