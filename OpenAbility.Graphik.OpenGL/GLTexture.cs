using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLTexture : ITexture
{
	private TextureHandle handle;
	public GLTexture()
	{
		handle = GL.GenTexture();
		PrepareModifications();
		
		// TODO: Add functions to set these variables
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
	}
	public void SetData<T>(TextureFormat format, T[] imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged
	{

		GL.TexImage2D(TextureTarget.Texture2d, mipmapLevel, GetInternalFormat(format), width, height, 0, GetPixelFormat(format), GetPixelType(format), imageData);
		
	}

	private static InternalFormat GetInternalFormat(TextureFormat textureFormat)
	{
		return textureFormat switch
		{
			TextureFormat.R8 => InternalFormat.R8,
			TextureFormat.R32 => InternalFormat.R32i,
			TextureFormat.Rf => InternalFormat.R32f,
			TextureFormat.Rgb8 => InternalFormat.Rgb8,
			TextureFormat.Rgb32 => InternalFormat.Rgb32i,
			TextureFormat.Rgbf => InternalFormat.Rgb32f,
			TextureFormat.Rgba8 => InternalFormat.Rgba8,
			TextureFormat.Rgba32 => InternalFormat.Rgba32i,
			TextureFormat.Rgbaf => InternalFormat.Rgba32f,
			_ => 0
		};
	}

	private PixelFormat GetPixelFormat(TextureFormat textureFormat)
	{
		return textureFormat switch
		{
			TextureFormat.R8 => PixelFormat.Red,
			TextureFormat.R32 => PixelFormat.Red,
			TextureFormat.Rf => PixelFormat.Red,
			TextureFormat.Rgb8 => PixelFormat.Rgb,
			TextureFormat.Rgb32 => PixelFormat.Rgb,
			TextureFormat.Rgbf => PixelFormat.Rgb,
			TextureFormat.Rgba8 => PixelFormat.Rgba,
			TextureFormat.Rgba32 => PixelFormat.Rgba,
			TextureFormat.Rgbaf => PixelFormat.Rgba,
			_ => 0
		};
	}
	
	private PixelType GetPixelType(TextureFormat textureFormat)
	{
		return textureFormat switch
		{
			TextureFormat.R8 => PixelType.UnsignedByte,
			TextureFormat.R32 => PixelType.UnsignedInt,
			TextureFormat.Rf => PixelType.Float,
			TextureFormat.Rgb8 => PixelType.UnsignedByte,
			TextureFormat.Rgb32 => PixelType.UnsignedInt,
			TextureFormat.Rgbf => PixelType.Float,
			TextureFormat.Rgba8 => PixelType.UnsignedByte,
			TextureFormat.Rgba32 => PixelType.UnsignedInt,
			TextureFormat.Rgbaf => PixelType.Float,
			_ => 0
		};
	}

	public void PrepareModifications()
	{
		GL.BindTexture(TextureTarget.Texture2d, handle);
	}

	public void Bind(int index = 0)
	{
		if (index > 31)
			throw new IndexOutOfRangeException("Max texture index is 32!");
		GL.BindTexture(TextureTarget.Texture2d, handle);
		GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + index));
	}
}
