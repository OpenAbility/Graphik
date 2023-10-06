using System.ComponentModel.DataAnnotations;

namespace OpenAbility.Graphik;

public interface ICubemapTexture
{
	public void PrepareModification();
	public unsafe void SetFaceData<T>(CubemapFace face, TextureFormat format, T[] imageData, int width, int height, int mipmapLevel = 0)
		where T : unmanaged
	{
		fixed(T* ptr = imageData)
			SetFaceData(face, format, ptr, width, height, mipmapLevel);
	}
	public unsafe void SetFaceData<T>(CubemapFace face, TextureFormat format, T* imageData, int width, int height, int mipmapLevel = 0) where T : unmanaged;
	
	public void Bind([Range(0, 32)] int slot = 0);
	public void Dispose();
	public uint GetHandle();
	public void SetName(string name);
}

public enum CubemapFace
{
	PositiveX,
	NegativeX,
	
	PositiveY,
	NegativeY,
	
	PositiveZ,
	NegativeZ,
	
	Up = PositiveY,
	Down = NegativeY,
	
	Left = NegativeX,
	Right = PositiveX,
	
	Forward = PositiveZ,
	Backward = NegativeZ
}