sampler2D Texture1Sampler : register(S0);

float Desaturation : register(C0);
float Toned : register(C1);
float4 LightColor : register(C2);
float4 DarkColor : register(C3);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(Texture1Sampler, uv);
    float3 scnColor = LightColor * (color.rgb / color.a);
    float gray = dot(float3(0.3, 0.59, 0.11), scnColor);
    
    float3 muted = lerp(scnColor, gray.xxx, Desaturation);
    float3 middle = lerp(DarkColor, LightColor, gray);
    
    scnColor = lerp(muted, middle, Toned);
    return float4(scnColor * color.a, color.a);
}


