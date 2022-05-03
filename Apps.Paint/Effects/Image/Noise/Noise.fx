sampler2D input : register(s0);

float Amount : register(C0);
float Mode : register(C1);

//Gets random number in range [0,1) using texture coordinates as the random number seed.
//Author: Michael Pohoreski
//https://stackoverflow.com/questions/5149544/can-i-generate-a-random-number-inside-a-pixel-shader
float random(float2 p)
{
    //We need irrationals for pseudo randomness. Most (all?) known transcendental numbers will (generally) work.
    const float2 r = float2(
        23.1406926327792690,  // e^pi (Gelfond's constant)
        2.6651441426902251); // 2^sqrt(2) (Gelfond–Schneider constant)

    return frac(cos(fmod(123456789., 1e-7 + 256. * dot(p, r))));
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy);

    float2 newPoint = round(uv * 100) / 100;

    float rr = random(newPoint);
    float rg = random(newPoint);
    float rb = random(newPoint);

    float r = rr * Amount, g = rg * Amount, b = rb * Amount;

    //Add
    if (Mode == 0)
    {
        color.r = clamp(color.r + r, 0, 1);
        color.g = clamp(color.g + g, 0, 1);
        color.b = clamp(color.b + b, 0, 1);
    }
    //Subtract
    else if (Mode == 1)
    {
        color.r = clamp(color.r - r, 0, 1);
        color.g = clamp(color.g - g, 0, 1);
        color.b = clamp(color.b - b, 0, 1);
    }
    //Add | Subtract
    else if (Mode == 2)
    {
        color.r = clamp(color.r + (rr > 0.5 ? r : -r), 0, 1);
        color.g = clamp(color.g + (rg > 0.5 ? g : -g), 0, 1);
        color.b = clamp(color.b + (rb > 0.5 ? b : -b), 0, 1);
    }
    return color;
}