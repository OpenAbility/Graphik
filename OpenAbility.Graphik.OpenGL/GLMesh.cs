using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLMesh : IMesh
{
	private readonly BufferHandle vbo;
	private readonly BufferHandle ebo;
	private readonly VertexArrayHandle vao;

	public GLMesh()
	{
		vbo = GL.GenBuffer();
		ebo = GL.GenBuffer();
		vao = GL.GenVertexArray();
	}

	public void PrepareModifications()
	{
		GL.BindVertexArray(vao);
	}

	public void SetVertexData(float[] data)
	{
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
		GL.BufferData(BufferTargetARB.ArrayBuffer, data, BufferUsageARB.StaticDraw);
	}

	public void SetIndices(uint[] indices)
	{
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
		GL.BufferData(BufferTargetARB.ElementArrayBuffer, indices, BufferUsageARB.StaticDraw);
	}
	
	public void SetVertexAttrib(uint index, int size, VertexAttribType vertexAttribType, int stride, int offset)
	{
		VertexAttribPointerType vertexAttribPointerType = 0;
		switch (vertexAttribType)
		{
			case VertexAttribType.Byte:
				vertexAttribPointerType = VertexAttribPointerType.Byte;
				break;
			case VertexAttribType.Double:
				vertexAttribPointerType = VertexAttribPointerType.Double;
				break;
			case VertexAttribType.Float:
				vertexAttribPointerType = VertexAttribPointerType.Float;
				break;
			case VertexAttribType.Int:
				vertexAttribPointerType = VertexAttribPointerType.Int;
				break;
		}
		
		GL.VertexAttribPointer(index, size, vertexAttribPointerType, false, stride, offset);
		GL.EnableVertexAttribArray(index);
	}
	
	public void Render(int indices, int offset = 0)
	{
		GL.BindVertexArray(vao);
		GL.DrawElements(PrimitiveType.Triangles, indices, DrawElementsType.UnsignedInt, offset);
		GL.BindVertexArray(VertexArrayHandle.Zero);
	}
	
	public void Dispose()
	{
		GL.DeleteVertexArray(vao);
		GL.DeleteBuffer(vbo);
		GL.DeleteBuffer(ebo);
	}
}
