#define UNIFORMS(__name) cbuffer __name
#define SSBO RWStructuredBuffer
#define ExplicitSSBO(__type, __name, __bind) RWStructuredBuffer<__type> __name : BufferBind(__bind)
#define BufferBind(__index) register(b##__index)

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