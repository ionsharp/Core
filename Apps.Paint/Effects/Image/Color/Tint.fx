sampler2D input : register(s0);

float Red : register(C0);
float Green : register(C1);
float Blue : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	
	float r = color.r * 255, g = color.g * 255, b = color.b * 255;
	color.r = round(r + (255 - r) * (Red / 100.0)) / 255;
	color.g = round(g + (255 - g) * (Green / 100.0)) / 255;
	color.b = round(b + (255 - b) * (Blue / 100.0)) / 255;
	
	color.r = clamp(color.r, 0, 1);
	color.g = clamp(color.g, 0, 1);
	color.b = clamp(color.b, 0, 1);
	return color; 
}