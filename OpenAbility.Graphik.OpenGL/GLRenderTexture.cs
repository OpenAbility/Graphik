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

	public GLRenderTexture()
	{
		
	}

	public void Build(int width, int height, bool colour)
	{
		this.width = width;
		this.height = height;
		
		this.colour = colour;
		if (colour)
			CreateWithColour();
		else
		
			CreateWithoutColour();
		

		if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete)
			throw new GLException( GLLocation.General, "Incomplete framebuffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
	}

	private unsafe void CreateWithoutColour()
	{
		fbo = GL.GenFramebuffer();
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
		
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
		GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2d, depthHandle, 0);
		GL.DrawBuffer(DrawBufferMode.None);
		GL.ReadBuffer(ReadBufferMode.None);
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
	}

	private unsafe void CreateWithColour()
	{
		fbo = GL.GenFramebuffer();
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

		colourHandle = GL.GenTexture();
		GL.BindTexture(TextureTarget.Texture2d, colourHandle);
		
		GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.Float, null);
		
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);

		GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, colourHandle, 0);
		rbo = GL.GenRenderbuffer();
		GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
		GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, width, height);
		GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
		
		GL.DrawBuffers(new DrawBufferMode[]
		{
			DrawBufferMode.ColorAttachment0
		});

		if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete)
			throw new GLException( GLLocation.General, "Incomplete framebuffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
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
			case RenderTextureComponent.Depth:
				GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + index));
				GL.BindTexture(TextureTarget.Texture2d, depthHandle);
				break;
			default:
				throw new ArgumentException("Component is invalid value!", nameof(component));
		}

	}
	public void Dispose()
	{
		GL.DeleteTexture(depthHandle);
		GL.DeleteTexture(colourHandle);
		GL.DeleteFramebuffer(fbo);
		GL.DeleteRenderbuffer(rbo);
	}
}
