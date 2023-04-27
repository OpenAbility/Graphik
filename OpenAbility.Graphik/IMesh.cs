namespace OpenAbility.Graphik;

public interface IMesh
{
	public void PrepareModifications();
	public void SetVertexData(float[] data);
	public void SetIndices(uint[] indices);
	public void SetVertexAttrib(uint index, int size, VertexAttribType vertexAttribType, int stride, int offset);
	public void Render(int indices, int offset = 0);
	public void Dispose();
}
