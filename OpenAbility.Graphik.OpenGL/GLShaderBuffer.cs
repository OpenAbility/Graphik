using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLShaderBuffer : IShaderBuffer
{
	private long pushedSize;

	private readonly BufferHandle buffer = GL.GenBuffer();

	public long GetDataSize()
	{
		return pushedSize;
	}
	
	public unsafe void PushData(void* data, int size)
	{
		pushedSize = size;
		
		GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, buffer);
		GL.BufferData(BufferTargetARB.ShaderStorageBuffer, new IntPtr(size), data, BufferUsageARB.DynamicDraw);
	}

	public void Bind(uint id)
	{
		GL.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, id, buffer);
	}
	public void Dispose()
	{
		GL.DeleteBuffer(buffer);
	}
}
