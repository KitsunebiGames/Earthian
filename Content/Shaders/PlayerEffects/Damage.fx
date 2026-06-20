sampler s0;
float damagetime = 0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	if (damagetime > 0.01f)
	color.r = color.r + (damagetime * 2);
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}