namespace OpenAbility.Graphik;

public interface ITexture2D : ITexture
{
	/// <summary>
	/// Prepare for any modifications to the texture by binding it internally
	/// </summary>
	public void PrepareModifications();
	/// <summary>
	/// Set the texture data
	/// </summary>
	/// <param name="format">The format of the texture</param>
	/// <param name="imageData">The raw binary data</param>
	/// <param name="width">The width of the texture</param>
	/// <param name="height">The height of the texture</param>
	/// <param name="mipmapLevel">The mip map level to set the data for</param>
	/// <typeparam name="T">The type of the texture data(byte, float, int, custom struct etc)</typeparam>
	public void SetData<T>(TextureFormat format, T[] imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged;
	/// <summary>
	/// Set the texture data
	/// </summary>
	/// <param name="format">The format of the texture</param>
	/// <param name="imageData">The raw binary data</param>
	/// <param name="width">The width of the texture</param>
	/// <param name="height">The height of the texture</param>
	/// <param name="mipmapLevel">The mip map level to set the data for</param>
	/// <typeparam name="T">The type of the texture data(byte, float, int, custom struct etc)</typeparam>
	public unsafe void SetData<T>(TextureFormat format, T* imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged;
	/// <summary>
	/// Generate mip-maps to a certain depth
	/// </summary>
	/// <param name="depth">The mip depth to generate for</param>
	public void GenerateMipMaps(int depth);
}
