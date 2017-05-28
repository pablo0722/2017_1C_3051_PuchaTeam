//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

								   //Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};










/******************************************************************************************/
/*                                      vvvvv VARIABLES vvvvv
/******************************************************************************************/
float time = 0;
float4 CameraPos;

// variable de fogs
float4 FogColor;
float FogStartDistance;
float FogEndDistance;
float FogDensity;
/******************************************************************************************/
/*                                      ^^^^^ VARIABLES ^^^^^
/******************************************************************************************/










//Input del Vertex Shader
struct VS_INPUT_VERTEX
{
	float4 Position :	POSITION0;
	float4 Color :		COLOR0;
	float1 Fog:			FOG;
	float2 Texture :	TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_VERTEX
{
	float4 Position :	POSITION0;
	float2 Texture:		TEXCOORD0;
	float1 Fog:			FOG;
	float4 Color :		COLOR0;
};

//Vertex Shader
VS_OUTPUT_VERTEX vs_main(VS_INPUT_VERTEX input)
{
	VS_OUTPUT_VERTEX output;


	//Propago Color
	output.Color = input.Color;

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);
	output.Texture = input.Texture;
	float4 CameraPosWorld = mul(CameraPos, matWorld);

	//calcula fog Exponencial
	float DistFog = distance(input.Position.xyz, CameraPosWorld.xyz);
	output.Fog = saturate(exp((FogStartDistance - DistFog)*FogDensity));

	return output;
}

//Pixel Shader
float4 ps_main(VS_OUTPUT_VERTEX input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);
	// combino fog y textura
	float4 fogFactor = float4(input.Fog,input.Fog,input.Fog,input.Fog);
	float4 fvFogColor = (1.0 - fogFactor) * FogColor;
	return fogFactor * fvBaseColor + fvFogColor;
}

// ------------------------------------------------------------------
technique RenderComun
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_main();
	}
}


//Vertex Shader
VS_OUTPUT_VERTEX vs_mainGirar(VS_INPUT_VERTEX input)
{
	VS_OUTPUT_VERTEX output;

	// Animar posicion
	float X = input.Position.x;
	float Z = input.Position.z;
	input.Position.x = X * cos(time) - Z * sin(time);
	input.Position.z = Z * cos(time) + X * sin(time);

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	float4 CameraPosWorld = mul(CameraPos, matWorld);

	//calcula fog Exponencial
	float DistFog = distance(input.Position.xyz, CameraPosWorld.xyz);
	output.Fog = saturate(exp((FogStartDistance - DistFog)*FogDensity));

	// Animar color
	input.Color.r = abs(sin(2 * time));
	input.Color.b = abs(sin(3 * time));

	//Propago el color x vertice
	output.Color = input.Color;

	return output;
}

//Pixel Shader
float4 ps_mainGirar(VS_OUTPUT_VERTEX input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);
	// combino fog y textura
	// combino fog y textura
	float4 fogFactor = float4(input.Fog, input.Fog, input.Fog, input.Fog);
	float4 fvFogColor = (1.0 - fogFactor) * FogColor;

	return 0.25*(fogFactor * fvBaseColor + fvFogColor) + 0.75*input.Color;
}

// ------------------------------------------------------------------
technique RenderGirar
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_mainGirar();
		PixelShader = compile ps_3_0 ps_mainGirar();
	}
}