sampler2D TexSampler : register(S0);

//X-, Y-, Z- = [1, 0.95, 0.82] 
float X0 : register(C0);
float Y0 : register(C1);
float Z0 : register(C2);

float X1 : register(C3);
float Y1 : register(C4);
float Z1 : register(C5);

float X2 : register(C6);
float Y2 : register(C7);
float Z2 : register(C8);

float Amount : register(C9);

float FLerp(float norm, float min, float max)
{
    return (max - min) * norm + min;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(TexSampler, uv);

    float r = color.r, g = color.g, b = color.b;

    float nr = (r * clamp(X0, 0, 1)) + (g * clamp(Y0, 0, 0.95)) + (b * clamp(Z0, 0, 0.82));
    float ng = (r * clamp(X1, 0, 1)) + (g * clamp(Y1, 0, 0.95)) + (b * clamp(Z1, 0, 0.82));
    float nb = (r * clamp(X2, 0, 1)) + (g * clamp(Y2, 0, 0.95)) + (b * clamp(Z2, 0, 0.82));
    
    nr = FLerp(Amount, r, nr);
    ng = FLerp(Amount, g, ng);
    nb = FLerp(Amount, b, nb);

    color.r = nr;
    color.g = ng;
    color.b = nb;
    return color;
}
