sampler2D input : register(s0);

float Levels : register(C0);
float Size : register(C1) = 3;

static int Levelss = 100;

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
    float x = uv.x; float y = uv.y;

    float filterOffset = (3 - 1) / 2;

    int currentIntensity = 0;
    int maxIndex = 0;
    
    float maxIntensity = 0;

    float red = 0, green = 0, blue = 0;

    int intensityBin[50];
    int blueBin[50];
    int greenBin[50];
    int redBin[50];

    int start = -filterOffset;
    
    int count = (filterOffset % 1023) * 2;
    for (int filterY = 0; filterY < count; filterY++)
    {
        for (int filterX = 0; filterX < count; filterX++)
        {
            float x2 = x + filterX - filterOffset;
            float y2 = y + filterY - filterOffset;

            if (x2 >= 0 && x2 < 1 && y2 >= 0 && y2 < 1)
            {
                float4 c = tex2D(input, float2(x2, y2)) * 255;
                currentIntensity = (int)((((c.r + c.g + c.b) / 3.0) * 50) / 255.0);
                /*
                The following error prevents progressing with shader:
                
                    error X3500: array reference cannot be used as an l-value; not natively addressable
                
                ...

                intensityBin[currentIntensity] = intensityBin[currentIntensity] + 1;

                redBin[currentIntensity] += c.r;
                greenBin[currentIntensity] += c.g;
                blueBin[currentIntensity] += c.b;

                if (intensityBin[currentIntensity] > maxIntensity)
                {
                    maxIntensity = intensityBin[currentIntensity];
                    maxIndex = currentIntensity;
                }
                */
            }
            if (filterX - filterOffset > filterOffset)
                break;
        }
        if (filterY - filterOffset > filterOffset)
            break;
    }

    /*
    red = redBin[maxIndex] / maxIntensity;
    green = greenBin[maxIndex] / maxIntensity;
    blue = blueBin[maxIndex] / maxIntensity;
    */

    //Are these values in range [0, 1]?
    color.r = clamp(red / 255, 0, 1);
    color.g = clamp(green / 255, 0, 1);
    color.b = clamp(blue / 255, 0, 1);
    return color;
}