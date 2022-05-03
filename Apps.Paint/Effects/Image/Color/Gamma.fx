sampler2D input : register(s0);

float Value : register(C0);
float Scale : register(C1);

float Calculate(float input)
{
    return Scale * pow(abs(input), Value);
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
    color.r = Calculate(color.r);
    color.g = Calculate(color.g);
    color.b = Calculate(color.b);
    return color;
}