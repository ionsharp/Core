sampler2D Texture1Sampler : register(S0);

float4 Color1 : register(C0);
float4 Color2 : register(C1);

float Tolerance : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color = tex2D(Texture1Sampler, uv);
   
   float4 color1 = Color1;
   float4 color2 = Color2;
   
   if (all(abs(color.rgba - color1.rgba) < Tolerance)) 
      color.rgba = color2.rgba;

   return color;
}