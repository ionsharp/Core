sampler2D input : register(s0);

float Model : register(C0);
float Component : register(C1);
float Mode : register(C2);
float X : register(C3);
float Y : register(C4);
float Z : register(C5);

//...

static float INDEX_LUV = 14;
static float INDEX_XYZ = 22;

static float pi = 3.14159265359;

//The default illuminant (1931, 2°)
static float3 D65 = float3(95.045592705167, 100, 108.9057750759878);

//...

float Coerce(float input, float maximum, float minimum = 0)
{
	return max(min(maximum, input), minimum);
}

float3 ConvertRange(float3 value, float3 minimum, float3 maximum)
{
	value[0] = (value[0] * (maximum[0] - minimum[0])) + minimum[0];
	value[1] = (value[1] * (maximum[1] - minimum[1])) + minimum[1];
	value[2] = (value[2] * (maximum[2] - minimum[2])) + minimum[2];
	return value;
}

float Modulo(float value, float left, float right)
{
    //Swap frame order
    if (left > right)
    {
        float t = right;
        right = left;
        left = t;
    }

    float frame = right - left;
    value = ((value + left) % frame) - left;

    if (value < left)
        value += frame;

    if (value > right)
        value -= frame;

    return value;
}

//...

float3 Maximums(float model)
{
	float3 result = { 0, 0, 0 };

	//RGB
	if (model == 0)
	{
		return float3(1, 1, 1);
	}

	//HCG
	else if (model == 1)
	{
		return float3(359, 100, 100);
	}

	//HCY
	else if (model == 2)
	{
		return float3(359, 100, 255);
	}

	//HPLuv
	else if (model == 3)
	{
		return float3(359, 100, 100);
	}

	//HSB
	else if (model == 4)
	{
		return float3(359, 100, 100);
	}

	//HSL
	else if (model == 5)
	{
		return float3(359, 100, 100);
	}

	//HSLuv
	else if (model == 6)
	{
		return float3(359, 100, 100);
	}

	//HSP
	else if (model == 7)
	{
		return float3(359, 100, 255);
	}

	//HWB
	else if (model == 8)
	{
		return float3(359, 100, 100);
	}

	//LAB
	else if (model == 9)
	{
		return float3(100, 100, 100);
	}

	//LABh
	else if (model == 10)
	{
		return float3(100, 128, 128);
	}

	//LCHab
	else if (model == 11)
	{
		return float3(100, 100, 359);
	}

	//LCHuv
	else if (model == 12)
	{
		return float3(100, 100, 359);
	}

	//LMS
	else if (model == 13)
	{
		return float3(100, 100, 100);
	}

	//LUV
	else if (model == 14)
	{
		return float3(100, 224, 122);
	}

	//TSL
	else if (model == 15)
	{
		return float3(1, 1, 1);
	}

	//HUVcv
	else if (model == 16)
	{
		return float3(100, 100, 359);
	}

	//HUVcy
	else if (model == 17)
	{
		return float3(100, 100, 359);
	}

	//HUVsp
	else if (model == 18)
	{
	return float3(100, 100, 359);
	}

	//UCS
	else if (model == 19)
	{
		return float3(100, 100, 100);
	}

	//UVW
	else if (model == 20)
	{
		return float3(224, 122, 100);
	}

	//xyY
	else if (model == 21)
	{
		return float3(1, 1, 100);
	}

	//XYZ
	else if (model == 22)
	{
		return D65;
	}

	return result;
}

float3 Minimums(float model)
{
	//LAB
	if (model == 9)
	{
		return float3(0, 0, 0);
	}
	//LABh
	else if (model == 10)
	{
		return float3(0, -128, -128);
	}
	//LCHab
	else if (model == 11)
	{
		return float3(0, 0, 0);
	}
	//LCHuv
	else if (model == 12)
	{
		return float3(0, 0, 0);
	}
	//LMS
	else if (model == 13)
	{
		return float3(0, 0, 0);
	}
	//LUV
	else if (model == 14)
	{
		return float3(0, -134, -140);
	}
	//(15) TSL
	//(16) HUVcv
	//(17) HUVcy
	//(18) TSLsp
	//(19) UCS
	if (model == 20)
	{
		return float3(-134, -140, 0);
	}
	//(21) xyY
	//(22) XYZ

	//...

	return float3(0, 0, 0);
}

//... [*] Incomplete

//... RGB (sRGB)

float3 FromHCV(float3 input)
{
	float3 result = { 0, 0, 0 };

	float h = input[0] / 359.0, c = input[1] / 100.0, g = input[2] / 100.0;

	if (c == 0)
	{
		result[0] = result[1] = result[2] = g;
		return result;
	}

	float hi = (h % 1.0) * 6.0;
	float v = hi % 1.0;
	float3 pure = { 0, 0, 0 };
	float w = 1.0 - v;

	float fhi = floor(hi);

	if (fhi == 0)
	{
		pure[0] = 1;
		pure[1] = v;
		pure[2] = 0;
	}
	else if (fhi == 1)
	{
		pure[0] = w;
		pure[1] = 1;
		pure[2] = 0;
	}
	else if (fhi == 2)
	{
		pure[0] = 0;
		pure[1] = 1;
		pure[2] = v;
	}
	else if (fhi == 3)
	{
		pure[0] = 0;
		pure[1] = w;
		pure[2] = 1;
	}
	else if (fhi == 4)
	{
		pure[0] = v;
		pure[1] = 0;
		pure[2] = 1;
	}
	else
	{
		pure[0] = 1;
		pure[1] = 0;
		pure[2] = w;
	}

	float mg = (1.0 - c) * g;

	result[0] = c * pure[0] + mg;
	result[1] = c * pure[1] + mg;
	result[2] = c * pure[2] + mg;
	return result;
}

float3 FromHCY(float3 input)
{
	float h = (input[0] < 0 ? (input[0] % 360) + 360 : (input[0] % 360)) * pi / 180;
	float s = max(0, min(input[1], 100)) / 100;
	float i = max(0, min(input[2], 255)) / 255;

	float pi3 = pi / 3;

	float r, g, b;
	if (h < (2 * pi3)) 
	{
		b = i * (1 - s);
		r = i * (1 + (s * cos(h) / cos(pi3 - h)));
		g = i * (1 + (s * (1 - cos(h) / cos(pi3 - h))));
	}
	else if (h < (4 * pi3)) 
	{
		h = h - 2 * pi3;
		r = i * (1 - s);
		g = i * (1 + (s * cos(h) / cos(pi3 - h)));
		b = i * (1 + (s * (1 - cos(h) / cos(pi3 - h))));
	}
	else 
	{
		h = h - 4 * pi3;
		g = i * (1 - s);
		b = i * (1 + (s * cos(h) / cos(pi3 - h)));
		r = i * (1 + (s * (1 - cos(h) / cos(pi3 - h))));
	}

	return float3(r, g, b);
}

float3 FromHSB(float3 input)
{
	float3 result = { 0, 0, 0 };

	float _h = input[0] / 359, _s = input[1] / 100, _b = input[2] / 100;
	float r = 0, g = 0, b = 0;
	
	if (_s == 0)
	{
	    r = g = b = _b;
	}
	else
	{
	    _h *= 359;
	
	    //The color wheel consists of 6 sectors: Figure out which sector we're in...
	    float SectorPosition = _h / 60.0;
	    float SectorNumber = floor(SectorPosition);
	
	    //Get the fractional part of the sector
	    float FractionalSector = SectorPosition - SectorNumber;
	
	    //Calculate values for the three axes of the color. 
	    float p = _b * (1.0 - _s);
	    float q = _b * (1.0 - (_s * FractionalSector));
	    float t = _b * (1.0 - (_s * (1.0 - FractionalSector)));
	
	    //Assign the fractional colors to r, g, and b based on the sector the angle is in.
        if (SectorNumber == 0)
        {
            r = _b;
            g = t;
            b = p;
        }
        else if (SectorNumber == 1)
        {
            r = q;
            g = _b;
            b = p;
        }
        else if (SectorNumber == 2)
        {
            r = p;
            g = _b;
            b = t;
        }
        else if (SectorNumber == 3)
        {
            r = p;
            g = q;
            b = _b;
        }
        else if (SectorNumber == 4)
        {
            r = t;
            g = p;
            b = _b;
        }
        else if (SectorNumber == 5)
        {
            r = _b;
            g = p;
            b = q;
        }
	}
	result[0] = r;
	result[1] = g;
	result[2] = b;
	return result;
}

float3 FromHSL(float3 input)
{
	float h = input[0], s = input[1] / 100, l = input[2] / 100;
	h /= 60;

	float3 result = { 0, 0, 0 };

	if (s > 0)
	{
		float chroma = (1.0 - abs(2.0 * l - 1.0)) * s;
		float v = chroma * (1.0 - abs((h % 2.0) - 1));

		if (0 <= h && h <= 1)
		{
			result[0] = chroma;
			result[1] = v;
			result[2] = 0;
		}
		else if (1 <= h && h <= 2)
		{
			result[0] = v;
			result[1] = chroma;
			result[2] = 0;
		}
		else if (2 <= h && h <= 3)
		{
			result[0] = 0;
			result[1] = chroma;
			result[2] = v;
		}
		else if (3 <= h && h <= 4)
		{
			result[0] = 0;
			result[1] = v;
			result[2] = chroma;
		}
		else if (4 <= h && h <= 5)
		{
			result[0] = v;
			result[1] = 0;
			result[2] = chroma;
		}
		else if (5 <= h && h <= 6)
		{
			result[0] = chroma;
			result[1] = 0;
			result[2] = v;
		}

		float w = l - (0.5 * chroma);
		result[0] += w;
		result[1] += w;
		result[2] += w;
	}
	else
	{
		result[0] = result[1] = result[2] = l;
	}
	return result;
}

float3 FromHSP(float3 input)
{
	float3 result = { 0, 0, 0 };

	const float Pr = 0.299;
	const float Pg = 0.587;
	const float Pb = 0.114;
	
	float h = input[0] / 360.0, s = input[1] / 100.0, p = input[2];
	float r = 0, g= 0, b= 0;
	
	float part= 0, minOverMax = 1.0 - s;
	
	if (minOverMax > 0.0)
	{
	    // R > G > B
	    if (h < 1.0 / 6.0)
	    {
	        h = 6.0 * (h - 0.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        b = p / sqrt(Pr / minOverMax / minOverMax + Pg * part * part + Pb);
	        r = (b) / minOverMax;
	        g = (b) + h * ((r) - (b));
	    }
	    // G > R > B
	    else if (h < 2.0 / 6.0)
	    {
	        h = 6.0 * (-h + 2.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        b = p / sqrt(Pg / minOverMax / minOverMax + Pr * part * part + Pb);
	        g = (b) / minOverMax;
	        r = (b) + h * ((g) - (b));
	    }
	    // G > B > R
	    else if (h < 3.0 / 6.0)
	    {
	        h = 6.0 * (h - 2.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        r = p / sqrt(Pg / minOverMax / minOverMax + Pb * part * part + Pr);
	        g = (r) / minOverMax;
	        b = (r) + h * ((g) - (r));
	    }
	    // B > G > R
	    else if (h < 4.0 / 6.0)
	    {
	        h = 6.0 * (-h + 4.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        r = p / sqrt(Pb / minOverMax / minOverMax + Pg * part * part + Pr);
	        b = (r) / minOverMax;
	        g = (r) + h * ((b) - (r));
	    }
	    // B > R > G
	    else if (h < 5.0 / 6.0)
	    {
	        h = 6.0 * (h - 4.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        g = p / sqrt(Pb / minOverMax / minOverMax + Pr * part * part + Pg);
	        b = (g) / minOverMax;
	        r = (g) + h * ((b) - (g));
	    }
	    // R > B > G
	    else
	    {
	        h = 6.0 * (-h + 6.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        g = p / sqrt(Pr / minOverMax / minOverMax + Pb * part * part + Pg);
	        r = (g) / minOverMax;
	        b = (g) + h * ((r) - (g));
	    }
	}
	else
	{
	    // R > G > B
	    if (h < 1.0 / 6.0)
	    {
	        h = 6.0 * (h - 0.0 / 6.0);
	        r = sqrt(p * p / (Pr + Pg * h * h));
	        g = (r) * h;
	        b = 0.0;
	    }
	    // G > R > B
	    else if (h < 2.0 / 6.0)
	    {
	        h = 6.0 * (-h + 2.0 / 6.0);
	        g = sqrt(p * p / (Pg + Pr * h * h));
	        r = (g) * h;
	        b = 0.0;
	    }
	    // G > B > R
	    else if (h < 3.0 / 6.0)
	    {
	        h = 6.0 * (h - 2.0 / 6.0);
	        g = sqrt(p * p / (Pg + Pb * h * h));
	        b = (g) * h;
	        r = 0.0;
	    }
	    // B > G > R
	    else if (h < 4.0 / 6.0)
	    {
	        h = 6.0 * (-h + 4.0 / 6.0);
	        b = sqrt(p * p / (Pb + Pg * h * h));
	        g = (b) * h;
	        r = 0.0;
	    }
	    // B > R > G
	    else if (h < 5.0 / 6.0)
	    {
	        h = 6.0 * (h - 4.0 / 6.0);
	        b = sqrt(p * p / (Pb + Pr * h * h));
	        r = (b) * h;
	        g = 0.0;
	    }
	    // R > B > G
	    else
	    {
	        h = 6.0 * (-h + 6.0 / 6.0);
	        r = sqrt(p * p / (Pr + Pb * h * h));
	        b = (r) * h;
	        g = 0.0;
	    }
	}
	result[0] = Coerce(round(r) / 255.0, 1);
	result[1] = Coerce(round(g) / 255.0, 1);
	result[2] = Coerce(round(b) / 255.0, 1);
	return result;
}

float3 FromHWB(float3 input)
{
	float3 result = { 0, 0, 0 };

	float white = input.y / 100;
	float black = input.z / 100;

	if (white + black >= 1)
	{
		float gray = white / (white + black);
		return float3(gray, gray, gray);
	}

	float3 hsl = float3(input.x, 1, 0.5);
	float3 rgb = FromHSL(hsl);
	rgb.x *= (1 - white - black);
	rgb.x += white;

	rgb.y *= (1 - white - black);
	rgb.y += white;

	rgb.z *= (1 - white - black);
	rgb.z += white;
	return rgb;
}

float3 FromTSL(float3 input)
{
	float T = input[0], S = input[1], L = input[2];

	//wikipedia solution
	/*
	// var x = - 1 / Math.tan(Math.PI * 2 * T);
	var x = -Math.sin(2*Math.PI*T);
	if ( x != 0 ) x = Math.cos(2*Math.PI*T)/x;
	var g = T > .5 ? -S * Math.sqrt( 5 / (9 * (x*x + 1)) ) :
			T < .5 ? S * Math.sqrt( 5 / (9 * (x*x + 1)) ) : 0;
	var r = T === 0 ? 0.7453559 * S : (x * g + 1/3);
	var R = k * r, G = k * g, B = k * (1 - r - g);
	*/

	float x = tan(2 * pi * (T - 1 / 4));
	x = x * x;

	float r = sqrt(5 * S * S / (9 * (1 / x + 1))) + 1 / 3;
	float g = sqrt(5 * S * S / (9 * (x + 1))) + 1 / 3;

	float k = L / (.185 * r + .473 * g + .114);

	float B = k * (1 - r - g);
	float G = k * g;
	float R = k * r;

	return float3(R, G, B);
}

//... XYZ (2°, D65)

float3 FromXYZ(float3 input)
{
	float x = input[0] / D65[0];
	float y = input[1] / D65[1];
	float z = input[2] / D65[2];

	//Linear transformation
	float r = 0; float g = 0; float b = 0;
	r = (x * 3.240969941904521) + (y * -1.537383177570093) + (z * -0.498610760293);
	g = (x * -0.969243636280870) + (y * 1.8759675015077200) + (z * 0.041555057407175);
	b = (x * 0.055630079696993) + (y * -0.203976958888970) + (z * 1.056971514242878);

	//Gamma correction
	if (r > 0.0031308)
	{
		r = ((1.055 * pow(r, 1.0 / 2.4)) - 0.055);
	}
	else
	{
		r = (r * 12.92);
	}
	if (g > 0.0031308)
	{
		g = ((1.055 * pow(g, 1.0 / 2.4)) - 0.055);
	}
	else
	{
		g = (g * 12.92);
	}
	if (b > 0.0031308)
	{
		b = ((1.055 * pow(b, 1.0 / 2.4)) - 0.055);
	}
	else
	{
		b = (b * 12.92);
	}

	r = min(max(0, r), 1);
	g = min(max(0, g), 1);
	b = min(max(0, b), 1);

	return float3(r, g, b);
}

//...

float3 FromLMS(float3 input)
{
	float l = input[0], m = input[1], s = input[2];

	float3 a = { +1.096123820835514, -0.278869000218287, +0.182745179382773 };
	float3 b = { +0.454369041975359, +0.473533154307412, +0.072097803717229 };
	float3 c = { -0.009627608738429, -0.005698031216113, +1.015325639954543 };

	float x = l * a[0] + m * a[1] + s * a[2];
	float y = l * b[0] + m * b[1] + s * b[2];
	float z = l * c[0] + m * c[1] + s * c[2];
	return FromXYZ(float3(x, y, z));
}

float3 FromUCS(float3 input)
{
	float u = input[0], v = input[1], w = input[2];
	return FromXYZ(float3(1.5 * u, v, 1.5 * u - 3 * v + 2 * w));
}

float3 FromUVW(float3 input)
{
	float x, y, z;
	x = input[0] / 2 / 3;
	y = input[1];
	z = (input[2] * 2) - (y * 3) + x;
	return FromXYZ(float3(x, y, z));
}

float3 FromxyY(float3 input)
{
	float x = input[0]; float y = input[1]; float Y = input[2];
	if (y == 0)
	{
		return float3(0, 0, 0);
	}
	return FromXYZ(float3(x * Y / y, Y, (1 - x - y) * Y / y));
}

//...

float3 FromLAB(float3 input)
{
	float3 xyz = float3(0, 0, 0);

	xyz[1] = (input[0] + 16.0f) / 116.0f;
	xyz[0] = (input[1] / 500.0f) + xyz[1];
	xyz[2] = xyz[1] - (input[2] / 200.0f);

	for (int i = 0; i < 3; i++)
	{
		float pow = xyz[i] * xyz[i] * xyz[i];
		float ratio = (6.0f / 29.0f);
		if (xyz[i] > ratio)
		{
			xyz[i] = pow;
		}
		else
		{
			xyz[i] = (3.0f * (6.0f / 29.0f) * (6.0f / 29.0f) * (xyz[i] - (4.0f / 29.0f)));
		}
	}

	xyz[0] = xyz[0] * D65[0];
	xyz[1] = xyz[1] * D65[1];
	xyz[2] = xyz[2] * D65[2];
	return FromXYZ(xyz);
}

float3 FromLABh(float3 input)
{
	float l = input[0]; float a = input[1]; float b = input[2];

	float _y = l / 10;
	float _x = a / 17.5 * l / 10;
	float _z = b / 7 * l / 10;

	float y = _y * _y;
	float x = (_x + y) / 1.02;
	float z = -(_z - y) / 0.847;

	return FromXYZ(float3(x, y, z));
}

//...

/*
float3 FromLUV(float3 input)
{
	float _u, _v, l, u, v, x, y, z, xn, yn, zn, un, vn;
	l = input[0], u = input[1], v = input[2];

	if (l == 0)
		return float3(0, 0, 0);

	//get constants
	//var e = 0.008856451679035631; //(6/29)^3
	float k = 0.0011070564598794539; //(3/29)^3

	//get illuminant/observer
	xn = D65[0];
	yn = D65[1];
	zn = D65[2];

	un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
	vn = (9 * yn) / (xn + (15 * yn) + (3 * zn));
	// un = 0.19783000664283;
	// vn = 0.46831999493879;

	_u = u / (13 * l) + un;
	_v = v / (13 * l) + vn;

	if (l > 8)
	{
		y = yn * pow((l + 16) / 116, 3);
	}
	else
	{
		y = yn * l * k;
	}

	//wikipedia method
	x = y * 9 * _u / (4 * _v);
	z = y * (12 - 3 * _u - 20 * _v) / (4 * _v);

	//boronine method
	//https://github.com/boronine/husl/blob/master/husl.coffee#L201
	// x = 0 - (9 * y * _u) / ((_u - 4) * _v - _u * _v);
	// z = (9 * y - (15 * _v * y) - (_v * x)) / (3 * _v);

	return FromXYZ(float3(x, y, z));
}
*/

//[*] http://www.brucelindbloom.com/index.html?Eqn_Luv_to_XYZ.html
float3 FromLUV(float3 input)
{
	float l = input[0], u = input[1], v = input[2];

	float ONE_OVER_KAPPA = 27.0 / 24389.0;
	float WHITE_U_PRIME = 0.1978330369967827;
	float WHITE_V_PRIME = 0.4683304743525223;

	if (l <= 0.0)
	{
		return float3(0, 0, 0);
	}

	float ll = 13.0 * l;
	float u_prime = ll == 0 ? 0 : u / ll + WHITE_U_PRIME;
	float v_prime = ll == 0 ? 0 : v / ll + WHITE_V_PRIME;

	float y;
	if (l > 8.0)
	{
		y = pow((l + 16.0) / 116.0, 3);
	}
	else 
	{
		y = l * ONE_OVER_KAPPA;
	}

	float a = 0.75 * y * u_prime / v_prime;
	float x = 3.0 * a;
	float z = y * (3.0 - 5.0 * v_prime) / v_prime - a;

	float3 xyz = float3(x, y, z);
	xyz = ConvertRange(xyz, Minimums(20), Maximums(20));
	return FromXYZ(xyz);
}

//[*] 
float3 FromHPLuv(float3 input)
{   
	/*
	//HPLuv -> LCHuv
	LCHuv lchuv = null;

	double c = this[0], b = this[1], a = this[2];
	if (99.9999999 < a)
	{
		lchuv = new(100, 0, c);
	}
	else if (1E-8 > a)
	{
		lchuv = new(0, 0, c);
	}
	else
	{
		b = M(a) / 100 * b;
		lchuv = new LCHuv(a, b, c);
	}

	//LCHuv -> RGB
	return lchuv.Convert();
	*/
	return FromUVW(input);
}

//[*] 
float3 FromHSLuv(float3 input)
{   
	/*
	//HSLuv -> LCHuv
	LCHuv lchuv = null;

	double c = this[0], b = this[1], a = this[2];
	if (99.9999999 < a)
	{
		lchuv = new(100, 0, c);
	}
	else if (1E-8 > a)
	{
		lchuv = new(0, 0, c);
	}
	else
	{
		b = N(a, c) / 100 * b;
		lchuv = new(a, b, c);
	}

	//LCH -> RGB
	return lchuv.Convert();
	*/
	return FromUVW(input);
}

//...

float3 FromLCHab(float3 input)
{
	float l = input[0]; float c = input[1]; float h = input[2];
	float a; float b; float hr;

	hr = h / 359 * 2 * pi;
	a = c * cos(hr);
	b = c * sin(hr);

	return FromLAB(float3(l, a, b));
}

//[*]
float3 FromLCHuv(float3 input)
{
	float l = input[0]; float c = input[1]; float h = input[2];
	float a; float b; float hr;

	hr = h / 359 * 2 * pi;
	a = c * cos(hr);
	b = c * sin(hr);

	return FromLAB(float3(l, a, b));
}

//... My own experimental derivatives (:

//HUV
float3 ToHUV(float3 input)
{
	float l = input[0]; float c = input[1]; float h = input[2];
	float s; float b;

	l = l / 100 * 50 + 50;
	c = c / 100 * 25 + 75;

	float hr = h / 359 * 2 * pi;
	float cr = c / 100 * 2 * pi;
	float lr = l / 100 * 2 * pi;

	h = 359 - (((l / 100) * (c / 100) * h) % 359);

	s = c * sin(cr);
	s = (s + 140) / (122 + 140) * 100 * 1.5;

	b = l * cos(lr);
	b = (b + 134) / (224 + 134) * 100 * 1.5;
	return float3(h, s, b / 100 * 255);
}

//HUV : HCV
float3 FromHUVcv(float3 input)
{
	float3 huv = ToHUV(input);
	huv[2] = huv[2] / 255 * 100;
	return FromHCV(huv);
}

//HUV : HCY
float3 FromHUVcy(float3 input)
{
	float3 huv = ToHUV(input);
	return FromHCY(huv);
}

//HUV : HSP
float3 FromHUVsp(float3 input)
{
	float3 huv = ToHUV(input);
	return FromHSP(huv);
}

//...

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy);

	//[0, 1]
	float x, y, z;

	//XY
	if (Mode == 0)
	{
		x = uv.xy.x;
		y = 1 - uv.xy.y;
		z = Z;
	}
	//Z
	if (Mode == 1)
	{
		x = X;
		y = Y;
		z = 1 - uv.xy.y;
	}

	float3 output; float3 input;
	
	float c = Component;
	float m = Model;

	if (c == 0)
	{
		input[0] = z;
		input[1] = x;
		input[2] = y;
	}
	else if (c == 1)
	{
		input[0] = x;
		input[1] = z;
		input[2] = y;
	}
	else if (c == 2)
	{
		input[0] = x;
		input[1] = y;
		input[2] = z;
	}
	
	float3 maximums = Maximums(m);
	float3 minimums = Minimums(m);
	input = ConvertRange(input, minimums, maximums);
	
	//RGB
	if (m == 0)
	{
	    output = input;
	}
	//HCV
	else if (m == 1)
	{
	    output = FromHCV(input);
	}
	//HCY
	else if (m == 2)
	{
		output = FromHCY(input);
	}
	//HPLuv
	else if (m == 3)
	{
		output = FromHPLuv(input);
	}
	//HSB
	else if (m == 4)
	{
	    output = FromHSB(input);
	}
	//HSL
	else if (m == 5)
	{
		output = FromHSL(input);
	}
	//HSLuv
	else if (m == 6)
	{
		output = FromHSLuv(input);
	}
	//HSP
	else if (m == 7)
	{
		output = FromHSP(input);
	}
	//HWB
	else if (m == 8)
	{
		output = FromHWB(input);
	}
	//LAB
	else if (m == 9)
	{
		output = FromLAB(input);
	}
	//LABh
	else if (m == 10)
	{
		output = FromLABh(input);
	}
	//LCHab
	else if (m == 11)
	{
		output = FromLCHab(input);
	}
	//LCHuv
	else if (m == 12)
	{
		output = FromLCHuv(input);
	}
	//LMS
	else if (m == 13)
	{
		output = FromLMS(input);
	}
	//LUV
	else if (m == 14)
	{
		output = FromLUV(input);
	}
	//TSL
	else if (m == 15)
	{
		output = FromTSL(input);
	}
	//HUVcv
	else if (m == 16)
	{
		output = FromHUVcv(input);
	}
	//HUVcy
	else if (m == 17)
	{
		output = FromHUVcy(input);
	}
	//HUVsp
	else if (m == 18)
	{
		output = FromHUVsp(input);
	}
	//UCS
	else if (m == 19)
	{
		output = FromUCS(input);
	}
	//UVW
	else if (m == 20)
	{
		output = FromUVW(input);
	}
	//xyY
	else if (m == 21)
	{
		output = FromxyY(input);
	}
	//XYZ
	else if (m == 22)
	{
		output = FromXYZ(input);
	}

	color.r = output[0];
	color.g = output[1];
	color.b = output[2];
	return color;
}