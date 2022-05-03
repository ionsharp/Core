sampler2D input : register(s0);

float Red : register(C0);
float Green : register(C1);
float Blue : register(C2);
float Cyan : register(C3);
float Yellow : register(C4);
float Magenta : register(C5);

float HighlightAmount : register(C6);
float HighlightRange : register(C7);

float MidtoneAmount : register(C8);
float MidtoneRange : register(C9);

float ShadowAmount : register(C10);
float ShadowRange : register(C11);

float Target : register(C12);

//...

float3 RgbToHsl(float3 input)
{
    float maximum = max(max(input[0], input[1]), input[2]);
    float minimum = min(min(input[0], input[1]), input[2]);

    float chroma = maximum - minimum;

    float h = 0, s = 0, l = (maximum + minimum) / 2.0;

    if (chroma != 0)
    {
        s
            = l < 0.5
            ? chroma / (2.0 * l)
            : chroma / (2.0 - 2.0 * l);

        if (input[0] == maximum)
        {
            h = (input[1] - input[2]) / chroma;
            h = input[1] < input[2]
                ? h + 6.0
                : h;
        }
        else if (input[2] == maximum)
        {
            h = 4.0 + ((input[0] - input[1]) / chroma);
        }
        else if (input[1] == maximum)
            h = 2.0 + ((input[2] - input[0]) / chroma);

        h *= 60;
    }

    float3 result = { h, s, l };
    return result;
}

float3 HslToRgb(float3 input)
{
    float h = input[0], s = input[1], l = input[2];
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

float3 RgbToHsb(float3 input)
{
    float r = input[0], g = input[1], b = input[2];

    float minimum = min(input[0], min(input[1], input[2]));
    float maximum = max(input[0], max(input[1], input[2]));

    float chroma = maximum - minimum;

    float _h = 0.0;
    float _s = 0.0;
    float _b = maximum;

    if (chroma == 0)
    {
        _h = 0;
        _s = 0;
    }
    else
    {
        _s = chroma / maximum;

        if (input[0] == maximum)
        {
            _h = (input[1] - input[2]) / chroma;
            _h = input[1] < input[2] ? _h + 6 : _h;
        }
        else if (input[1] == maximum)
        {
            _h = 2.0 + ((input[2] - input[0]) / chroma);
        }
        else if (input[1] == maximum)
            _h = 4.0 + ((input[0] - input[1]) / chroma);

        _h *= 60;
    }

    float3 result = { _h, _s * 100, _b * 100 };
    return result;
}

float3 HsbToRgb(float3 input)
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

//...

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(input, uv.xy);

    float3 rgb = { color.r, color.g, color.b };
    float3 hsb = RgbToHsl(color.rgb);

    //[0] Color
    if (Target == 0)
    {
        float oldValue = hsb[1];

        //Yellow
        if (color.r > color.b && color.g > color.b)
        {
            hsb[1] += (oldValue * Yellow / 100);
        }
        //Magenta
        else if (color.r > color.g && color.b > color.g)
        {
            hsb[1] += (oldValue * Magenta / 100);
        }
        //Cyan
        else if (color.g > color.r && color.b > color.r)
        {
            hsb[1] += (oldValue * Cyan / 100);
        }

        //Red
        if (color.r > color.g && color.r > color.b)
        {
            hsb[1] += (oldValue * Red / 100);
        }
        //Green
        else if (color.g > color.r && color.g > color.b)
        {
            hsb[1] += (oldValue * Green / 100);
        }
        //Blue
        else if (color.b > color.r && color.b > color.g)
        {
            hsb[1] += (oldValue * Blue / 100);
        }
    }
    //[1] Threshold
    else if (Target == 1)
    {
        float oldValue = hsb[1];

        float threshold = Threshold / 100;
        float thresholdAmount = ThresholdAmount / 100;

        float absoluteThreshold = abs(threshold);

        if ((threshold > 0 && oldValue > absoluteThreshold) || (threshold < 0 && oldValue < absoluteThreshold))
        {
            hsb[0] = hsb[0];
            hsb[1] = oldValue + (oldValue * thresholdAmount);
            hsb[2] = hsb[2];
        }
        else
        {
            oldValue += (oldValue / (1 - absoluteThreshold)) * thresholdAmount;
            hsb[0] = hsb[0];
            hsb[1] = oldValue;
            hsb[2] = hsb[2];
        }
    }
    //[2] Tone
    else if (Target == 2)
    {
        //Shadow
        if (hsb[2] <= ShadowRange)
        {
            hsb[1] += hsb[1] * ShadowAmount;
        }
        //Midtone
        else if (hsb[2] <= MidtoneRange)
        {
            hsb[1] += hsb[1] * MidtoneAmount;
        }
        //Highlight
        else if (hsb[2] <= HighlightRange)
        {
            hsb[1] += hsb[1] * HighlightAmount;
        }

    }
    float3 result = HslToRgb(hsb);
    color.r = result[0]; color.g = result[1]; color.b = result[2];
    return color;
}