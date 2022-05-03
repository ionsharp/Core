sampler2D input : register(s0);

float Red : register(C0);
float Green : register(C1);
float Blue : register(C2);

float Range : register(C3);

float GetBrightness(float3 input)
{
	float maximum = max(max(input[0], input[1]), input[2]);
	float minimum = min(min(input[0], input[1]), input[2]);
	return (maximum + minimum) / 2.0;
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 

    float r = color.r * 255, g = color.g * 255, b = color.b * 255;

	float3 rgb = { color.r, color.g, color.b };
    float l = GetBrightness(rgb);

    if (/*Highlight*/(Range == 0 && l > 0.66) || /*Midtone*/ (Range == 1 && l > 0.33) || /*Shadow*/(Range == 2 && l <= 0.33))
    {
    	r = r + Red;
    	g = g + Green;
    	b = b + Blue;
    }
    else
    {
    	r += Red * l;
    	g += Green * l;
    	b += Blue * l;
    }
    
    color.r = r / 255;
    color.g = g / 255;
    color.b = b / 255;
    return color;
}