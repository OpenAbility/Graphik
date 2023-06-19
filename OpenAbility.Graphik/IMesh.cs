namespace OpenAbility.Graphik;

public interface IMesh
{
	public void PrepareModifications();
	public void SetVertexData(float[] data);
	public void SetIndices(uint[] indices);
	public void AllocateVertexData(int size);
	public void AllocateIndexData(int size);
	public int GetVertexBufferSize();
	public int GetIndexBufferSize();
	public void SetVertexAttrib(uint index, int size, VertexAttribType vertexAttribType, int stride, int offset, bool normalized = false);
	public void Render(int indices, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0, int vertexOffset = 0);
	public void Dispose();
}
