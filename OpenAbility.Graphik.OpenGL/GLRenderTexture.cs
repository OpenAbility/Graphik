using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLRenderTexture : IRenderTexture
{
	
	public static GLRenderTexture? Bound;
	
	private FramebufferHandle fbo;
	private RenderbufferHandle rbo;
	private TextureHandle colourHandle;
	private TextureHandle depthHandle;
	private TextureHandle normalHandle;

	private int width;
	private int height;
	private bool colour;
	private bool depth;
	private bool normal;

	public GLRenderTexture()
	{
		
	}

	public unsafe void Build(int _width, int _height, bool _colour, bool _depth, bool _normal)
	{
		width = _width;
		height = _height;
		colour = _colour;
		depth = _depth;
		normal = _normal;
		
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

		List<DrawBufferMode> drawBufferModes = new List<DrawBufferMode>();
		
		if(colour) {
			colourHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2d, colourHandle);

			GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, null);

			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2d, colourHandle, 0);
			
			drawBufferModes.Add(DrawBufferMode.ColorAttachment0);
		}
		if (normal)
		{
			normalHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2d, normalHandle);

			GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb32f, width, height, 0, PixelFormat.Rgb, PixelType.Float, null);

			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2d, normalHandle, 0);
			
			drawBufferModes.Add(DrawBufferMode.ColorAttachment1);
		}

		if (drawBufferModes.Count != 0)
		{
			GL.DrawBuffers(drawBufferModes.ToArray());
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
		Bound = this;
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
			case RenderTextureComponent.Normal when normal:
				GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + index));
				GL.BindTexture(TextureTarget.Texture2d, normalHandle);
				break;
			default:
				throw new ArgumentException("Component is invalid value!", nameof(component));
		}

	}

	public void Bind(int index = 0)
	{
		Bind(RenderTextureComponent.Colour, index);
	}
	
	public void PrepareModifications()
	{
		
	}
	public void Dispose()
	{
		GL.DeleteTexture(depthHandle);
		GL.DeleteTexture(colourHandle);
		GL.DeleteTexture(normalHandle);
		GL.DeleteFramebuffer(fbo);
		GL.DeleteRenderbuffer(rbo);
	}

	public void CopyChannelFrom(RenderTextureComponent sourceChannel, RenderTextureComponent targetChannel, IRenderTexture other)
	{

		GLRenderTexture source = (GLRenderTexture)other;
		
		GL.BindTexture(TextureTarget.Texture2d, source.GetHandle(sourceChannel));

		int width = 0;
		int height = 0;
		
		GL.GetTexParameteri(TextureTarget.Texture2d, GetTextureParameter.TextureWidth, ref width);
		GL.GetTexParameteri(TextureTarget.Texture2d, GetTextureParameter.TextureHeight, ref height);
		
		GL.CopyImageSubData((uint)source.GetHandle(sourceChannel).Handle, 
			CopyImageSubDataTarget.Texture2d, 0, 0, 0, 0, 
			(uint)GetHandle(targetChannel).Handle, CopyImageSubDataTarget.Texture2d, 0, 0, 0, 0, width, height, 0);
	}

	public void CopyChannelFrom(RenderTextureComponent component, ITexture other)
	{
		if (other is IRenderTexture renderTexture)
		{
			CopyChannelFrom(component, component, renderTexture);
		}
		
		
		GL.BindTexture(TextureTarget.Texture2d, new TextureHandle((int)other.GetHandle()));

		int width = 0;
		int height = 0;
		
		GL.GetTexParameteri(TextureTarget.Texture2d, GetTextureParameter.TextureWidth, ref width);
		GL.GetTexParameteri(TextureTarget.Texture2d, GetTextureParameter.TextureHeight, ref height);
		
		GL.CopyImageSubData(other.GetHandle(), 
			CopyImageSubDataTarget.Texture2d, 0, 0, 0, 0, 
			(uint)GetHandle(component).Handle, CopyImageSubDataTarget.Texture2d, 0, 0, 0, 0, width, height, 0);
		
	}
	
	public void CopyFrom(ITexture other)
	{
		CopyChannelFrom(RenderTextureComponent.Colour, other);
	}
	
	public uint GetHandle()
	{
		return (uint)colourHandle.Handle;
	}

	internal TextureHandle GetHandle(RenderTextureComponent component)
	{
		return component switch
		{
			RenderTextureComponent.Colour => colourHandle,
			RenderTextureComponent.Depth => depthHandle,
			RenderTextureComponent.Normal => normalHandle,
			_ => colourHandle
		};
	}
	
	public void SetFiltering(TextureFiltering filtering)
	{
		
		int minFilter = filtering switch
		{
			TextureFiltering.Linear => (int)TextureMinFilter.Linear,
			TextureFiltering.Nearest => (int)TextureMinFilter.Nearest,
			TextureFiltering.Trilinear => (int)TextureMinFilter.LinearMipmapLinear,
			_ => 0
		};
		
		int magFilter = filtering switch
		{
			TextureFiltering.Linear => (int)TextureMagFilter.Linear,
			TextureFiltering.Nearest => (int)TextureMagFilter.Nearest,
			TextureFiltering.Trilinear => (int)TextureMagFilter.Linear,
			_ => 0
		};
		
		if (colour)
		{
			GL.BindTexture(TextureTarget.Texture2d, colourHandle);
			
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, magFilter);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, minFilter);
		}
		
		if (depth)
		{			
			GL.BindTexture(TextureTarget.Texture2d, depthHandle);
			
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, magFilter);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, minFilter);
		}
		
		if (normal)
		{			
			GL.BindTexture(TextureTarget.Texture2d, normalHandle);
			
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, magFilter);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, minFilter);
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
		
		if (colour)
		{
			GL.BindTexture(TextureTarget.Texture2d, colourHandle);
			
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, repeat);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, repeat);
		}
		
		if (depth)
		{			
			GL.BindTexture(TextureTarget.Texture2d, depthHandle);
			
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, repeat);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, repeat);
		}
		
		if (normal)
		{			
			GL.BindTexture(TextureTarget.Texture2d, normalHandle);
			
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, repeat);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, repeat);
		}
	}
	public void SetName(string name)
	{
		GLAPI.SetLabel(ObjectIdentifier.Texture, colourHandle.Handle, name + ".COLOUR");
		GLAPI.SetLabel(ObjectIdentifier.Texture, depthHandle.Handle, name + ".DEPTH");
		GLAPI.SetLabel(ObjectIdentifier.Renderbuffer, rbo.Handle, name + ".RBO");
		GLAPI.SetLabel(ObjectIdentifier.Framebuffer, fbo.Handle, name + ".FBO");
	}
}
