using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLTexture : ITexture
{
	private TextureHandle handle;
	public GLTexture()
	{
		handle = GL.GenTexture();
	}
}
