#include "matrixBuffer.hlsl"

struct VSOut
{
	float4 position : SV_Position;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
};

VSOut main(float3 pos : POSITION, float2 uv: UV, float3 normal : NORMAL)
{
	VSOut result;
	float4 position = float4(pos, 1);
	position = mul(position, world);
	result.worldPos = position;
	position = mul(position, view);
	position = mul(position, proj);
	
	result.uv = uv;
	result.position = position;
	result.normal = normal;
	return result;
}