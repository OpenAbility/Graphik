#define UNIFORM_BUFFER(__name) cbuffer __name

struct VS_INPUT
{
    float3 Position;
    float2 UV;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
};

struct FS_OUTPUT
{
    float4 Color : SV_TARGET;
};

UNIFORM_BUFFER(my_buffer)
{
    float hi;
}


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
    output.Color = float4(1, 1, 1, 1);
    return output;
}
