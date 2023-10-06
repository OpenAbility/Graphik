using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLShaderBuffer : IShaderBuffer
{

	private readonly BufferHandle buffer = GL.GenBuffer();

	public void PushData<T>(Span<T> data) where T : unmanaged
	{
		GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, buffer);
		GL.BufferData(BufferTargetARB.ShaderStorageBuffer, (ReadOnlySpan<T>)data, BufferUsageARB.DynamicDraw);
	}

	public unsafe void PushData<T>(T* data, int size) where T : unmanaged
	{
		GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, buffer);
		GL.BufferData(BufferTargetARB.ShaderStorageBuffer, new IntPtr(size), data, BufferUsageARB.DynamicDraw);
	}

	public unsafe void PushData(void* data, int size)
	{
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
