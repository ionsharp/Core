sampler2D input : register(s0);

float4 Color1 : register(C0);
float4 Color2 : register(C1);

float Level : register(C2);

float GetBrightness(float3 input)
{
	float maximum = max(max(input[0], input[1]), input[2]);
	float minimum = min(min(input[0], input[1]), input[2]);
	return (maximum + minimum) / 2.0;
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	
	float3 rgb = { color.r, color.g, color.b };
    float brightness = GetBrightness(rgb);
    
    if (brightness > Level / 255)
    {
    	color.r = Color1.r;
    	color.g = Color1.g;
    	color.b = Color1.b;
    }
    else
    {
	    color.r = Color2.r;
    	color.g = Color2.g;
    	color.b = Color2.b;
    }
	return color; 
}