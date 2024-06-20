namespace OpenAbility.Graphik;

public interface ITexture2D : ITexture
{
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
	/// Allocates storage for texture data
	/// </summary>
	/// <param name="format">The texture format</param>
	/// <param name="width">The texture width</param>
	/// <param name="height">The texture height</param>
	/// <param name="levels">The total amount of texture levels</param>
	public unsafe void AllocateImage(TextureFormat format, int width, int height, int levels = 1);
	
	/// <summary>
	/// Generate mip-maps to a certain depth
	/// </summary>
	/// <param name="depth">The mip depth to generate for</param>
	public void GenerateMipMaps(int depth);
}
