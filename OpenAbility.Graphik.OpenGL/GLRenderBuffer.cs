using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

public class GLRenderBuffer : IRenderBuffer
{
	private readonly FramebufferHandle handle = GL.CreateFramebuffer();
	private readonly Dictionary<FramebufferAttachment, RenderbufferHandle> renderBuffers = new();

	private TextureHandle GetHandle(ITexture texture)
	{
		return new TextureHandle((int)texture.GetHandle());
	}

	private void PopRenderbuffer(FramebufferAttachment attachment)
	{
		// I have no idea what happens if we have a render buffer AND texture
		// present, so I'll clean up any remaining render buffers.
		if (renderBuffers.Remove(attachment, out RenderbufferHandle handle))
		{
			GL.DeleteRenderbuffer(handle);
		}
	}

	private void AttachRenderBuffer(FramebufferAttachment attachment, InternalFormat format, int width, int height, int msaa)
	{
		if(renderBuffers.ContainsKey(attachment))
			return;
		RenderbufferHandle renderbufferHandle = GL.CreateRenderbuffer();
		if(msaa > 0)
			GL.NamedRenderbufferStorageMultisample(renderbufferHandle, msaa, format, width, height);
		else
			GL.NamedRenderbufferStorage(renderbufferHandle, format, width, height);
		GL.NamedFramebufferRenderbuffer(handle, attachment, RenderbufferTarget.Renderbuffer, renderbufferHandle);
	}

	public void BindColorTexture(int id, ITexture texture, int mip = 0)
	{
		FramebufferAttachment framebufferAttachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + id);
		PopRenderbuffer(framebufferAttachment);
		GL.NamedFramebufferTexture(handle, framebufferAttachment, GetHandle(texture), mip);
	}
	
	public void BindDepthTexture(ITexture texture, int mip = 0)
	{
		PopRenderbuffer(FramebufferAttachment.DepthAttachment);
		GL.NamedFramebufferTexture(handle, FramebufferAttachment.DepthAttachment, GetHandle(texture), mip);
	}
	
	public void BindStencilTexture(ITexture texture, int mip = 0)
	{
		PopRenderbuffer(FramebufferAttachment.StencilAttachment);
		GL.NamedFramebufferTexture(handle, FramebufferAttachment.StencilAttachment, GetHandle(texture), mip);
	}
	
	public void BindDepthStencilTexture(ITexture texture, int mip = 0)
	{
		PopRenderbuffer(FramebufferAttachment.DepthStencilAttachment);
		GL.NamedFramebufferTexture(handle, FramebufferAttachment.DepthStencilAttachment, GetHandle(texture), mip);
	}

	public void BindColorBuffer(int id, TextureFormat format, int width, int height, int msaaLevels = 0)
	{
		FramebufferAttachment framebufferAttachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + id);
		AttachRenderBuffer(framebufferAttachment, GLTexture.GetInternalFormat(format), width, height, msaaLevels);
	}
	
	public void BindDepthBuffer(int width, int height)
	{
		AttachRenderBuffer(FramebufferAttachment.DepthAttachment, InternalFormat.DepthComponent24, width, height, 0);
	}
	
	public void BindStencilBuffer(int width, int height)
	{
		AttachRenderBuffer(FramebufferAttachment.StencilAttachment, InternalFormat.Depth24Stencil8, width, height, 0);
	}
	
	public void BindDepthStencilBuffer(int width, int height)
	{
		AttachRenderBuffer(FramebufferAttachment.DepthStencilAttachment, InternalFormat.Depth24Stencil8, width, height, 0);
	}

	public void MarkDraw(params int[] buffers)
	{
		if (buffers.Length == 0)
		{
			GL.NamedFramebufferDrawBuffer(handle, ColorBuffer.None);
			return;
		}
		ColorBuffer[] bufferArray = new ColorBuffer[buffers.Length];
		for (int i = 0; i < buffers.Length; i++)
		{
			if (buffers[i] == -1)
				bufferArray[i] = ColorBuffer.None;
			else
				bufferArray[i] = (ColorBuffer)((int)ColorBuffer.ColorAttachment0 + buffers[i]);
		}
		GL.NamedFramebufferDrawBuffers(handle, bufferArray);
	}
	
	public void SetName(string name)
	{
		foreach (var renderBuffer in renderBuffers)
		{
			GLAPI.SetLabel(ObjectIdentifier.Renderbuffer, 
				renderBuffer.Value.Handle, name + "." + renderBuffer.Key);
		}
		
		GLAPI.SetLabel(ObjectIdentifier.Framebuffer, handle.Handle, name + ".FBO");
	}

	public void Target(int width, int height)
	{
		GL.Viewport(0, 0, width, height);
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
	}
	
	/*
	public void MarkRead(int buffer)
	{
		if (buffer < 0 || buffer >= 32)
			throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer has to be in range 0-31");
		GL.NamedFramebufferReadBuffer(handle, (ColorBuffer)((int)ColorBuffer.ColorAttachment0 + buffer));
	}
	*/

	public void Dispose()
	{
		// Destroy it all
		GL.DeleteRenderbuffers(renderBuffers.Values.ToArray());
		GL.DeleteFramebuffer(handle);
	}
	public void Validate()
	{
		FramebufferStatus status = GL.CheckNamedFramebufferStatus(handle, FramebufferTarget.Framebuffer);
		if(status != FramebufferStatus.FramebufferComplete)
			CallbackHandler.ErrorCallback?.Invoke("[FBO " + handle.Handle + "]", "Framebuffer Incomplete: " + status);
	}
}
