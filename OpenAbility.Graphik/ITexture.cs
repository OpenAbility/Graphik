namespace OpenAbility.Graphik;

public interface ITexture
{


	public void Bind(int index = 0);

	public void Dispose();
	public void CopyFrom(ITexture other);
	public uint GetHandle();
}
