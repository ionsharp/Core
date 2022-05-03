sampler2D input : register(s0);

float4 Fill : register(C0);
float Sides : register(C1);
bool Star : register(C2);
float4 Stroke : register(C3);
float StrokeThickness : register(C4);

cbuffer vars : register(b0)
{
	float2 uResolution;
	float uTime;
};

static const float pi = 3.1415926535;

float polygonShape(float2 position, float radius, float sides)
{
	position = position * 2 - 1;
	float angle = atan2(position.x, position.y);
	float slice = pi * 2 / sides;

	return step(radius, cos(floor(0.5 + angle / slice) * slice - angle) * length(position));
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float sides = Sides;
	if (Sides < 3)
		sides = 6;

	float result = polygonShape(uv, 0.9, sides);

	float4 hello;
	hello.r = Fill.r;
	hello.g = Fill.g;
	hello.b = Fill.b;
	hello.a = result == 0 ? 1 : 0;
	return hello;
}