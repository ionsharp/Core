sampler2D input : register(s0);

float R : register(C0);
float G : register(C1);
float B : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 

	if (R == 0)
		color.r = 0;

	if (G == 0)
		color.g = 0;

	if (B == 0)
		color.b = 0;

	return color;
}