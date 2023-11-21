namespace OpenAbility.Graphik;

public interface IRenderTexture  : ITexture
{
	public void Target();
	public void Bind(RenderTextureComponent component, int index = 0);
	public void Build(int width, int height, bool colour = true, bool depth = true, bool normal = true);
	public void Dispose();

	/// <summary>
	/// Copy data from another texture
	/// </summary>
	/// <param name="component">The component to copy into(and possibly from)</param>
	/// <param name="other">The other texture to copy data from</param>
	public void CopyChannelFrom(RenderTextureComponent component, ITexture other);

	/// <summary>
	/// Copy data from another texture
	/// </summary>
	/// <param name="sourceChannel">The component to copy from</param>
	/// <param name="targetChannel">The component to copy to</param>
	/// <param name="other">The other texture to copy data from</param>
	public void CopyChannelFrom(RenderTextureComponent sourceChannel, RenderTextureComponent targetChannel, IRenderTexture other);
}


public enum RenderTextureComponent
{
	Colour,
	Depth,
	Normal
}