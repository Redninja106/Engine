
struct PointLight
{
	float3 pos;
	float rad;
};


SamplerState texSampler;
Texture2D tex;

StructuredBuffer<PointLight> lights;

float3 calculateLight(PointLight light, float3 normal, float3 worldPos)
{
	float3 delta = light.pos - worldPos;
	float deflection = max(0, dot(delta, normal));
	float dist = length(delta);
	float falloff = light.rad / (dist * dist);
	float brightness = deflection * falloff;
	return float3(brightness, brightness, brightness);
}

float4 main(float4 position : SV_Position, float2 uv : TEXCOORD0, float3 normal : TEXCOORD1, float3 worldPos : TEXCOORD2) : SV_Target
{
	float3 albedo = tex.Sample(texSampler, uv).xyz;
	
	float3 ambient = albedo * .2f;
	
	uint elements, stride;
	lights.GetDimensions(elements, stride);
	
	float3 diffuse = float3(0, 0, 0);
	for (int i = 0; i < elements; i++)
	{
		PointLight light = lights[i];
		diffuse += albedo * calculateLight(light, normal, worldPos);
	}
	
	return float4(ambient + diffuse, 1);
}