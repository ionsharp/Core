sampler2D input : register(s0);

float Grey : register(C0);
float Channel : register(C1);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color; 
	Color = tex2D(input, uv.xy); 

	bool grey = true; //Grey;
	
	float result = 0;
	if (Channel == 0)
	{
		result = Color.r;
	}
	if (Channel == 1)
	{
		result = Color.g;
	}
	if (Channel == 2)
	{
		result = Color.b;
	}
	
	if (grey == 1)
	{
		Color.r = result;
		Color.g = result;
		Color.b = result;
	}
	else if (grey == 0)
	{
		Color.r = result;
		Color.g = 0;
		Color.b = 0;
	}
	return Color; 
}