sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
 
    float4 outputColor = color;
	outputColor.r = color.r - (color.r / 4);
	outputColor.g = color.g - (color.g / 4);
    outputColor.b = (color.r * 0.272) + (color.g * 0.534) + (color.b * 0.231) - (color.b / 4);
 
    return outputColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}