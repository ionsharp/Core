sampler2D Input : register(s0);

float Angle 
	: register(C0);

static float pi = 3.14159265359;

float2 rotateUV(float2 uv, float2 pivot, float rotation) 
{
	float sine = sin(rotation);
	float cosine = cos(rotation);

	uv -= pivot;
	uv.x = uv.x * cosine - uv.y * sine;
	uv.y = uv.x * sine + uv.y * cosine;
	uv += pivot;

	return uv;
}

float4 main(float2 xy : TEXCOORD) : COLOR 
{ 
	return tex2D(Input, rotateUV(xy, float2(0.5), pi / 4));
}