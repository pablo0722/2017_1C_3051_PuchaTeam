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

//Textura para Lightmap
texture texLightMap;
sampler2D lightMap = sampler_state
{
	Texture = (texLightMap);
};










/******************************************************************************************/
/*                                      vvvvv CONSTANTES vvvvv
/******************************************************************************************/
#define SMAP_SIZE 1024
#define EPSILON 0.05f
#define PI 3.1415926535897932384626433832795
/******************************************************************************************/
/*                                      ^^^^^ CONSTANTES ^^^^^
/******************************************************************************************/



/******************************************************************************************/
/*                                      vvvvv VARIABLES vvvvv
/******************************************************************************************/
float time = 0;
float frameTime = 0;
float time2 = 0;
float4 CameraPos;

// Variables de fogs
	float4 FogColor;
	float FogStartDistance;
	float FogEndDistance;
	float FogDensity;

// Variables de ShadowMap
	float4x4 g_mViewLightProj;
	float4x4 g_mProjLight;
	float3   g_vLightPos;  // posicion de la luz (en World Space) = pto que representa patch emisor Bj
	float3   g_vLightDir;  // Direcion de la luz (en World Space) = normal al patch Bj
	texture  g_txShadow;	// textura para el shadow map
	sampler2D g_samShadow =
	sampler_state
	{
		Texture = <g_txShadow>;
		MinFilter = Point;
		MagFilter = Point;
		MipFilter = Point;
		AddressU = Clamp;
		AddressV = Clamp;
	};

// Variables de la luz de explosion
	float ExplosionLightOn; //Color RGB de las 4 luces
	float4 ExplosionLightPosition; //Posicion de las 4 luces
	float ExplosionLightAttenuation; //Factor de atenuacion de las 4 luces


// Efecto Explosion del Jalapeño ()
	float timeExplota = 0;

// Efecto Fuego del Jalapeño ()
	int fuegoGirar;
	texture texFuego;
	sampler2D FuegoMap = sampler_state
	{
		Texture = (texFuego);
		ADDRESSU = WRAP;
		ADDRESSV = WRAP;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
		MIPFILTER = LINEAR;
	};

	texture texFuegoAlpha;
	sampler2D FuegoAlphaMap = sampler_state
	{
		Texture = (texFuegoAlpha);
		ADDRESSU = WRAP;
		ADDRESSV = WRAP;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
		MIPFILTER = LINEAR;
	};

	texture texQuemado;
	sampler2D QuemadoMap = sampler_state
	{
		Texture = (texQuemado);
		ADDRESSU = WRAP;
		ADDRESSV = WRAP;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
		MIPFILTER = LINEAR;
	};

	texture texSuperNuez;
	sampler2D SuperNuezMap = sampler_state
	{
		Texture = (texSuperNuez);
		ADDRESSU = WRAP;
		ADDRESSV = WRAP;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
		MIPFILTER = LINEAR;
	};
/******************************************************************************************/
/*                                      ^^^^^ VARIABLES ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER COMUN vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT
{
	float4 Position :	POSITION0;
	float4 Color :		COLOR0;
	float1 Fog:			FOG;
	float2 Texture :	TEXCOORD0;
	float3 Normal :		NORMAL;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT
{
	float4 Position :		POSITION0;
	float2 Texture:			TEXCOORD0;
	float1 Fog:				FOG;
	float4 Color :			COLOR0;
	float4 WorldPosition :	TEXCOORD1;
	float3 WorldNormal :	TEXCOORD2;
	float4 PosLight :		TEXCOORD3;
};

// Vertex Shader
VS_OUTPUT vs_Comun(VS_INPUT input)
{
	VS_OUTPUT output;

	//Propago Color
	output.Color = input.Color;

	// propago la normal
	output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;

	// propago la textura
	output.Texture = input.Texture;

	// propago la posicion del vertice en World space
	output.WorldPosition = mul(input.Position, matWorld);

	// propago la posicion del vertice en el espacio de proyeccion de la luz
	output.PosLight = mul(output.WorldPosition, g_mViewLightProj);

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);
	float4 CameraPosWorld = mul(CameraPos, matWorld);

	//calcula fog Exponencial
	float DistFog = distance(input.Position.xyz, CameraPosWorld.xyz);
	output.Fog = saturate(exp((FogStartDistance - DistFog)*FogDensity));

	return output;
}

//Funcion para calcular color RGB de Diffuse
float3 computeDiffuseComponent(float3 surfacePosition, float3 N)
{
	//Calcular intensidad de luz, con atenuacion por distancia
	float distAtten = length(ExplosionLightPosition.xyz - surfacePosition);
	float3 Ln = (ExplosionLightPosition.xyz - surfacePosition) / distAtten;
	distAtten = distAtten * ExplosionLightAttenuation;
	float intensity = (sin(time2* 3.1415926535897932384 /3) * 10 + 1 ) / distAtten; //Dividimos intensidad sobre distancia

													 //Calcular Diffuse (N dot L)
	float3 ExplosionLightColor; //Color RGB de las 4 luces
	ExplosionLightColor.r = 1;
	ExplosionLightColor.gb = 1-sin(1.570796326794*(1+time2 / 3));
	return intensity * ExplosionLightColor.rgb * max(0.0, dot(N, Ln));
}

// Pixel Shader
float4 ps_Comun(VS_OUTPUT input) : COLOR0
{
	float3 vLight = normalize(float3(input.WorldPosition - g_vLightPos));
	float cono = dot(vLight, g_vLightDir);
	float4 K = 0.0;
	if (cono > 0.7)
	{
		// coordenada de textura CT
		float2 CT = 0.5 * input.PosLight.xy / input.PosLight.w + float2(0.5, 0.5);
		CT.y = 1.0f - CT.y;

		// sin ningun aa. conviene con smap size >= 512
		float I = (tex2D(g_samShadow, CT) + EPSILON < input.PosLight.z / input.PosLight.w) ? 0.0f : 1.0f;

		// interpolacion standard bi-lineal del shadow map
		// CT va de 0 a 1, lo multiplico x el tamaño de la textura
		// la parte fraccionaria indica cuanto tengo que tomar del vecino
		// conviene cuando el smap size = 256
		// leo 4 valores
		/*float2 vecino = frac( CT*SMAP_SIZE);
		float prof = vPosLight.z / vPosLight.w;
		float s0 = (tex2D( g_samShadow, float2(CT)) + EPSILON < prof)? 0.0f: 1.0f;
		float s1 = (tex2D( g_samShadow, float2(CT) + float2(1.0/SMAP_SIZE,0))
		+ EPSILON < prof)? 0.0f: 1.0f;
		float s2 = (tex2D( g_samShadow, float2(CT) + float2(0,1.0/SMAP_SIZE))
		+ EPSILON < prof)? 0.0f: 1.0f;
		float s3 = (tex2D( g_samShadow, float2(CT) + float2(1.0/SMAP_SIZE,1.0/SMAP_SIZE))
		+ EPSILON < prof)? 0.0f: 1.0f;
		float I = lerp( lerp( s0, s1, vecino.x ),lerp( s2, s3, vecino.x ),vecino.y);
		*/

		/*
		// anti-aliasing del shadow map
		float I = 0;
		float r = 2;
		for(int i=-r;i<=r;++i)
		for(int j=-r;j<=r;++j)
		I += (tex2D( g_samShadow, CT + float2((float)i/SMAP_SIZE, (float)j/SMAP_SIZE) ) + EPSILON < vPosLight.z / vPosLight.w)? 0.0f: 1.0f;
		I /= (2*r+1)*(2*r+1);
		*/

		if (cono < 0.8)
			I *= 1 - (0.8 - cono) * 10;

		K = I;
	}

	// LUZ DE LA EXPLOSION DE LA SUPER DE LA REPETIDORA
		float3 Nn = normalize(input.WorldNormal);

		//Emissive + Diffuse de 1 luz PointLight
		float3 diffuseLighting = computeDiffuseComponent(input.WorldPosition, Nn);
		diffuseLighting[0] = diffuseLighting[0] * ExplosionLightOn + 1 * (1 - ExplosionLightOn);
		diffuseLighting[1] = diffuseLighting[1] * ExplosionLightOn + 1 * (1 - ExplosionLightOn);
		diffuseLighting[2] = diffuseLighting[2] * ExplosionLightOn + 1 * (1 - ExplosionLightOn);


	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);
	fvBaseColor.rgb *= diffuseLighting;
	fvBaseColor.rgb *= 0.5 + 0.5*K;
	
	// combino fog y textura
	float4 fogFactor = float4(input.Fog,input.Fog,input.Fog,input.Fog);
	float4 fvFogColor = (1.0 - fogFactor) * FogColor;

	fvBaseColor.a = 1;
	fvFogColor.a = 1;
	fogFactor.a = 1;

	return (fogFactor * fvBaseColor) * 0.8 + (fvFogColor) * 0.2;
}

// TECNICA
technique TecnicaComun
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Comun();
		PixelShader = compile ps_3_0 ps_Comun();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER COMUN ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER SOMBRA (SHADOW) vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_SHADOWMAP
{
	float4 Position : POSITION;
	float3 Normal : NORMAL;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_SHADOWMAP
{
	float4 Position : POSITION;
	float2 Depth : TEXCOORD0;
};

// Vertex Shader
VS_OUTPUT_SHADOWMAP vs_ShadowMap(VS_INPUT_SHADOWMAP input)
{
	VS_OUTPUT_SHADOWMAP output;

	// transformacion estandard
	output.Position = mul(input.Position, matWorld);					// uso el del mesh
	output.Position = mul(output.Position, g_mViewLightProj);		// pero visto desde la pos. de la luz

																	// devuelvo: profundidad = z/w
	output.Depth.xy = output.Position.zw;

	return output;
}

// Pixel Shader
float4 ps_ShadowMap(VS_OUTPUT_SHADOWMAP input) : COLOR
{
	// parche para ver el shadow map
	//float k = Depth.x/Depth.y;
	//Color = (1-k);
	return input.Depth.x / input.Depth.y;
}

// TECNICA
technique TecnicaShadowMap
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_ShadowMap();
		PixelShader = compile ps_3_0 ps_ShadowMap();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER SOMBRA (SHADOW) ^^^^^
/******************************************************************************************/












/******************************************************************************************/
/*                                      vvvvv SHADER GIRAR vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_GIRAR
{
	float4 Position :	POSITION0;
	float4 Color :		COLOR0;
	float1 Fog:			FOG;
	float2 Texture :	TEXCOORD0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_GIRAR
{
	float4 Position :	POSITION0;
	float2 Texture:		TEXCOORD0;
	float1 Fog:			FOG;
	float4 Color :		COLOR0;
	float4 Pos3D :		TEXCOORD1;
	float4 PosLight :	TEXCOORD3;
};

// Vertex Shader
VS_OUTPUT_GIRAR vs_Girar(VS_INPUT_GIRAR input)
{
	VS_OUTPUT_GIRAR output;


	// propago la posicion del vertice en World space
	output.Pos3D = mul(input.Position, matWorld);

	// propago la posicion del vertice en el espacio de proyeccion de la luz
	output.PosLight = mul(output.Pos3D, g_mViewLightProj);

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

// Pixel Shader
float4 ps_Girar(VS_OUTPUT_GIRAR input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);
	// combino fog y textura
	float4 fogFactor = float4(input.Fog, input.Fog, input.Fog, input.Fog);
	float4 fvFogColor = (1.0 - fogFactor) * FogColor;

	fvBaseColor.a = 2;
	fvFogColor.a = 2;
	fogFactor.a = 2;
	input.Color.a = 0;

	return 0.25*(fogFactor * fvBaseColor + fvFogColor) + 0.75*input.Color;
}

// TECNICA
technique TecnicaGirar
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Girar();
		PixelShader = compile ps_3_0 ps_Girar();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER GIRAR ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER BOLA DE FUEGO vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_BOLA_DE_FUEGO
{
	float4 Position :	POSITION;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_BOLA_DE_FUEGO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_BOLA_DE_FUEGO vs_BolaDeFuego(VS_INPUT_BOLA_DE_FUEGO input)
{
	VS_OUTPUT_BOLA_DE_FUEGO output;


	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	float factor = fmod(abs(input.Position.z*(sin(time / 1.5))) + abs(input.Position.y*(sin(time / 1.7))), 2)/2;

	output.Color.r = factor;
	output.Color.g = 0;
	output.Color.b = 0;
	output.Color.a = 1;


	return output;
}

// Pixel Shader
float4 ps_BolaDeFuego(VS_OUTPUT_BOLA_DE_FUEGO input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	//float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	input.Color.a = 1;

	return input.Color;
}

// TECNICA
technique TecnicaBolaDeFuego
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_BolaDeFuego();
		PixelShader = compile ps_3_0 ps_BolaDeFuego();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER BOLA DE FUEGO ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER BOLA DE EXPLOSION vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_BOLA_DE_EXPLOSION
{
	float4 Position :	POSITION;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_BOLA_DE_EXPLOSION
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_BOLA_DE_EXPLOSION vs_BolaDeExplosion(VS_INPUT_BOLA_DE_EXPLOSION input)
{
	VS_OUTPUT_BOLA_DE_EXPLOSION output;


	//Proyectar posicion
	input.Position.x = input.Position.x * (1 + sin(time * 5) / 3);
	input.Position.y = input.Position.y * (1 + sin(time * 5) / 3);
	input.Position.z = input.Position.z * (1 + sin(time * 5) / 3);

	output.Position = mul(input.Position, matWorldViewProj);
	
	output.Texture = input.Texture;


	float factor = fmod(abs(input.Position.z*(sin(time / 1.5))) + abs(input.Position.y*(sin(time / 1.7))), 2) / 2;

	output.Color.r = factor;
	output.Color.g = factor;
	output.Color.b = 0;
	output.Color.a = 1;

	return output;
}

// Pixel Shader
float4 ps_BolaDeExplosion(VS_OUTPUT_BOLA_DE_EXPLOSION input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	//float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	input.Color.a = 1;

	return input.Color;
}

// TECNICA
technique TecnicaBolaDeExplosion
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_BolaDeExplosion();
		PixelShader = compile ps_3_0 ps_BolaDeExplosion();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER BOLA DE EXPLOSION ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER EXPLOSION vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_EXPLOSION
{
	float4 Position :	POSITION;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_EXPLOSION
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_EXPLOSION vs_Explosion(VS_INPUT_EXPLOSION input)
{
	VS_OUTPUT_EXPLOSION output;


	//Proyectar posicion
	input.Position.x = input.Position.x * exp(time2 / 1.1);
	input.Position.y = input.Position.y * exp(time2 / 1.1);
	input.Position.z = input.Position.z * exp(time2 / 1.1);

	float x = input.Position.x;
	float z = input.Position.z;
	input.Position.x = x * cos(time*2) - z * sin(time*2);
	input.Position.z = z * cos(time*2) + x * sin(time*2);

	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	//Propago el color x vertice
	output.Color = input.Color;

	return output;
}

// Pixel Shader
void ps_Explosion(VS_OUTPUT_EXPLOSION input, out float4 o_color  : COLOR0)
{
	// Animar color
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	input.Color.r = fvBaseColor.r * abs(cos(time2*1.5707963267948966192313216916398/4));
	input.Color.g = fvBaseColor.g * abs(cos(time2*1.5707963267948966192313216916398/4));
	input.Color.b = 0;
	input.Color.a = abs(cos(time2*1.5707963267948966192313216916398/3));

	//Propago el color x vertice
	o_color = input.Color;
}

// TECNICA
technique TecnicaExplosion
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Explosion();
		PixelShader = compile ps_3_0 ps_Explosion();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER EXPLOSION ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER SUPER GIRASOL vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_SUPER_GIRASOL
{
	float4 Position :		POSITION;
	float2 Texture : TEXCOORD0;
	float4 Color : COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_SUPER_GIRASOL
{
	float4 Position :	POSITION0;
	float2 Texture : TEXCOORD0;
	float4 Color : COLOR0;
};

// Vertex Shader
VS_OUTPUT_SUPER_GIRASOL vs_SuperGirasol(VS_INPUT_SUPER_GIRASOL input)
{
	VS_OUTPUT_SUPER_GIRASOL output;


	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	//Propago el color x vertice
	output.Color = input.Color;


	float factor = abs(sin(time*5))+0.5;

	output.Color.r = factor;
	output.Color.g = factor;
	output.Color.b = 0;


	return output;
}

// Pixel Shader
float4 ps_SuperGirasol(VS_OUTPUT_SUPER_GIRASOL input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	fvBaseColor.a = 1;

	return fvBaseColor*0.5+input.Color*0.5;
}

// TECNICA
technique TecnicaSuperGirasol
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_SuperGirasol();
		PixelShader = compile ps_3_0 ps_SuperGirasol();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER SUPER GIRASOL ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER GIRAR SOL vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_GIRAR_SOL
{
	float4 Position :	POSITION0;
	float4 Color :		COLOR0;
	float2 Texture :	TEXCOORD0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_GIRAR_SOL
{
	float4 Position :		POSITION0;
	float2 Texture:			TEXCOORD0;
	float4 Color :			COLOR0;
};

VS_OUTPUT_GIRAR_SOL vs_GirarSol(VS_INPUT_GIRAR_SOL input)
{
	VS_OUTPUT_GIRAR_SOL output;

	// Animar posicion
	float y = input.Position.y;
	float Z = input.Position.z;
	input.Position.y = y * cos(time * 2) - Z * sin(time * 2);
	input.Position.z = Z * cos(time * 2) + y * sin(time * 2);

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	//Propago el color x vertice
	output.Color = input.Color;


	float factor = fmod(abs(input.Position.z*(sin(time / 1.5))) + abs(input.Position.y*(sin(time / 1.7))), 2) / 2;

	output.Color.r = factor;
	output.Color.g = 0;
	output.Color.b = factor;


	return output;
}

//Pixel Shader
float4 ps_GirarSol(VS_OUTPUT_GIRAR_SOL input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	fvBaseColor.a = 1 - saturate(time2 / 3);

	fvBaseColor.a = 1;

	return fvBaseColor;
}

technique RenderSol
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_GirarSol();
		PixelShader = compile ps_3_0 ps_GirarSol();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER GIRAR SOL ^^^^^
/******************************************************************************************/




/******************************************************************************************/
/*                                      vvvvv SHADER BOLA DE HIELO vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_BOLA_DE_HIELO
{
	float4 Position :	POSITION;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_BOLA_DE_HIELO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_BOLA_DE_HIELO vs_BolaDeHielo(VS_INPUT_BOLA_DE_HIELO input)
{
	VS_OUTPUT_BOLA_DE_HIELO output;


	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	float factor = fmod(abs(input.Position.z*(sin(time / 1.5))) + abs(input.Position.y*(sin(time / 1.7))), 2) / 2;

	output.Color.r = 0;
	output.Color.g = 0;
	output.Color.b = factor;
	output.Color.a = 1;


	return output;
}

// Pixel Shader
float4 ps_BolaDeHielo(VS_OUTPUT_BOLA_DE_HIELO input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	//float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	return input.Color;
}

// TECNICA
technique TecnicaBolaDeHielo
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_BolaDeHielo();
		PixelShader = compile ps_3_0 ps_BolaDeHielo();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER BOLA DE HIELO ^^^^^
/******************************************************************************************/




/******************************************************************************************/
/*                                      vvvvv SHADER Enfriar Zombie vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_ENFRIAR
{
	float4 Position :	POSITION;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_ENFRIAR
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_ENFRIAR vs_Enfriar(VS_INPUT_ENFRIAR input)
{
	VS_OUTPUT_ENFRIAR output;
	

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	output.Color.r = 0;
	output.Color.g = 0;
	output.Color.b = 0.5;
	output.Color.a = 1;


	return output;
}

// Pixel Shader
float4 ps_Enfriar(VS_OUTPUT_ENFRIAR input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	return fvBaseColor + input.Color;
}

// TECNICA
technique TecnicaEnfriar
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Enfriar();
		PixelShader = compile ps_3_0 ps_Enfriar();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER Enfriar Zombie ^^^^^
/******************************************************************************************/




/******************************************************************************************/
/*                                      vvvvv SHADER Congelar Zombie vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_CONGELAR
{
	float4 Position :	POSITION;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_CONGELAR
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_CONGELAR vs_Congelar(VS_INPUT_CONGELAR input)
{
	VS_OUTPUT_CONGELAR output;


	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	output.Color.r = 0;
	output.Color.g = 0;
	output.Color.b = 0.5;
	output.Color.a = 1;


	return output;
}

// Pixel Shader
float4 ps_Congelar(VS_OUTPUT_CONGELAR input) : COLOR0
{
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D(diffuseMap, input.Texture);

	return fvBaseColor + input.Color;
}

// TECNICA
technique TecnicaCongelar
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Congelar();
		PixelShader = compile ps_3_0 ps_Congelar();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER Enfriar Zombie ^^^^^
/******************************************************************************************/




/******************************************************************************************/
/*                                      vvvvv SHADER EXPLOTA JALAPENIO vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_EXPLOTA_JALAPENIO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_EXPLOTA_JALAPENIO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_EXPLOTA_JALAPENIO vs_ExplotaJalapenio(VS_INPUT_EXPLOTA_JALAPENIO input)
{
	VS_OUTPUT_EXPLOTA_JALAPENIO output;

	input.Position.x = input.Position.x * (1 + saturate(atan(timeExplota*1.5 - input.Position.y*0.4)));
	input.Position.z = input.Position.z * (1 + saturate(atan(timeExplota*1.5 - input.Position.y*0.4))) + 0.25 * sin(timeExplota*timeExplota*10);
	input.Position.y = input.Position.y * (1 + saturate(atan(timeExplota*1.5 - input.Position.y*0.4)));

	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	//Propago el color x vertice
	output.Color = input.Color;

	return output;
}

// Pixel Shader
float4 ps_ExplotaJalapenio(VS_OUTPUT_EXPLOTA_JALAPENIO input) : COLOR0
{
	float4 output;

	output = tex2D(diffuseMap, input.Texture);

	output.r = saturate(output.r + timeExplota*0.3);
	output.a = 1;

	return output;
}

// TECNICA
technique TecnicaExplotaJalapenio
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_ExplotaJalapenio();
		PixelShader = compile ps_3_0 ps_ExplotaJalapenio();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER EXPLOTA JALAPENIO ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER FUEGO vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_FUEGO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_FUEGO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_FUEGO vs_Fuego(VS_INPUT_FUEGO input)
{
	VS_OUTPUT_FUEGO output;

	float Y = input.Position.y / 50; // Y normalizada al tamaño maximo del objeto
	float frecuencia = 2;
	float fase = - Y * PI/2;
	float amplitud = Y*15;

	input.Position.z = input.Position.z + amplitud*sin(time*frecuencia + fase);

	// Girar si estoy en modo CamaraPersonal
	float X = input.Position.x; // Y normalizada al tamaño maximo del objeto
	float Z = input.Position.z; // Y normalizada al tamaño maximo del objeto
	input.Position.x = ( fuegoGirar ) * ( X * cos(PI/2) + Z * sin(PI/2) ) + (1 - fuegoGirar) * (X);
	input.Position.z = ( fuegoGirar ) * ( Z * cos(PI/2) - X * sin(PI / 2) ) + (1 - fuegoGirar) * (Z);

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	//Propago las coordenadas de textura
	output.Texture = input.Texture;

	//Propago el color x vertice
	output.Color = input.Color;

	return output;
}

// Pixel Shader
float4 ps_Fuego(VS_OUTPUT_FUEGO input) : COLOR0
{
	float4 output;

	output = tex2D(diffuseMap, input.Texture);

	float4 fuegoAlpha = tex2D(FuegoAlphaMap, input.Texture); // En FuegoAlphaMap los 3 canales tienen siempre el mismo valor (escala de grises)

	float frecuenciaY = abs(sin((time + input.Texture.y) * 5));

	output.a = saturate((fuegoAlpha.r*2.5 - 1.25) + frecuenciaY);

	return output;
}

// TECNICA
technique TecnicaFuego
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Fuego();
		PixelShader = compile ps_3_0 ps_Fuego();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER FUEGO ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER QUEMADO vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_QUEMADO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_QUEMADO
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_QUEMADO vs_Quemado(VS_INPUT_QUEMADO input)
{
	VS_OUTPUT_QUEMADO output = input;

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	return output;
}

// Pixel Shader
float4 ps_Quemado(VS_OUTPUT_QUEMADO input) : COLOR0
{
	float4 output;

	output = tex2D(diffuseMap, input.Texture);

	input.Texture[1] = input.Texture[1] + time;

	float4 fuego = tex2D(QuemadoMap, input.Texture); // En FuegoAlphaMap los 3 canales tienen siempre el mismo valor (escala de grises)

	output = output*0.25 + fuego*0.75;

	return output;
}

// TECNICA
technique TecnicaQuemado
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_Quemado();
		PixelShader = compile ps_3_0 ps_Quemado();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER QUEMADO ^^^^^
/******************************************************************************************/










/******************************************************************************************/
/*                                      vvvvv SHADER SUPER NUEZ vvvvv
/******************************************************************************************/
// Estructura Input del Vertex Shader
struct VS_INPUT_SUPER_NUEZ
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Estructura Output del Vertex Shader
struct VS_OUTPUT_SUPER_NUEZ
{
	float4 Position :	POSITION0;
	float2 Texture :	TEXCOORD0;
	float4 Color :		COLOR0;
};

// Vertex Shader
VS_OUTPUT_SUPER_NUEZ vs_SuperNuez(VS_INPUT_SUPER_NUEZ input)
{
	VS_OUTPUT_SUPER_NUEZ output = input;

	//Proyectar posicion
	output.Position = mul(input.Position, matWorldViewProj);

	return output;
}

// Pixel Shader
float4 ps_SuperNuez(VS_OUTPUT_SUPER_NUEZ input) : COLOR0
{
	float4 output = tex2D(SuperNuezMap, input.Texture); // En FuegoAlphaMap los 3 canales tienen siempre el mismo valor (escala de grises)

	return output;
}

technique TecnicaNuez
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_SuperNuez();
		PixelShader = compile ps_3_0 ps_SuperNuez();
	}
}
/******************************************************************************************/
/*                                      ^^^^^ SHADER SUPER NUEZ ^^^^^
/******************************************************************************************/