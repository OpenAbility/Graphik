using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLTexture : ITexture2D
{
	private readonly TextureHandle handle;
	private InternalFormat internalFormat;

	public GLTexture(uint handle)
	{
		this.handle = new TextureHandle()
		{
			Handle = (int)handle
		};
	}
	
	public GLTexture()
	{
		handle = GL.CreateTexture(TextureTarget.Texture2d);
        
		GL.TextureParameteri(handle, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
		GL.TextureParameteri(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TextureParameteri(handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.TextureParameteri(handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
	}
	public unsafe void SetData<T>(TextureFormat format, T[] imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged
	{
		fixed(T* p = imageData)
			SetData(format, p, width, height, mipmapLevel);
	}

	public unsafe void AllocateImage(TextureFormat format, int width, int height, int levels = 1)
	{
		GL.TextureStorage2D(handle, levels, GetSizedInternalFormat(format), width, height);
		internalFormat = GetInternalFormat(format);
	}

	public unsafe void SetData<T>(TextureFormat format, T* imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged
	{
		GL.TextureSubImage2D(handle, mipmapLevel, 0, 0, width, height, GetPixelFormat(format), GetPixelType(format), imageData);
	}
	public void GenerateMipMaps(int depth)
	{
		GL.TextureParameteri(handle, TextureParameterName.TextureMaxLevel, depth);
		GL.GenerateTextureMipmap(handle);
	}

	public static InternalFormat GetInternalFormat(TextureFormat textureFormat)
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
			TextureFormat.Bgr8 => InternalFormat.Rgb8,
			TextureFormat.Depth => InternalFormat.DepthComponent32f,
			TextureFormat.Stencil => InternalFormat.StencilIndex8,
			TextureFormat.DepthStencil => InternalFormat.Depth32fStencil8,
			_ => 0
		};
	}
	
	public static SizedInternalFormat GetSizedInternalFormat(TextureFormat textureFormat)
	{
		return textureFormat switch
		{
			TextureFormat.R8 => SizedInternalFormat.R8,
			TextureFormat.R32 => SizedInternalFormat.R32i,
			TextureFormat.Rf => SizedInternalFormat.R32f,
			TextureFormat.Rgb8 => SizedInternalFormat.Rgb8,
			TextureFormat.Rgb32 => SizedInternalFormat.Rgb32i,
			TextureFormat.Rgbf => SizedInternalFormat.Rgb32f,
			TextureFormat.Rgba8 => SizedInternalFormat.Rgba8,
			TextureFormat.Rgba32 => SizedInternalFormat.Rgba32i,
			TextureFormat.Rgbaf => SizedInternalFormat.Rgba32f,
			TextureFormat.Bgr8 => SizedInternalFormat.Rgb8,
			TextureFormat.Depth => SizedInternalFormat.DepthComponent24,
			TextureFormat.Stencil => SizedInternalFormat.StencilIndex8,
			TextureFormat.DepthStencil => SizedInternalFormat.Depth24Stencil8,
			_ => 0
		};
	}

	public static PixelFormat GetPixelFormat(TextureFormat textureFormat)
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
			TextureFormat.Bgr8 => PixelFormat.Bgr,
			TextureFormat.Depth => PixelFormat.DepthComponent,
			TextureFormat.Stencil => PixelFormat.StencilIndex,
			TextureFormat.DepthStencil => PixelFormat.DepthStencil,
			_ => 0
		};
	}
	
	public static PixelType GetPixelType(TextureFormat textureFormat)
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
			TextureFormat.Bgr8 => PixelType.UnsignedByte,
			TextureFormat.Depth => PixelType.UnsignedInt248, // Is this correct? Stencil is Depth+Stencil but this?
			TextureFormat.Stencil => PixelType.UnsignedInt248,
			TextureFormat.DepthStencil => PixelType.UnsignedInt248,
			_ => 0
		};
	}

	public void PrepareModifications()
	{
		//GL.BindTexture(TextureTarget.Texture2d, handle);
	}

	public void Bind(int index = 0)
	{
		GL.BindTextureUnit((uint)index, handle);
	}
	
	public void Dispose()
	{
		GL.DeleteTexture(handle);
	}

	public void CopyFrom(ITexture other)
	{
		GL.BindTexture(TextureTarget.Texture2d,  new TextureHandle((int)other.GetHandle()));

		int width = 0;
		int height = 0;
		
		GL.GetTexParameteri(TextureTarget.Texture2d, GetTextureParameter.TextureWidth, ref width);
		GL.GetTexParameteri(TextureTarget.Texture2d, GetTextureParameter.TextureHeight, ref height);
		
		GL.CopyImageSubData(other.GetHandle(), CopyImageSubDataTarget.Texture2d, 0, 0, 0, 0, GetHandle(), CopyImageSubDataTarget.Texture2d,
			0, 0, 0, 0, width, height, 0);
	}

	public uint GetHandle()
	{
		return (uint)handle.Handle;
	}
	public void SetFiltering(TextureFiltering filtering)
	{
		if (filtering == TextureFiltering.Linear)
		{
			GL.TextureParameteri(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TextureParameteri(handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		} else if (filtering == TextureFiltering.Nearest)
		{
			GL.TextureParameteri(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TextureParameteri(handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
		}
		else if (filtering == TextureFiltering.Trilinear)
		{
			GL.TextureParameteri(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			GL.TextureParameteri(handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		}
	}
	public void SetRepetition(TextureRepetition repetition)
	{
		int repeat = repetition switch
		{
			TextureRepetition.Repeat => (int)TextureWrapMode.Repeat,
			TextureRepetition.ClampToBorder => (int)TextureWrapMode.ClampToBorder,
			TextureRepetition.ClampToEdge => (int)TextureWrapMode.ClampToEdge,
			_ => 0
		};

        GL.TextureParameteri(handle, TextureParameterName.TextureWrapS, repeat);
        GL.TextureParameteri(handle, TextureParameterName.TextureWrapT, repeat);
		
	}
	public void SetName(string name)
	{
		GLAPI.SetLabel(ObjectIdentifier.Texture, handle.Handle, name);
	}
	
	public void SetDepthStencilMode(DepthStencilMode depthStencilMode)
	{
		DepthStencilTextureMode mode = depthStencilMode switch
		{
			DepthStencilMode.Depth => DepthStencilTextureMode.DepthComponent,
			DepthStencilMode.Stencil => DepthStencilTextureMode.StencilIndex,
			_ => 0
		};
		GL.TextureParameteri(handle, TextureParameterName.DepthStencilTextureMode, (int)mode);
	}
	public ulong GetPointer()
	{
		if (!Graphik.Supports(SupportCap.TexturePointer))
			throw new Exception("TexturePointer functionality is unsupported!");
		return GL.ARB.GetTextureHandleARB(handle);
	}
	public void MakeResident()
	{
		if (!Graphik.Supports(SupportCap.TexturePointer))
			throw new Exception("TexturePointer functionality is unsupported!");
		GL.ARB.MakeTextureHandleResidentARB(GetPointer());
	}
	public void FreeResidency()
	{
		if (!Graphik.Supports(SupportCap.TexturePointer))
			throw new Exception("TexturePointer functionality is unsupported!");
		GL.ARB.MakeTextureHandleNonResidentARB(GetPointer());
	}

	private static readonly float[] borderBuffer = new float[4];
	public unsafe void SetBorder(float r, float g, float b, float a = 1)
	{
		borderBuffer[0] = r;
		borderBuffer[1] = g;
		borderBuffer[2] = b;
		borderBuffer[3] = a;
		fixed(float* ptr = borderBuffer)
			GL.TextureParameterfv(handle, TextureParameterName.TextureBorderColor, ptr);
	}
	public unsafe void GetData(void* buffer, int bufferSize)
	{
		GL.GetTextureImage(handle, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bufferSize, buffer);
	}
}
