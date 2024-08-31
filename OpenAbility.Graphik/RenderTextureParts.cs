using System.ComponentModel.DataAnnotations;

namespace OpenAbility.Graphik;

public struct RenderTextureParts
{

	public const int ColourLength = 32;
	
	public PartState DepthStencil;
	public PartState Depth;
	public PartState Stencil;

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
		Stencil = PartState.Disabled;
		Depth = PartState.Disabled;
		return this;
	}

	public RenderTextureParts SeparateDepthStencil()
	{
		Stencil = DepthStencil;
		Depth = DepthStencil;
		DepthStencil = PartState.Disabled;
		return this;
	}

	public RenderTextureParts SetDepth(PartState state)
	{
		DepthStencil = PartState.Disabled;
		Depth = state;
		return this;
	}
	
	public RenderTextureParts SetStencil(PartState state)
	{
		DepthStencil = PartState.Disabled;
		Stencil = state;
		return this;
	}


	public static readonly RenderTextureParts Default = new RenderTextureParts();
	public static readonly RenderTextureParts SingleColour = new RenderTextureParts().SetDepthStencil(PartState.Disabled);
	public static readonly RenderTextureParts DefaultSeparated = new RenderTextureParts().SeparateDepthStencil();
	public static readonly RenderTextureParts DepthStencilOnly = new RenderTextureParts().ClearColours().SeparateDepthStencil();
	public static readonly RenderTextureParts ShadowMap = new RenderTextureParts().ClearColours().SetDepth(PartState.Texture).SetStencil(PartState.Disabled);
	
}

public enum PartState
{
	Texture,
	Buffer,
	Disabled
}