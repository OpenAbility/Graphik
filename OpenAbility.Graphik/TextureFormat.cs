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
	/// 3 bytes representing a red, a green and a blue channel
	/// </summary>
	Rgb8, 
	/// <summary>
	/// 4 bytes representing a red, a green, a blue and an alpha channel
	/// </summary>
	Rgba8,
	
	/// <summary>
	/// An unsigned 32-bit integer that represents a single red channel
	/// </summary>
	R32, 
	/// <summary>
	/// 3 unsigned 32-bit integers representing a red, a green and a blue channel
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
	Rgbaf
}
