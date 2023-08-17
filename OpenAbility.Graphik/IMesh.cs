namespace OpenAbility.Graphik;

public interface IMesh
{
	public void PrepareModifications();
	public void SetIndexType(IndexType type);
	public void SetVertexData<T>(T[] data, bool realloc = true) where T : unmanaged;
	public void SetVertexData(IntPtr data, int size, bool realloc = true);
	public void SetIndices<T>(T[] indices, bool realloc = true) where T : unmanaged;
	public void SetIndices(IntPtr data, int size, bool realloc = true);
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

