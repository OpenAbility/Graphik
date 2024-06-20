namespace OpenAbility.Graphik;

/// <summary>
/// The format of a texture
/// </summary>
public enum TextureFormat
{
	/// <summary>
	/// A single byte representing a single red channel
	/// </summary>
	R8,
	/// <summary>
	/// A red, green and blue channel with 1 byte each
	/// </summary>
	Rgb8, 
	/// <summary>
	/// A red, a green, a blue and an alpha channel with 2 bytes each
	/// </summary>
	Rgba8,
	
	/// <summary>
	/// An unsigned 32-bit integer that represents a single red channel
	/// </summary>
	R32, 
	/// <summary>
	/// A red, a green and a blue channel each represented by a 32-bit integer
	/// </summary>
	Rgb32,
	/// <summary>
	/// 4 unsigned 32-bit integers representing a red, a green, a blue and an alpha channel
	/// </summary>
	Rgba32,
	
	/// <summary>
	/// A 32-bit float representing a single red channel
	/// </summary>
	Rf,
	/// <summary>
	/// A 32-bit float representing a red, a green and a blue channel
	/// </summary>
	Rgbf, 
	/// <summary>
	/// A 32-bit float representing a red, a green a blue and an alpha channel
	/// </summary>
	Rgbaf,
	/// <summary>
	/// A blue, green and red channel with 1 byte each
	/// </summary>
	Bgr8,
	/// <summary>
	/// Implementation-specific depth format
	/// </summary>
	Depth,
	/// <summary>
	/// Implementation-specific stencil format
	/// </summary>
	Stencil,
	/// <summary>
	/// Implementation-specific combined depth-and-stencil format
	/// </summary>
	DepthStencil
}
