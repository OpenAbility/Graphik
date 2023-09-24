using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLShaderBuffer : IShaderBuffer
{

	private BufferHandle buffer = GL.GenBuffer();

	public unsafe void PushData(void* data, int size)
	{
		
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
