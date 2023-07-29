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
	/// A red and green channel with 3 bytes each, and a blue channel with 2 bytes
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
}
