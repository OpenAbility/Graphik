#include "defaults.hlsl"

Texture2D my_texture;
SSBO<float> buffer : BufferBind(1);

VS_OUTPUT vertex(VS_INPUT input)
{
    VS_OUTPUT output;

    output.Position = float4(input.Position.xy, 0, 1.0f);
    output.UV = input.UV;
    
    return output;
}

FS_OUTPUT fragment(VS_OUTPUT input)
{
    FS_OUTPUT output;
    output.Color = tex2D(my_texture, input.UV) * buffer[0];
    return output;
}
