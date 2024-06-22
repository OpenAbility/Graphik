namespace OpenAbility.Graphik;

/// <summary>
/// Render target & replacement for <see cref="IRenderTexture"/>
/// </summary>
public interface IRenderBuffer : IDisposable
{
	/// <summary>
	/// Target the render buffer
	/// </summary>
	/// <param name="width">The render width</param>
	/// <param name="height">The render height</param>
	public void Target(int width, int height);

	/// <summary>
	/// Bind a texture to use for colour storage
	/// </summary>
	/// <param name="id">The colour buffer id</param>
	/// <param name="texture">The texture to use</param>
	/// <param name="mip">The mip level to bind(?)</param>
	public void BindColorTexture(int id, ITexture texture, int mip = 0);
	/// <summary>
	/// Bind a texture to use for depth storage
	/// </summary>
	/// <param name="texture">The texture to use</param>
	/// <param name="mip">The mip level to bind(?)</param>
	public void BindDepthTexture(ITexture texture, int mip = 0);
	/// <summary>
	/// Bind a texture to use for stencil storage
	/// </summary>
	/// <param name="texture">The texture to use</param>
	/// <param name="mip">The mip level to bind(?)</param>
	public void BindStencilTexture(ITexture texture, int mip = 0);
	/// <summary>
	/// Bind a texture to use for depth & stencil storage
	/// </summary>
	/// <param name="texture">The texture to use</param>
	/// <param name="mip">The mip level to bind(?)</param>
	public void BindDepthStencilTexture(ITexture texture, int mip = 0);



	public void BindDepthBuffer(int width, int height);
	public void BindStencilBuffer(int width, int height);
	public void BindColorBuffer(int id, TextureFormat format, int width, int height, int msaaLevels = 0);
	public void BindDepthStencilBuffer(int width, int height);


	

	/// <summary>
	/// Mark the color buffers to use when drawing
	/// </summary>
	/// <param name="buffers">The buffers to use, "-1" would be null/GL_NONE/whatever</param>
	/// <remarks>Leaving "buffers" empty is the same as passing in "-1". Good to know.</remarks>
	public void MarkDraw(params int[] buffers);
	
	/// <summary>
	/// Set the name of this buffer
	/// </summary>
	/// <param name="name">The name to use</param>
	public void SetName(string name);

	/// <summary>
	/// Validate the buffer
	/// </summary>
	public void Validate();
}
