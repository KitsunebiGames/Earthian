sampler s0;
float2 taken;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
		float2 amnt;
		amnt.x = taken.x / 100;
		amnt.y = taken.y / 100;
		coords.x = amnt.x;
		coords.y = amnt.y;
		float4 color = tex2D(s0, coords);
		return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}