namespace OpenAbility.Graphik;

public interface IRenderTexture
{
	public void Target();
	public void Bind(RenderTextureComponent component, int index = 0);
	public void Build(int width, int height);
}


public enum RenderTextureComponent
{
	Colour,
	Depth
}