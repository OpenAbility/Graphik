namespace OpenAbility.Graphik;

public interface IMesh
{
	public void PrepareModifications();
	public void SetIndexType(IndexType type);
	public void SetVertexData<T>(T[] data, bool reallocate = true, bool preferQuickWrite = false) where T : unmanaged
	{
		SetVertexData((Span<T>)data, reallocate, preferQuickWrite);
	}
	public void SetVertexData<T>(Span<T> data, bool reallocate = true, bool preferQuickWrite = false) where T : unmanaged;
	public void SetVertexData(IntPtr data, int size, bool reallocate = true, bool preferQuickWrite = false);
	public void SetIndices<T>(T[] data, bool reallocate = true, bool preferQuickWrite = false) where T : unmanaged
	{
		SetIndices((Span<T>)data, reallocate, preferQuickWrite);
	}
	public void SetIndices<T>(Span<T> indices, bool reallocate = true, bool preferQuickWrite = false) where T : unmanaged;
	public void SetIndices(IntPtr data, int size, bool reallocate = true, bool preferQuickWrite = false);
	public void AllocateVertexData(int size, bool quickWrite = false);
	public void AllocateIndexData(int size, bool quickWrite = false);
	public int GetVertexBufferSize();
	public int GetIndexBufferSize();
	public void SetVertexAttrib(uint index, int size, VertexAttribType vertexAttribType, int stride, int offset, bool normalized = false);
	public void Render(int indices, int vertexOffset, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0);
	public void Render(int indices, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0);
	public void RenderInstanced(int indices, int instances, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0);
	public void Dispose();
	public void SetName(string name);
	public void Bind();
}

