sampler2D input : register(s0);

float Red : register(C0);
float Green : register(C1);
float Blue : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 

    float or = color.r * 255, og = color.g * 255, ob = color.b * 255;
    float nr = or, ng = og, nb = ob;

    nr = Red > or
        ? Red - or
        : or - Red;
    ng = Green > og
        ? Green - og
        : og - Green;
    nb = Blue > ob
        ? Blue - ob
        : ob - Blue;

	color.r = clamp(nr / 255, 0, 1);
	color.g = clamp(ng / 255, 0, 1);
	color.b = clamp(nb / 255, 0, 1);
	return color;
}