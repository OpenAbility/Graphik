using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLRenderTexture : IRenderTexture
{
	private FramebufferHandle fbo;
	private RenderbufferHandle rbo;
	private TextureHandle colourHandle;
	private TextureHandle depthHandle;

	private int width;
	private int height;
	private bool colour;
	private bool depth;

	public GLRenderTexture()
	{
		
	}

	public unsafe void Build(int _width, int _height, bool _colour, bool _depth)
	{
		width = _width;
		height = _height;
		colour = _colour;
		depth = _depth;
		
		fbo = GL.GenFramebuffer();
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

		if(depth) {
			depthHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2d, depthHandle);
			GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, null);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
			
			float[] borderColor =
			{
				1.0f, 1.0f, 1.0f, 1.0f
			};
		
			fixed(float* border = borderColor) 
				GL.TexParameterfv(TextureTarget.Texture2d, TextureParameterName.TextureBorderColor, border);
			
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2d, depthHandle, 0);
		}
		else
		{
			rbo = GL.GenRenderbuffer();
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth32fStencil8, width, height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
		}
		
		if(colour) {
			colourHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2d, colourHandle);

			GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, null);

			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2d, colourHandle, 0);
			
			GL.DrawBuffers(new DrawBufferMode[]
			{
				DrawBufferMode.ColorAttachment0,
			});
		}
		else
		{
			GL.DrawBuffer(DrawBufferMode.None);
			GL.ReadBuffer(ReadBufferMode.None);
		}


		if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete)
			throw new GLException( GLLocation.General, "Incomplete framebuffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
		
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
	}

	public void Target()
	{
		GL.Viewport(0, 0, width, height);
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
		
	}
	
	public void Bind(RenderTextureComponent component, int index = 0)
	{
		if (index > 31)
			throw new IndexOutOfRangeException("Max texture index is 32!");

		switch (component)
		{
			case RenderTextureComponent.Colour when colour:
				GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + index));
				GL.BindTexture(TextureTarget.Texture2d, colourHandle);
				break;
			case RenderTextureComponent.Depth when depth:
				GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + index));
				GL.BindTexture(TextureTarget.Texture2d, depthHandle);
				break;
			default:
				throw new ArgumentException("Component is invalid value!", nameof(component));
		}

	}
	public void Bind(int index = 0)
	{
		Bind(RenderTextureComponent.Colour, index);
	}
	public void Dispose()
	{
		GL.DeleteTexture(depthHandle);
		GL.DeleteTexture(colourHandle);
		GL.DeleteFramebuffer(fbo);
		GL.DeleteRenderbuffer(rbo);
	}
	
	public void CopyFrom(ITexture other)
	{
		// We can't copy.
	}
	
	public uint GetHandle()
	{
		return (uint)colourHandle.Handle;
	}
	public void SetFiltering(TextureFiltering filtering)
	{
		if (colour)
		{
			GL.BindTexture(TextureTarget.Texture2d, colourHandle);
			if (filtering == TextureFiltering.Linear)
			{
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			} else if (filtering == TextureFiltering.Nearest)
			{
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}
		}
		
		if (depth)
		{			
			GL.BindTexture(TextureTarget.Texture2d, depthHandle);
			if (filtering == TextureFiltering.Linear)
			{
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			} else if (filtering == TextureFiltering.Nearest)
			{
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}
		}
	}
}
