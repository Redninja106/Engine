
struct PointLight
{
	float3 pos;
	float rad;
};

StructuredBuffer<PointLight> lights;
SamplerState texSampler;
Texture2D tex;

float4 main(float4 position : SV_Position, float2 uv : TEXCOORD0, float3 normal : TEXCOORD1, float3 worldPos : TEXCOORD2) : SV_Target
{
	float4 color = tex.Sample(texSampler, uv);
	return color;
}