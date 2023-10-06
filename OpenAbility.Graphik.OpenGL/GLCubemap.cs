using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLCubemap : ICubemapTexture
{
	private readonly TextureHandle handle;

	public GLCubemap()
	{
		handle = GL.GenTexture();
		PrepareModification();
		
		GL.TexParameteri(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		GL.TexParameteri(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
		GL.TexParameteri(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
	}

	public void PrepareModification()
	{
		GL.BindTexture(TextureTarget.TextureCubeMap, handle);
	}
	
	public unsafe void SetFaceData<T>(CubemapFace face, TextureFormat format, T* imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged
	{

		TextureTarget target = face switch
		{
			CubemapFace.PositiveX => TextureTarget.TextureCubeMapPositiveX,
			CubemapFace.NegativeX => TextureTarget.TextureCubeMapNegativeX,

			CubemapFace.PositiveY => TextureTarget.TextureCubeMapPositiveY,
			CubemapFace.NegativeY => TextureTarget.TextureCubeMapNegativeY,

			CubemapFace.PositiveZ => TextureTarget.TextureCubeMapPositiveZ,
			CubemapFace.NegativeZ => TextureTarget.TextureCubeMapNegativeZ,
			_ => 0
		};
		
		GL.TexImage2D(target, mipmapLevel, GLTexture.GetInternalFormat(format), width, height, 0, 
			GLTexture.GetPixelFormat(format), GLTexture.GetPixelType(format), imageData);
	}
	
	public void Bind(int slot = 0)
	{
		GL.BindTexture(TextureTarget.TextureCubeMap, handle);
		GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + slot));
	}
	
	public void Dispose()
	{
		GL.DeleteTexture(handle);
	}
	
	public uint GetHandle()
	{
		return (uint)handle.Handle;
	}
	
	public void SetName(string name)
	{
		GLAPI.SetLabel(ObjectIdentifier.Texture, handle.Handle, name);
	}
}
