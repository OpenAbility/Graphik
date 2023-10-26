namespace OpenAbility.Graphik;
/// <summary>
/// Various "Features" that can be enabled or disabled
/// </summary>
public enum Feature
{
	/// <summary>
	/// Should the rendering account for depth?
	/// </summary>
	DepthTesting,
	/// <summary>
	/// Should the depth buffer be written to?
	/// </summary>
	DepthWrite,
	/// <summary>
	/// Should blending be done?
	/// </summary>
	Blending,
	/// <summary>
	/// Should we support and render with HDR?
	/// </summary>
	HDR,
	/// <summary>
	/// Should we cull?
	/// </summary>
	Culling,
	/// <summary>
	/// Should we use vertical sync
	/// </summary>
	VSync,
	/// <summary>
	/// Should everything be rendered in wireframe?
	/// </summary>
	Wireframe,
	/// <summary>
	/// Should we do scissor testing?
	/// </summary>
	Scissor,
	/// <summary>
	/// Should we print errors etc?
	/// </summary>
	DebugOutput
}
