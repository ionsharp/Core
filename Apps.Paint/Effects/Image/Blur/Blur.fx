//https://stackoverflow.com/questions/11282394/what-kind-of-blurs-can-be-implemented-in-pixel-shaders
sampler2D input : register(s0);

float Bias : register(C0);
float Offset : register(C1);
float Weight : register(C2);

float X0 : register(C3);
float Y0 : register(C4);
float Z0 : register(C5);

float X1 : register(C6);
float Y1 : register(C7);
float Z1 : register(C8);

float X2 : register(C9);
float Y2 : register(C10);
float Z2 : register(C11);

float4 main(float2 uv : TEXCOORD) : COLOR
{ 
	float4 color = tex2D(input, uv.xy); 

    float4 result = float4(0, 0, 0, 0);
    float2 offset = Offset;

    float2 start = uv.xy - offset;
    float2 current = start;

    float conMatrix[9] = { X0, Y0, Z0, X1, Y1, Z1, X2, Y2, Z2 };

    for (int i = 0; i < 9; i++)
    {
        result += tex2D(input, current) * conMatrix[i];
        current.x += Offset;
        if (i == 2 || i == 5) 
        {
            current.x = start.x;
            current.y += Offset;
        }
    }

    float4 newResult = result * Weight + Bias;
    newResult.a = color.a;
    return newResult;
}