sampler2D input : register(S0);

float BlendMode
	: register(C0);
float Opacity
	: register(C1);

//...

float Angle 
	: register(C2);
float OffsetX 
	: register(C3);
float OffsetY 
	: register(C4);
bool Reflect 
	: register(C5);
bool Reverse
	: register(C6);
float Scale 
	: register(C7);
float TypeIndex 
	: register(C8);

//...

float4 Color1 : register(C9);
float Offset1 : register(C10);

float4 Color2 : register(C11);
float Offset2 : register(C12);

float4 Color3 : register(C13);
float Offset3 : register(C14);

float4 Color4 : register(C15);
float Offset4 : register(C16);

float4 Color5 : register(C17);
float Offset5 : register(C18);

float4 Color6 : register(C19);
float Offset6 : register(C20);

float4 Color7 : register(C21);
float Offset7 : register(C22);

float4 Color8 : register(C23);
float Offset8 : register(C24);

float4 Color9 : register(C25);
float Offset9 : register(C26);

float4 Color10 : register(C27);
float Offset10 : register(C28);

float4 Color11 : register(C29);
float Offset11 : register(C30);

float4 Color12 : register(C31);
float Offset12 : register(C32);

float4 Color13 : register(C33);
float Offset13 : register(C34);

float4 Color14 : register(C35);
float Offset14 : register(C36);

float4 Color15 : register(C37);
float Offset15 : register(C38);

float4 Color16 : register(C39);
float Offset16 : register(C40);

float Length : register(C41);

//To do: Support arbitrary gradient sizes.

//...

/*
uniform sampler1D Colors: register(S1);
uniform float ColorsLength : register(C9);

uniform sampler1D Offsets: register(S2);
uniform float OffsetsLength : register(C10);
*/

//...

static float pi = 3.14159265359;

//...

static float Encode(float4 color)
{
	return color.r + color.g * 256.0 + color.b * 256.0 * 256.0 + color.a * 256.0 * 256.0 * 256.0;
}

static float GetDistance(float x1, float y1, float x2, float y2)
{
	return sqrt(pow(abs(x1 - x2), 2) + pow(abs(y1 - y2), 2));
}

static float FLerp(float norm, float min, float max)
{
	return (max - min) * norm + min;
}

//...

#define BlendColorDodgef(base, blend)	((blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0))
#define BlendColorBurnf(base, blend)	((blend == 0.0) ? blend : max((1.0 - ((1.0 - base) / blend)), 0.0))
#define BlendNegation(base, blend)	(float3(1.0, 1.0, 1.0) - abs(float3(1.0, 1.0, 1.0) - base - blend))
#define BlendPhoenix(base, blend)	(min(base, blend) - max(base, blend) + float3(1.0, 1.0, 1.0))
#define BlendReflectf(base, blend)	((blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0))
#define BlendSoftLightf(base, blend)	((blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend)))
#define BlendVividLightf(base, blend)	((blend < 0.5) ? BlendColorBurnf(base, (2.0 * blend)) : BlendColorDodgef(base, (2.0 * (blend - 0.5))))
#define BlendHardMixf(base, blend)		((BlendVividLightf(base, blend) < 0.5) ? 0.0 : 1.0)

float GetHue(float temp1, float temp2, float temp3)
{
	if (temp3 < 0.0)
		temp3 += 1.0;
	else if (temp3 > 1.0)
		temp3 -= 1.0;

	float rgbComponent;

	if ((6.0 * temp3) < 1.0)
		rgbComponent = temp1 + (temp2 - temp1) * 6.0 * temp3;
	else if ((2.0 * temp3) < 1.0)
		rgbComponent = temp2;
	else if ((3.0 * temp3) < 2.0)
		rgbComponent = temp1 + (temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0;
	else
		rgbComponent = temp1;

	return rgbComponent;
}

//...

float3 RGBToHSL(float3 color)
{
	float3 hsl;

	float fmin = min(min(color.r, color.g), color.b);
	float fmax = max(max(color.r, color.g), color.b);
	float delta = fmax - fmin;
	float maxplusmin = fmax + fmin;

	// set luminance
	hsl.z = maxplusmin / 2.0;

	if (delta == 0.0)
	{
		// a gray color, set hue and satuation to 0
		hsl.x = 0.0;
		hsl.y = 0.0;
	}
	else
	{
		// not a gray color

		// set saturation
		if (hsl.z < 0.5)
			hsl.y = delta / (maxplusmin);
		else
			hsl.y = delta / (2.0 - maxplusmin);

		if (color.r == fmax)
			hsl.x = ((color.g - color.b) / delta);
		else if (color.g == fmax)
			hsl.x = 2.0 + ((color.b - color.r) / delta);
		else if (color.b == fmax)
			hsl.x = 4.0 + ((color.r - color.g) / delta);

		hsl.x = hsl.x / 6.0;

		if (hsl.x < 0.0)
			hsl.x += 1.0;
	}

	return hsl;
}

float3 HSLToRGB(float3 hsl)
{
	float3 rgb;

	if (hsl.y == 0.0)
	{
		rgb = float3(hsl.z, hsl.z, hsl.z);
	}
	else
	{
		float temp2;
		if (hsl.z < 0.5)
			temp2 = hsl.z * (1.0 + hsl.y);
		else
			temp2 = (hsl.y + hsl.z) - (hsl.y * hsl.z);

		float temp1 = 2.0 * hsl.z - temp2;

		rgb.r = GetHue(temp1, temp2, hsl.x + (1.0 / 3.0));
		rgb.g = GetHue(temp1, temp2, hsl.x);
		rgb.b = GetHue(temp1, temp2, hsl.x - (1.0 / 3.0));
	}

	return rgb;
}

//...

float4 Blend(float4 inputColor, float4 blendColor)
{
	blendColor.r = blendColor.r == 0 ? 0.001 : blendColor.r;
	blendColor.g = blendColor.g == 0 ? 0.001 : blendColor.g;
	blendColor.b = blendColor.b == 0 ? 0.001 : blendColor.b;

	float4 result;
	result.a = inputColor.a;

	//Average
	if (BlendMode == 0)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.rgb = (inputColor.rgb + blendColor.rgb) / 2;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Color burn
	else if (BlendMode == 1)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = 1 - (1-Base) / Blend
		result.rgb = clamp(1 - (1 - inputColor.rgb) / blendColor.rgb, 0, 1);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Color dodge
	else if (BlendMode == 2)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = Base / (1-Blend)
		result.r = BlendColorDodgef(inputColor.r, blendColor.r);
		result.g = BlendColorDodgef(inputColor.g, blendColor.g);
		result.b = BlendColorDodgef(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Color
	else if (BlendMode == 3)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		float3 blendHSL = RGBToHSL(blendColor.rgb);
		result.rgb = HSLToRGB(float3(blendHSL.r, blendHSL.g, RGBToHSL(inputColor.rgb).b));

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Darken
	else if (BlendMode == 4)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		if (inputColor.r > blendColor.r)
		{
			result.r = blendColor.r;
		}
		else result.r = inputColor.r;

		if (inputColor.g > blendColor.g)
		{
			result.g = blendColor.g;
		}
		else
			result.g = inputColor.g;

		if (inputColor.b > blendColor.b)
			result.b = blendColor.b;
		else result.b = inputColor.b;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Difference
	else if (BlendMode == 5)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = | Base - Blend |
		result.rgb = abs(inputColor.rgb - blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Exclusion
	else if (BlendMode == 6)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = ½ - 2×(Base-½)×(Blend-½) 
		result.rgb = 0.5 - 2 * (inputColor.rgb - 0.5) * (blendColor.rgb - 0.5);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Glow
	else if (BlendMode == 7)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.r = BlendReflectf(blendColor.r, inputColor.r);
		result.g = BlendReflectf(blendColor.g, inputColor.g);
		result.b = BlendReflectf(blendColor.b, inputColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Hard light
	else if (BlendMode == 8)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// if (Blend > ½) R = 1 - (1-Base) × (1-2×(Blend-½))
		// if (Blend <= ½) R = Base × (2×Blend)
		if (blendColor.r > 0.5)
		{
			result.r = 1 - (1 - inputColor.r) * (1 - 2 * (blendColor.r - 0.5));
		}
		else result.r = inputColor.r * (2 * blendColor.r);

		if (blendColor.g > 0.5)
		{
			result.g = 1 - (1 - inputColor.g) * (1 - 2 * (blendColor.g - 0.5));
		}
		else result.g = inputColor.g * (2 * blendColor.g);

		if (blendColor.b > 0.5)
		{
			result.b = 1 - (1 - inputColor.b) * (1 - 2 * (blendColor.b - 0.5));
		}
		else result.b = inputColor.b * (2 * blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Hard mix
	else if (BlendMode == 9)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.r = BlendHardMixf(inputColor.r, blendColor.r);
		result.g = BlendHardMixf(inputColor.g, blendColor.g);
		result.b = BlendHardMixf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Hue
	else if (BlendMode == 10)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		float3 baseHSL = RGBToHSL(inputColor.rgb);
		result.rgb = HSLToRGB(float3(RGBToHSL(blendColor.rgb).x, baseHSL.y, baseHSL.z));

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Lighten
	else if (BlendMode == 11)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		if (inputColor.r < blendColor.r)
		{
			result.r = blendColor.r;
		}
		else result.r = inputColor.r;

		if (inputColor.g < blendColor.g)
		{
			result.g = blendColor.g;
		}
		else result.g = inputColor.g;

		if (inputColor.b < blendColor.b)
		{
			result.b = blendColor.b;
		}
		else result.b = inputColor.b;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Linear burn
	else if (BlendMode == 12)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = Base + Blend - 1
		result.rgb = clamp(inputColor.rgb + blendColor.rgb - 1, 0, 1);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Linear dodge
	else if (BlendMode == 13)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = Base + Blend
		result.rgb = inputColor.rgb + blendColor.rgb;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Linear light
	else if (BlendMode == 14)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// if (Blend > ½) R = Base + 2×(Blend-½)
		// if (Blend <= ½) R = Base + 2×Blend - 1
		if (blendColor.r > 0.5)
		{
			result.r = inputColor.r + 2 * (blendColor.r - 0.5);
		}
		else result.r = inputColor.r + 2 * blendColor.r - 1;

		if (blendColor.g > 0.5)
		{
			result.g = inputColor.g + 2 * (blendColor.g - 0.5);
		}
		else result.g = inputColor.g + 2 * blendColor.g - 1;

		if (blendColor.b > 0.5)
		{
			result.b = inputColor.b + 2 * (blendColor.b - 0.5);
		}
		else result.b = inputColor.b + 2 * blendColor.b - 1;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Luminosity
	else if (BlendMode == 15)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		float3 baseHSL = RGBToHSL(inputColor.rgb);
		result.rgb = HSLToRGB(float3(baseHSL.x, baseHSL.y, RGBToHSL(blendColor.rgb).z));

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Multiply
	else if (BlendMode == 16)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = Base * Blend
		result.rgb = blendColor.rgb * inputColor.rgb;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Negation
	else if (BlendMode == 17)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.rgb = BlendNegation(inputColor, blendColor);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Normal
	else if (BlendMode == 18)
	{
		result.a = 1.0 - (1.0 - blendColor.a) * (1.0 - inputColor.a);
		result.r = blendColor.r * blendColor.a / result.a + inputColor.r * inputColor.a * (1.0 - blendColor.a) / result.a;
		result.g = blendColor.g * blendColor.a / result.a + inputColor.g * inputColor.a * (1.0 - blendColor.a) / result.a;
		result.b = blendColor.b * blendColor.a / result.a + inputColor.b * inputColor.a * (1.0 - blendColor.a) / result.a;
	}
	//Overlay
	else if (BlendMode == 19)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// if (Base > ½) R = 1 - (1-2×(Base-½)) × (1-Blend)
		// if (Base <= ½) R = (2×Base) × Blend

		if (inputColor.r > 0.5)
		{
			result.r = 1 - (1 - 2 * (inputColor.r - 0.5)) * (1 - blendColor.r);
		}
		else result.r = (2 * inputColor.r) * blendColor.r;

		if (inputColor.g > 0.5)
		{
			result.g = 1 - (1 - 2 * (inputColor.g - 0.5)) * (1 - blendColor.g);
		}
		else result.g = (2 * inputColor.g) * blendColor.g;

		if (inputColor.b > 0.5)
		{
			result.b = 1 - (1 - 2 * (inputColor.b - 0.5)) * (1 - blendColor.b);
		}
		else result.b = (2 * inputColor.b) * blendColor.b;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Phoenix
	if (BlendMode == 20)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.rgb = BlendPhoenix(inputColor, blendColor);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Pin light
	else if (BlendMode == 21)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// if (Blend > ½) R = max(Base,2×(Blend-½))
		// if (Blend <= ½) R = min(Base,2×Blend))
		if (blendColor.r > 0.5)
		{
			result.r = max(inputColor.r, 2 * (blendColor.r - 0.5));
		}
		else result.r = min(inputColor.r, 2 * blendColor.r);

		if (blendColor.g > 0.5)
		{
			result.g = max(inputColor.g, 2 * (blendColor.g - 0.5));
		}
		else result.g = min(inputColor.g, 2 * blendColor.g);

		if (blendColor.b > 0.5)
		{
			result.b = max(inputColor.b, 2 * (blendColor.b - 0.5));
		}
		else result.b = min(inputColor.b, 2 * blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Reflect
	else if (BlendMode == 22)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.r = BlendReflectf(inputColor.r, blendColor.r);
		result.g = BlendReflectf(inputColor.g, blendColor.g);
		result.b = BlendReflectf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}

	//* Nesting depth limitation of 24

	//Saturation
	else if (BlendMode == 23)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		float3 baseHSL = RGBToHSL(inputColor.rgb);
		result.rgb = HSLToRGB(float3(baseHSL.x, RGBToHSL(blendColor.rgb).y, baseHSL.z));

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Screen
	else if (BlendMode == 24)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		// R = 1 - (1-Base) × (1-Blend)
		result.rgb = 1 - (1 - inputColor.rgb) * (1 - blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Soft light
	else if (BlendMode == 25)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.r = BlendSoftLightf(inputColor.r, blendColor.r);
		result.g = BlendSoftLightf(inputColor.g, blendColor.g);
		result.b = BlendSoftLightf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Vivid light
	else if (BlendMode == 26)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend BlendMode math
		result.r = BlendVividLightf(inputColor.r, blendColor.r);
		result.g = BlendVividLightf(inputColor.g, blendColor.g);
		result.b = BlendVividLightf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	return result;
}

//...

float4 Blend(float t)
{
	t = t * (1 - Scale);
	//float4 color1 = tex1D(Colors, 0 / ColorsLength);
	//float4 color2 = tex1D(Colors, 1 / ColorsLength);

	//Encode(tex1D(Offsets, 0 / OffsetsLength));
	//Encode(tex1D(Offsets, 1 / OffsetsLength));

	float4 color1 = Color1;
	float4 color2 = Color2;

	float offset1 = Offset1;
	float offset2 = Offset2;

	if (Length == 0)
	{
		color1 = float4(0, 0, 0, 0);
		color2 = color1;

		offset1 = 0;
		offset2 = 1;
	}
	else if (Length == 1)
	{
		color1 = Color1;
		color2 = Color1;

		offset1 = 0;
		offset2 = 1;
	}
	else if (Length == 2)
	{
		color1 = Color1;
		color2 = Color2;

		offset1 = 0;
		offset2 = 1;
	}
	else
	{
		if (t >= Offset1 && t < Offset2)
		{
			color1 = Color1; color2 = Color2;
			offset1 = Offset1; offset2 = Offset2;
		}
		else if (t >= Offset2 && t < Offset3)
		{
			color1 = Color2; color2 = Color3;
			offset1 = Offset2; offset2 = Offset3;
		}
		else if (t >= Offset3 && t < Offset4)
		{
			color1 = Color3; color2 = Color4;
			offset1 = Offset3; offset2 = Offset4;
		}
		else if (t >= Offset4 && t < Offset5)
		{
			color1 = Color4; color2 = Color5;
			offset1 = Offset4; offset2 = Offset5;
		}
		else if (t >= Offset5 && t < Offset6)
		{
			color1 = Color5; color2 = Color6;
			offset1 = Offset5; offset2 = Offset6;
		}
		else if (t >= Offset6 && t < Offset7)
		{
			color1 = Color6; color2 = Color7;
			offset1 = Offset6; offset2 = Offset7;
		}
		else if (t >= Offset7 && t < Offset8)
		{
			color1 = Color7; color2 = Color8;
			offset1 = Offset7; offset2 = Offset8;
		}
		else if (t >= Offset8 && t < Offset9)
		{
			color1 = Color8; color2 = Color9;
			offset1 = Offset8; offset2 = Offset9;
		}
		else if (t >= Offset9 && t < Offset10)
		{
			color1 = Color9; color2 = Color10;
			offset1 = Offset9; offset2 = Offset10;
		}
		else if (t >= Offset10 && t < Offset11)
		{
			color1 = Color10; color2 = Color11;
			offset1 = Offset10; offset2 = Offset11;
		}
		else if (t >= Offset11 && t < Offset12)
		{
			color1 = Color11; color2 = Color12;
			offset1 = Offset11; offset2 = Offset12;
		}
		else if (t >= Offset12 && t < Offset13)
		{
			color1 = Color12; color2 = Color13;
			offset1 = Offset12; offset2 = Offset13;
		}
		else if (t >= Offset13 && t < Offset14)
		{
			color1 = Color13; color2 = Color14;
			offset1 = Offset13; offset2 = Offset14;
		}
		else if (t >= Offset14 && t < Offset15)
		{
			color1 = Color14; color2 = Color15;
			offset1 = Offset14; offset2 = Offset15;
		}
		else
		{
			color1 = Color15; color2 = Color16;
			offset1 = Offset15; offset2 = Offset16;
		}
	}

	float q = offset2 - offset1;
	float p = q == 0 ? 0 : (t - offset1) / q;

	color2.a = p;
	return Blend(color1, color2);
}

//...

#define GetRadian(input) (pi / 180) * input;

//...

float4 RenderAngle(float2 xy)
{
    float r = 2 * pi;

    float t = atan2(xy.y - 0.5, xy.x - 0.5) + GetRadian(Angle);
    t = t + pi;
    if (t > r)
        t = t - r;

    t = t / r;
    return Blend(t);
}

float4 RenderCircle(float2 xy)
{
    float md = GetDistance(0.5, 0.5, 1, 1);
    float nd = min(GetDistance(0.5, 0.5, xy.x, xy.y) / md, 1);
    float t = FLerp(nd, 0, 1);
    return Blend(t);
}

float4 RenderDiamond(float2 xy)
{
    float t = 0;
    float x = xy.x; float y = xy.y;

    //Top
    if (y <= 0.5)
    {
        if (x >= y && x <= 1 - y)
            t = y / 0.5;
    }

    //Bottom
    else if (y > 0.5)
    {
        if (x >= 1 - y && x <= y)
            t = 1 - ((y - 0.5) / 0.5);
    }

    //Left
    if (x <= 0.5)
    {
        if (y >= x && y <= 1 - x)
            t = x / 0.5;
    }

    //Right
    else if (x > 0.5)
    {
        if (y >= 1 - x && y <= x)
            t = 1 - ((x - 0.5) / 0.5);
    }

    return Blend(t);
}

float4 RenderLinear(float2 xy)
{
    float t = xy.x / 1;
    return Blend(t);
}

//...

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color1 = tex2D(input, uv.xy);

    if (color1.a > 0)
    {
		float4 color2;
		if (TypeIndex == 0)
			color2 = RenderAngle(uv.xy);

		else if (TypeIndex == 1)
			color2 = RenderCircle(uv.xy);
		
		else if (TypeIndex == 2)
			color2 = RenderDiamond(uv.xy);

		else color2 = RenderLinear(uv.xy);
		color2.a = Opacity;

		return Blend(color1, color2);
    }
    return color1;
}