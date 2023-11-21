#define UNIFORMS(__name) cbuffer __name
#define SSBO RWStructuredBuffer

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