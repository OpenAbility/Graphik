namespace OpenAbility.Graphik;

public interface IRenderTexture  : ITexture
{
	public void Target();
	public void Bind(RenderTextureComponent component, int index = 0);
	public void Build(int width, int height, bool colour = true, bool depth = true);
	public void Dispose();
}


public enum RenderTextureComponent
{
	Colour,
	Depth
}