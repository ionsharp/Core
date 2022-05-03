sampler2D input : register(s0);

float Threshold : register(C0);

float Coerce(float input, float maximum, float minimum = 0)
{
	return max(min(maximum, input), minimum);
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D( input , uv.xy); 
    float currentRed = color.r * 255, currentGreen = color.g * 255, currentBlue = color.b * 255;

    float NumAreas = 256 / Threshold;
    float NumValues = 255 / (Threshold - 1);

    float redAreaFloat = currentRed / NumAreas;
    int redArea = (int)redAreaFloat;
    
    if (redArea > redAreaFloat) 
    	redArea = redArea - 1;
    
    float newRedFloat = NumValues * redArea;
    float newRed = (int)newRedFloat;
    
    if (newRed > newRedFloat) 
    	newRed = newRed - 1;

    float greenAreaFloat = currentGreen / NumAreas;
    int greenArea = (int)greenAreaFloat;
    
    if (greenArea > greenAreaFloat) 
    	greenArea = greenArea - 1;
    	
    float newGreenFloat = NumValues * greenArea;
    float newGreen = (int)newGreenFloat;
    
    if (newGreen > newGreenFloat) 
    	newGreen = newGreen - 1;

    float blueAreaFloat = currentBlue / NumAreas;
    int blueArea = (int)blueAreaFloat;
    
    if (blueArea > blueAreaFloat) 
    	blueArea = blueArea - 1;
    
    float newBlueFloat = NumValues * blueArea;
    float newBlue = (int)newBlueFloat;
    
    if (newBlue > newBlueFloat) 
    	newBlue = newBlue - 1;

	color.r = Coerce(newRed / 255, 1);
	color.g = Coerce(newGreen / 255, 1);
	color.b = Coerce(newBlue / 255, 1);
	return color;
}