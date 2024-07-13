namespace OpenAbility.Graphik;

public interface IRenderTexture  : ITexture
{
	public void Target();
	public void Bind(RenderTextureComponent component, int index = 0);
	public void Build(int width, int height, RenderTextureParts parts);
	public void SetDefaultComponent(RenderTextureComponent component);
	
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

	public ulong GetPointer(RenderTextureComponent component);
	public void MakeResident(RenderTextureComponent component);
	public void FreeResidency(RenderTextureComponent component);
}


public enum RenderTextureComponent
{
	Colour0 = 0,
	Colour1 = 1,
	Colour2 = 2,
	Colour3 = 3,
	Colour4 = 4,
	Colour5 = 5,
	Colour6 = 6,
	Colour7 = 7,
	Colour8 = 8,
	Colour9 = 9,
	Colour10 = 10,
	Colour11 = 11,
	Colour12 = 12,
	Colour13 = 13,
	Colour14 = 14,
	Colour15 = 15,
	Colour16 = 16,
	Colour17 = 17,
	Colour18 = 18,
	Colour19 = 19,
	Colour20 = 20,
	Colour21 = 21,
	Colour22 = 22,
	Colour23 = 23,
	Colour24 = 24,
	Colour25 = 25,
	Colour26 = 26,
	Colour27 = 27,
	Colour28 = 28,
	Colour29 = 29,
	Colour30 = 30,
	Colour31 = 31,
	
	DepthStencil = 32,
	
	Depth = 33,
	Stencil = 34,
	Colour = Colour0,
	Normal = Colour1,

}