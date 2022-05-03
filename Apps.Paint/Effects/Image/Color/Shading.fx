sampler2D input : register(s0);

float Red : register(C0);
float Green : register(C1);
float Blue : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	
	color.r *= 255;
	color.g *= 255;
	color.b *= 255;
	
    color.r = round(color.r * (Red / 100.0));
    color.g = round(color.g * (Green / 100.0));
    color.b = round(color.b * (Blue / 100.0));
    
	color.r /= 255;
	color.g /= 255;
	color.b /= 255;
	
	color.r = clamp(color.r, 0, 1);
	color.g = clamp(color.g, 0, 1);
	color.b = clamp(color.b, 0, 1);
	return color; 
}