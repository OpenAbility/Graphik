using System.ComponentModel.DataAnnotations;

namespace OpenAbility.Graphik;

public struct RenderTextureParts
{

	public const int ColourLength = 32;
	
	public PartState DepthStencil;

	public readonly PartState[] Colours = new PartState[ColourLength];
	
	public RenderTextureParts()
	{
		for (int i = 0; i < ColourLength; i++)
		{
			Colours[i] = PartState.Disabled;
		}
		Colours[1] = PartState.Texture;
		DepthStencil = PartState.Buffer;
	}

	public RenderTextureParts ClearColours()
	{
		for (int i = 0; i < ColourLength; i++)
		{
			Colours[i] = PartState.Disabled;
		}
		return this;
	}
	
	public RenderTextureParts SetColour([Range(0, ColourLength)] int colour, PartState state)
	{
		Colours[colour] = state;
		return this;
	}
	
	public RenderTextureParts SetDepthStencil(PartState state)
	{
		DepthStencil = state;
		return this;
	}


	public static readonly RenderTextureParts Default = new RenderTextureParts();
	public static readonly RenderTextureParts DepthStencilOnly = new RenderTextureParts().ClearColours();
	public static readonly RenderTextureParts ShadowMap = new RenderTextureParts().ClearColours().SetDepthStencil(PartState.Texture);
	
}

public enum PartState
{
	Texture,
	Buffer,
	Disabled
}