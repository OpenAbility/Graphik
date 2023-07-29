using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLMesh : IMesh
{
	private readonly BufferHandle vbo;
	private readonly BufferHandle ebo;
	private readonly VertexArrayHandle vao;
	private DrawElementsType indexType = DrawElementsType.UnsignedInt;

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
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
	}

	public void SetIndexType(IndexType type)
	{
		indexType = type switch
		{
			IndexType.UnsignedInt => DrawElementsType.UnsignedInt,
			IndexType.UnsignedByte => DrawElementsType.UnsignedByte,
			IndexType.UnsignedShort => DrawElementsType.UnsignedShort,
			_ => 0
		};
	}

	public unsafe void SetVertexData<T>(T[] data, bool realloc) where T : unmanaged
	{
		fixed (T* ptr = data)
		{
			SetVertexData(new IntPtr(ptr), data.Length * sizeof(T), realloc);
		}
		
		
	}
	public void SetVertexData(IntPtr data, int size, bool realloc = true)
	{
		if (size != vboSize && realloc)
		{
			AllocateVertexData(size);
		} else if (size > vboSize)
		{
			AllocateVertexData(size);
		}
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
		GL.BufferSubData(BufferTargetARB.ArrayBuffer, IntPtr.Zero, size, data);
	}
	
	public unsafe void SetIndices<T>(T[] indices, bool realloc = true) where T : unmanaged
	{
		fixed (T* ptr = indices)
		{
			SetIndices(new IntPtr(ptr), indices.Length * sizeof(T), realloc);
		}
	}
	
	public void SetIndices(IntPtr data, int size, bool realloc = true)
	{
		if (size != eboSize && realloc)
		{
			AllocateIndexData(size);
		} else if (size > eboSize)
		{
			AllocateIndexData(size);
		}
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
		GL.BufferSubData(BufferTargetARB.ElementArrayBuffer, IntPtr.Zero, size, data);
	}
	

	public void AllocateVertexData(int size, bool quickWrite = false)
	{
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
		GL.BufferData(BufferTargetARB.ArrayBuffer, size, IntPtr.Zero, quickWrite ? BufferUsageARB.DynamicDraw : BufferUsageARB.StaticDraw);
		vboSize = size;
	}

	public void AllocateIndexData(int size, bool quickWrite = false)
	{
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
		GL.BufferData(BufferTargetARB.ElementArrayBuffer, size, IntPtr.Zero,  quickWrite ? BufferUsageARB.DynamicDraw : BufferUsageARB.StaticDraw);
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
	
	public void Render(int indices, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0)
	{
		GL.BindVertexArray(vao);
		GL.DrawElements(GetPrimitiveType(renderMode), indices, indexType, indexOffset);
		GL.BindVertexArray(VertexArrayHandle.Zero);
	}
	
	public void Render(int indices, int vertexOffset, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0)
	{
		GL.BindVertexArray(vao);
		GL.DrawElementsBaseVertex(GetPrimitiveType(renderMode), indices, indexType, indexOffset, vertexOffset);
		GL.BindVertexArray(VertexArrayHandle.Zero);
	}

	public void RenderInstanced(int indices, int instances, RenderMode renderMode = RenderMode.Triangle, int indexOffset = 0)
	{
		GL.BindVertexArray(vao);
		GL.DrawElementsInstanced(GetPrimitiveType(renderMode), indices, indexType, indexOffset, instances);
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

	public void SetName(string name)
	{
		GLAPI.SetLabel(ObjectIdentifier.VertexArray, vao.Handle, name + ".VAO");
		GLAPI.SetLabel(ObjectIdentifier.Buffer, vbo.Handle, name + ".VBO");
		GLAPI.SetLabel(ObjectIdentifier.Buffer, ebo.Handle, name + ".EBO");
	}
}
