namespace OpenAbility.Graphik;

// Basically, an SSBO
public unsafe interface IShaderBuffer
{
	public void PushData<T>(Span<T> data) where T : unmanaged
	{
		fixed(T* pointer = data)
			PushData(pointer, data.Length);
	}
	public void PushData<T>(T* data, int size) where T : unmanaged
	{
		PushData((void*)data, size * sizeof(T));
	}
	public void PushData(void* data, int size);
	public void Bind(uint id);
	public void Dispose();
}
