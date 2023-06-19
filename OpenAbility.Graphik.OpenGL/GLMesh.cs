using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLMesh : IMesh
{
	private readonly BufferHandle vbo;
	private readonly BufferHandle ebo;
	private readonly VertexArrayHandle vao;

	private int eboSize = 0;
	private int vboSize = 0;

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

	public unsafe void SetVertexData(float[] data)
	{

		if (data.Length * sizeof(float)  != vboSize)
		{
			AllocateVertexData(data.Length * sizeof(float));
		}
		
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
		GL.BufferSubData(BufferTargetARB.ArrayBuffer, 0, data);
		
		
	}

	public unsafe void SetIndices(uint[] indices)
	{
		if (indices.Length * sizeof(uint) != eboSize)
		{
			AllocateIndexData(indices.Length * sizeof(uint));
		}
		
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
		GL.BufferSubData(BufferTargetARB.ElementArrayBuffer, 0, indices);
		
	}

	public void AllocateVertexData(int size)
	{
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
		GL.BufferData(BufferTargetARB.ArrayBuffer, size, IntPtr.Zero, BufferUsageARB.StaticDraw);
		vboSize = size;
	}

	public void AllocateIndexData(int size)
	{
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
		GL.BufferData(BufferTargetARB.ElementArrayBuffer, size, IntPtr.Zero, BufferUsageARB.StaticDraw);
		eboSize = size;
	}
	
	public int GetVertexBufferSize()
	{
		return vboSize;
	}
	public int GetIndexBufferSize()
	{
		return eboSize;
	}

	public void SetVertexAttrib(uint index, int size, VertexAttribType vertexAttribType, int stride, int offset, bool normalized = false)
	{
		VertexAttribPointerType vertexAttribPointerType = vertexAttribType switch
		{
			VertexAttribType.Byte => VertexAttribPointerType.Byte,
			VertexAttribType.Double => VertexAttribPointerType.Double,
			VertexAttribType.Float => VertexAttribPointerType.Float,
			VertexAttribType.Int => VertexAttribPointerType.Int,
			VertexAttribType.UnsignedByte => VertexAttribPointerType.UnsignedByte,
			_ => 0
		};

		GL.VertexAttribPointer(index, size, vertexAttribPointerType, normalized, stride, offset);
		GL.EnableVertexAttribArray(index);
	}
	
	public void Render(int indices, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0, int vertexOffset = 0)
	{
		GL.BindVertexArray(vao);
		GL.DrawElements(GetPrimitiveType(renderMode), indices, DrawElementsType.UnsignedInt, 0);
		GL.BindVertexArray(VertexArrayHandle.Zero);
	}

	private PrimitiveType GetPrimitiveType(RenderMode renderMode)
	{
		return renderMode switch
		{
			RenderMode.Line => PrimitiveType.LineStrip,
			RenderMode.Lines => PrimitiveType.Lines,
			RenderMode.LineLoop => PrimitiveType.LineLoop,
			RenderMode.Point => PrimitiveType.Points,
			RenderMode.Triangle => PrimitiveType.Triangles,
			RenderMode.TriangleFan => PrimitiveType.TriangleFan,
			_ => PrimitiveType.Triangles
		};
	}
	
	public void Dispose()
	{
		GL.DeleteVertexArray(vao);
		GL.DeleteBuffer(vbo);
		GL.DeleteBuffer(ebo);
	}
}
