using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLCubemap : ICubemapTexture
{
	private readonly TextureHandle handle;

	public GLCubemap()
	{
		handle = GL.CreateTexture(TextureTarget.TextureCubeMapArray);
		PrepareModification();
		
		GL.TextureParameteri(handle, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
		GL.TextureParameteri(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TextureParameteri(handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		GL.TextureParameteri(handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
		GL.TextureParameteri(handle, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
	}

	public void PrepareModification()
	{
		//GL.BindTexture(TextureTarget.TextureCubeMap, handle);
	}

	private bool allocated = false;
	public unsafe void SetFaceData<T>(CubemapFace face, TextureFormat format, T* imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged
	{

		int faceID = face switch
		{
			CubemapFace.PositiveX => 0,
			CubemapFace.NegativeX => 1,

			CubemapFace.PositiveY => 2,
			CubemapFace.NegativeY => 3,

			CubemapFace.PositiveZ => 4,
			CubemapFace.NegativeZ => 5,
			_ => 0
		};

		if (!allocated)
		{
			GL.TextureStorage3D(handle, mipmapLevel + 1, GLTexture.GetSizedInternalFormat(format), width, height, 6);
			allocated = true;
		}
		
		GL.TextureSubImage3D(handle, mipmapLevel, 0, 0, faceID, width, height, 0, GLTexture.GetPixelFormat(format), GLTexture.GetPixelType(format), imageData);
	}
	
	public void Bind(int slot = 0)
	{
		GL.BindTextureUnit((uint)slot, handle);
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
