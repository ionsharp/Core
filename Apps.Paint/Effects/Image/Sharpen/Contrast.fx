sampler2D input : register(s0);

float Contrast : register(C0);

float Coerce(float input, float maximum, float minimum = 0)
{
	return max(min(maximum, input), minimum);
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color = tex2D(input, uv.xy); 

	float c = Contrast;
	
    float nr = Color.r * 255, ng = Color.g * 255, nb = Color.b * 255;
	float contrastFactor = (259.0 * (c + 255.0)) / (255.0 * (259.0 - c));
    nr = round(Coerce((contrastFactor * (nr - 128)) + 128, 255));
    ng = round(Coerce((contrastFactor * (ng - 128)) + 128, 255));
    nb = round(Coerce((contrastFactor * (nb - 128)) + 128, 255));

    Color.r = Coerce(nr, 255) / 255;
    Color.g = Coerce(ng, 255) / 255;
    Color.b = Coerce(nb, 255) / 255;
	return Color; 
}