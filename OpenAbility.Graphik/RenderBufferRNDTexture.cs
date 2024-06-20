namespace OpenAbility.Graphik;

public class RenderBufferRndTexture : IRenderTexture
{

	private IRenderBuffer renderBuffer;
	private ITexture2D?[] colours = new ITexture2D?[32];
	private ITexture2D? depthStencil;
	private ITexture2D? firstColour;
	private int width;
	private int height;

	public void Bind(int index = 0)
	{
		Bind(RenderTextureComponent.Colour, index);
	}
	public void PrepareModifications()
	{
		
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
	public void Target()
	{
		renderBuffer.Target(width, height);
	}

	private ITexture? GetTexture(RenderTextureComponent component)
	{
		return component switch
		{
			RenderTextureComponent.DepthStencil => depthStencil,
			_ => colours[(int)component]
		};
	}
	
	public void Bind(RenderTextureComponent component, int index = 0)
	{
		GetTexture(component)?.Bind(index);
	}

	public unsafe void Build(int width, int height, RenderTextureParts parts)
	{
		this.width = width;
		this.height = height;
		renderBuffer = Graphik.CreateRenderBuffer();
		List<int> buf = new List<int>();

		for (int i = 0; i < 16; i++)
		{
			if (parts.Colours[i] == PartState.Texture)
			{
				ITexture2D texture = Graphik.CreateTexture();
				colours[i] = texture;
				texture.AllocateImage(TextureFormat.Rgbaf, width, height);
				texture.SetData(TextureFormat.Rgbaf, (byte*)null, width, this.height);
				texture.GenerateMipMaps(1);
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
			depthStencil.SetData(TextureFormat.DepthStencil, (byte*)null, width, this.height);
			depthStencil.SetRepetition(TextureRepetition.ClampToBorder);
			depthStencil.GenerateMipMaps(1);
			depthStencil.SetBorder(1, 1, 1, 1);
			renderBuffer.BindDepthStencilTexture(depthStencil);
		} else if (parts.DepthStencil == PartState.Buffer)
		{
			renderBuffer.BindDepthStencilBuffer(width, height);
		}
		
		renderBuffer.MarkDraw(buf.ToArray());
		
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
	public void SetBorder(float r, float g, float b, float a = 1)
	{
		firstColour?.SetBorder(r, g, b, a);
	}
	public unsafe void GetData(void* buffer, int bufferSize)
	{
		firstColour?.GetData(buffer, bufferSize);
	}
}
