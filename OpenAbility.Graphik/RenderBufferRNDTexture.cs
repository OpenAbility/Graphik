namespace OpenAbility.Graphik;

public class RenderBufferRndTexture : IRenderTexture
{

	private IRenderBuffer renderBuffer;
	private ITexture2D?[] colours = new ITexture2D?[RenderTextureParts.ColourLength];
	private ITexture2D? depthStencil;
	private ITexture2D? depth;
	private ITexture2D? stencil;
	private ITexture2D? firstColour;
	private int width;
	private int height;
	private RenderTextureComponent defaultComponent = RenderTextureComponent.Colour;

	public void Bind(int index = 0)
	{
		Bind(defaultComponent, index);
	}
	
	public void PrepareModifications()
	{
		
	}

	public void SetDefaultComponent(RenderTextureComponent component)
	{
		defaultComponent = component;
	}
	
	public void CopyChannelFrom(RenderTextureComponent component, ITexture other)
	{
		GetTexture(component)?.CopyFrom(other);
		
	}
	
	public void CopyChannelFrom(RenderTextureComponent sourceChannel, RenderTextureComponent targetChannel, IRenderTexture other)
	{
		// TODO: same-channel copy?
		GetTexture(targetChannel)?.CopyFrom(other);
	}
	
	public ulong GetPointer(RenderTextureComponent component)
	{
		return GetTexture(component)?.GetPointer() ?? 0;
	}
	
	public void MakeResident(RenderTextureComponent component)
	{
		GetTexture(component)?.MakeResident();
	}
	
	public void FreeResidency(RenderTextureComponent component)
	{
		GetTexture(component)?.MakeResident();
	}
	
	public void Target()
	{
		renderBuffer.Target(width, height);
	}

	private ITexture2D? GetTexture(RenderTextureComponent component)
	{
		return component switch
		{
			RenderTextureComponent.DepthStencil => depthStencil,
			RenderTextureComponent.Depth => depth ?? depthStencil,
			RenderTextureComponent.Stencil => stencil ?? depthStencil,
			_ => colours[(int)component]
		};
	}
	
	public void Bind(RenderTextureComponent component, int index = 0)
	{
		ITexture2D? tex = GetTexture(component);
		if (component == RenderTextureComponent.Stencil && tex == depthStencil)
		{
			tex?.SetDepthStencilMode(DepthStencilMode.Stencil);
		}
		if (component == RenderTextureComponent.Depth && tex == depthStencil)
		{
			tex?.SetDepthStencilMode(DepthStencilMode.Depth);
		}
		tex?.Bind(index);
	}
	

	public void Build(int width, int height, RenderTextureParts parts)
	{
		if (parts.DepthStencil != PartState.Disabled && (parts.Depth != PartState.Disabled || parts.Stencil != PartState.Disabled))
			throw new Exception("Cannot have DepthStencil when either Depth or Stencil are enabled!");

		if (parts.Stencil != PartState.Disabled)
		{
			if (!Graphik.Supports(SupportCap.SeparateStencil))
			{
				Console.Error.WriteLine("Splitting stencil");
				// Texture takes priority over buffer, so if at least one is a texture we make both a texture.
				bool anyTexture = parts.Depth == PartState.Texture || parts.Stencil == PartState.Texture;
				if (anyTexture)
					parts.DepthStencil = PartState.Texture;
				else
					parts.DepthStencil = PartState.Buffer;
				parts.Depth = PartState.Disabled;
				parts.Stencil = PartState.Disabled;
			}
		}

		this.width = width;
		this.height = height;
		renderBuffer = Graphik.CreateRenderBuffer();
		List<int> buf = new List<int>();

		for (int i = 0; i < RenderTextureParts.ColourLength; i++)
		{
			if (parts.Colours[i] == PartState.Texture)
			{
				ITexture2D texture = Graphik.CreateTexture();
				colours[i] = texture;
				texture.AllocateImage(TextureFormat.Rgbaf, width, height);
				renderBuffer.BindColorTexture(i, texture);
				buf.Add(i);

				firstColour ??= texture;
			} else if (parts.Colours[i] == PartState.Buffer)
			{
				renderBuffer.BindColorBuffer(i, TextureFormat.Rgbaf, width, height);
			}
		}
		
		if (parts.DepthStencil == PartState.Texture)
		{
			depthStencil = Graphik.CreateTexture();
			depthStencil.AllocateImage(TextureFormat.DepthStencil, width, height);
			depthStencil.SetRepetition(TextureRepetition.ClampToBorder);
			depthStencil.SetFiltering(TextureFiltering.Nearest);
			depthStencil.SetBorder(1, 1, 1, 1);
			renderBuffer.BindDepthStencilTexture(depthStencil);
		} else if (parts.DepthStencil == PartState.Buffer)
		{
			renderBuffer.BindDepthStencilBuffer(width, height);
		}

		if (parts.Depth == PartState.Texture)
		{
			depth = Graphik.CreateTexture();
			depth.AllocateImage(TextureFormat.Depth, width, height);
			depth.SetRepetition(TextureRepetition.ClampToBorder);
			depth.SetFiltering(TextureFiltering.Nearest);
			depth.SetBorder(1, 1, 1, 1);
			renderBuffer.BindDepthTexture(depth);
		} else if (parts.Depth == PartState.Buffer)
		{
			renderBuffer.BindDepthBuffer(width, height);
		}
		
		if (parts.Stencil == PartState.Texture)
		{
			stencil = Graphik.CreateTexture();
			stencil.AllocateImage(TextureFormat.Stencil, width, height);
			stencil.SetRepetition(TextureRepetition.ClampToEdge);
			stencil.SetFiltering(TextureFiltering.Nearest);
			renderBuffer.BindStencilTexture(stencil);
		} else if (parts.Stencil == PartState.Buffer)
		{
			renderBuffer.BindStencilBuffer(width, height);
		}
		
		if(buf.Any())
			renderBuffer.MarkDraw(buf.ToArray());
		else
			renderBuffer.MarkDraw(-1);
		
		renderBuffer.Validate();
	}
	
	public void Dispose()
	{
		renderBuffer.Dispose();
		foreach (var c in colours)
		{
			c?.Dispose();
		}
		depthStencil?.Dispose();
	}

	public void CopyFrom(ITexture other)
	{
		firstColour?.CopyFrom(other);
	}
	public uint GetHandle()
	{
		for (int i = 0; i < 32; i++)
		{
			if (colours[i] != null)
				return colours[i]!.GetHandle();
		}
		return depthStencil?.GetHandle() ?? 0;
	}
	public void SetFiltering(TextureFiltering filtering)
	{
		for (int i = 0; i < 32; i++)
		{
			colours[i]?.SetFiltering(filtering);
		}
		depthStencil?.SetFiltering(filtering);
	}
	public void SetRepetition(TextureRepetition repetition)
	{
		for (int i = 0; i < 32; i++)
		{
			colours[i]?.SetRepetition(repetition);
		}
		depthStencil?.SetRepetition(repetition);
	}
	public void SetName(string name)
	{
		renderBuffer.SetName(name + ".RB");
		
		for (int i = 0; i < 32; i++)
		{
			colours[i]?.SetName(name + ".C" + i);
		}
		depthStencil?.SetName(name + ".DS");
	}
	public void SetDepthStencilMode(DepthStencilMode depthStencilMode)
	{
		depthStencil?.SetDepthStencilMode(depthStencilMode);
	}
	public ulong GetPointer()
	{
		return GetTexture(defaultComponent)?.GetPointer() ?? 0;
	}
	public void MakeResident()
	{
		GetTexture(defaultComponent)?.MakeResident();
	}
	public void FreeResidency()
	{
		GetTexture(defaultComponent)?.FreeResidency();
	}
	public void SetBorder(float r, float g, float b, float a = 1)
	{
		firstColour?.SetBorder(r, g, b, a);
	}
	public unsafe void GetData(void* buffer, int bufferSize)
	{
		firstColour?.GetData(buffer, bufferSize);
	}
}
