sampler2D input : register(S0);

float SizeX : register(C0);
float SizeY : register(C1);
float SizeZ : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float3 color = tex2D(input, uv.xy);

    float3 size = float3(SizeX, SizeY, SizeZ);

    color *= size;
    color = round(color);
    color /= size;
    return float4(color, 1.0);
}