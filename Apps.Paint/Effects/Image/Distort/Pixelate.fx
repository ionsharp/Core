/// <summary>The amount to shift alternate rows (use 1 to get a brick wall look).</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0</defaultValue>
float Offset : register(C0);

/// <summary>The number of horizontal and vertical pixel blocks.</summary>
/// <type>Size</type>
/// <minValue>20,20</minValue>
/// <maxValue>100,100</maxValue>
/// <defaultValue>60,40</defaultValue>
float2 Size : register(C1);

sampler2D Texture1Sampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float2 brickSize = 1.0 / Size;

   // Offset every other row of bricks
   float2 offsetuv = uv;
   bool oddRow = floor(offsetuv.y / brickSize.y) % 2.0 >= 1.0;
   if (oddRow)
   {
       offsetuv.x += Offset * brickSize.x / 2.0;
   }
   
   float2 brickNum = floor(offsetuv / brickSize);
   float2 centerOfBrick = brickNum * brickSize + brickSize / 2;
   float4 color = tex2D(Texture1Sampler, centerOfBrick);

   return color;
}


