sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float4 outputColor = color;
	outputColor.r = color.r * 0.5f - color.g * 0.25f;
	outputColor.g = color.g * 0.6f - color.r * 0.3f;
    outputColor.b = color.b * 0.6f;
 
    return outputColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}