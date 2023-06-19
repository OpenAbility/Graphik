using System.ComponentModel.DataAnnotations;

namespace OpenAbility.Graphik;

public interface ITexture
{
	/// <summary>
	/// Bind the texture to a texture index shader-side
	/// </summary>
	/// <param name="index">The index to bind to, should be limited to 32</param>
	public void Bind([Range(0, 32)] int index = 0);
	
	/// <summary>
	/// Delete the texture
	/// </summary>
	public void Dispose();
	
	/// <summary>
	/// Copy data from another texture
	/// </summary>
	/// <param name="other">The other texture to copy data from</param>
	public void CopyFrom(ITexture other);
	
	/// <summary>
	/// Get the native texture handle
	/// </summary>
	/// <returns>The native texture handle</returns>
	public uint GetHandle();
	
	/// <summary>
	/// Set the texture filtering, default is <see cref="TextureFiltering.Nearest"/>
	/// </summary>
	/// <param name="filtering">The filtering to use</param>
	public void SetFiltering(TextureFiltering filtering);
}

public enum TextureFiltering
{
	Nearest,
	Linear
}
