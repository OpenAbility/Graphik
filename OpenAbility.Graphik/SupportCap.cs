namespace OpenAbility.Graphik;

public enum SupportCap
{
	/// <summary>
	/// Keeping the Stencil and Depth textures in separate objects.
	/// </summary>
	SeparateStencil,
	/// <summary>
	/// Rendering to HDR
	/// </summary>
	HDR,
	/// <summary>
	/// Bindless textures(MakeResident/FreeResidency instead of Bind)
	/// </summary>
	TexturePointer,
}
