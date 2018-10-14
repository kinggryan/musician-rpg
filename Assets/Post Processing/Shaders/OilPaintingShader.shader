// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/OilPainting"
{
// 	HLSLINCLUDE

// 		#include "PostProcessing/Shaders/StdLib.hlsl"

// 		// TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
// 		float _Radius;
 
// 		struct Varyings
// 		{
// 			float4 vertex : SV_POSITION;
// 			float2 texcoord : TEXCOORD0;
// 			float2 texcoordStereo : TEXCOORD5;
// 		};

// 		// float4 _MainTex_ST;

// 		Varyings vert(VaryingsDefault v) {
// 			Varyings o;
// 			// o.vertex = v.vertex;
// 			o.vertex = float4(v.vertex.xy, 0.0, 1.0);
// 			// o.vertex = mul(unity_MatrixVP, v.vertex); 
// 			o.texcoord = v.texcoord;
// 			// o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
// 			return o;
// 		}

// 		// float4 _MainTex_TexelSize;

// 		float4 frag (Varyings i) : SV_Target
// 		{
// 			// half2 uv = i.uv;

// 			// float3 mean[4] = {
// 			//     {0, 0, 0},
// 			//     {0, 0, 0},
// 			//     {0, 0, 0},
// 			//     {0, 0, 0}
// 			// };

// 			// float3 sigma[4] = {
// 			//     {0, 0, 0},
// 			//     {0, 0, 0},
// 			//     {0, 0, 0},
// 			//     {0, 0, 0}
// 			// };

// 			// float2 start[4] = {{-_Radius, -_Radius}, {-_Radius, 0}, {0, -_Radius}, {0, 0}};

// 			// float2 pos;
// 			// float3 col;
// 			// for (int k = 0; k < 4; k++) {
// 			//     for(int i = 0; i <= _Radius; i++) {
// 			//         for(int j = 0; j <= _Radius; j++) {
// 			//             pos = float2(i, j) + start[k];
// 			// 			col = float3(1,1,1); //SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(pos.x * _MainTex_TexelSize.x, pos.y * _MainTex_TexelSize.y));
// 			//             //col = tex2Dlod(_MainTex, float4(uv + float2(pos.x * _MainTex_TexelSize.x, pos.y * _MainTex_TexelSize.y), 0., 0.)).rgb;
// 			//             mean[k] += col;
// 			//             sigma[k] += col * col;
// 			//         }
// 			//     }
// 			// }

// 			// float sigma2;

// 			// float n = pow(_Radius + 1, 2);
// 			float4 color = float4(1,1,0,1); //SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);//tex2D(_MainTex, uv);
// 			// float min = 1;

// 			// for (int l = 0; l < 4; l++) {
// 			//     mean[l] /= n;
// 			//     sigma[l] = abs(sigma[l] / n - mean[l] * mean[l]);
// 			//     sigma2 = sigma[l].r + sigma[l].g + sigma[l].b;

// 			//     if (sigma2 < min) {
// 			//         min = sigma2;
// 			//         color.rgb = mean[l].rgb;
// 			//     }
// 			// }
// 			return color;
// 		}

// 	ENDHLSL

//     SubShader
//     {
// 		// Not sure if needed
// 		Cull Off
// 		ZWrite Off
// 		ZTest Always
// 		// end not sure

//         Blend SrcAlpha OneMinusSrcAlpha
//         Pass
//         {
// 			HLSLPROGRAM
// 				#pragma vertex vert
// 				#pragma fragment frag
// 			ENDHLSL
//         }
//     }
// }

	HLSLINCLUDE

		#include "PostProcessing/Shaders/StdLib.hlsl"

		//Functions and macros from UnityCG, because we can't include it here (causes duplicates from StdLib)
		//Copied from UnityCG.cginc v2017.1.0f3

		inline float DecodeFloatRG( float2 enc )
		{
			float2 kDecodeDot = float2(1.0, 1/255.0);
			return dot( enc, kDecodeDot );
		}

		#if !defined(SHADER_TARGET_GLSL) && !defined(SHADER_API_PSSL) && !defined(SHADER_API_GLES3) && !defined(SHADER_API_VULKAN) && !(defined(SHADER_API_METAL) && defined(UNITY_COMPILER_HLSLCC))
			#define sampler2D_float sampler2D
		#endif

		#undef SAMPLE_DEPTH_TEXTURE
		#if defined(SHADER_API_PSP2)
			half4 SAMPLE_DEPTH_TEXTURE(sampler2D s, float4 uv) { return tex2D<float>(s, (float3)uv); }
			half4 SAMPLE_DEPTH_TEXTURE(sampler2D s, float3 uv) { return tex2D<float>(s, uv); }
			half4 SAMPLE_DEPTH_TEXTURE(sampler2D s, float2 uv) { return tex2D<float>(s, uv); }
		#else
			#define SAMPLE_DEPTH_TEXTURE(sampler, uv) (tex2D(sampler, uv).r)
		#endif

		//--------------------------------------------------------------------------------------------------------------------------------

		//Source image
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float4 _MainTex_ST;
		float4 _MainTex_TexelSize;

		//Camera depth/normals
		sampler2D _CameraDepthNormalsTexture;
		half4 _CameraDepthNormalsTexture_ST;
		sampler2D_float _CameraDepthTexture;
		half4 _CameraDepthTexture_ST;

		//Settings
		half4 _Sensitivity; 
		half4 _BgColor;
		half _BgFade;
		half _SampleDistance;
		float _Exponent;
		float _Threshold;
		float _Radius;

		//--------------------------------------------------------------------------------------------------------------------------------

		struct Varyings
		{
			float4 vertex : SV_POSITION;
			float2 texcoord[5] : TEXCOORD0;
			float2 texcoordStereo : TEXCOORD5;
		};

		struct VaryingsD
		{
			float4 vertex : SV_POSITION;
			float2 texcoord[2] : TEXCOORD0;
			float2 texcoordStereo : TEXCOORD2;
		};

		struct VaryingsLum
		{
			float4 vertex : SV_POSITION;
			float2 texcoord : TEXCOORD0;
			// float2 texcoordStereo : TEXCOORD3;
		};

		//--------------------------------------------------------------------------------------------------------------------------------

		inline half CheckSame (half2 centerNormal, float centerDepth, half4 theSample)
		{
			// difference in normals
			// do not bother decoding normals - there's no need here
			half2 diff = abs(centerNormal - theSample.xy) * _Sensitivity.y;
			int isSameNormal = (diff.x + diff.y) * _Sensitivity.y < 0.1;
			// difference in depth
			float sampleDepth = DecodeFloatRG (theSample.zw);
			float zdiff = abs(centerDepth-sampleDepth);
			// scale the required threshold by the distance
			int isSameDepth = zdiff * _Sensitivity.x < 0.09 * centerDepth;
			
			// return:
			// 1 - if normals and depth are similar enough
			// 0 - otherwise
			return isSameNormal * isSameDepth ? 1.0 : 0.0;
		}

		//--------------------------------------------------------------------------------------------------------------------------------

		//--------------------------------------------------------------------------------------------------------------------------------

		VaryingsLum VertThin(AttributesDefault v)
		{
			VaryingsDefault vd = VertDefault(v);
			VaryingsLum o;
			// o.vertex = v.vertex;
			// o.vertex = float4(v.vertex.xy, 0.0, 1.0);
			o.vertex = vd.vertex; //mul(unity_MatrixVP, float4(v.vertex.xyz,1)); 
			// o.texcoord = v.texcoord;
			o.texcoord = TRANSFORM_TEX(vd.texcoord, _MainTex);
			return o;
			
		// 	VaryingsLum o;

		// 	o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		// 	float2 texcoord = TransformTriangleVertexToUV(v.vertex.xy);

		// #if UNITY_UV_STARTS_AT_TOP
		// 	texcoord = texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		// #endif
			
		// 	o.texcoordStereo = TransformStereoScreenSpaceTex(texcoord, 1.0);
		// 	o.texcoord[0] = UnityStereoScreenSpaceUVAdjust(texcoord, _MainTex_ST);
			
		// 	// offsets for two additional samples
		// 	o.texcoord[1] = UnityStereoScreenSpaceUVAdjust(texcoord + float2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance, _MainTex_ST);
		// 	o.texcoord[2] = UnityStereoScreenSpaceUVAdjust(texcoord + float2(+_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance, _MainTex_ST);

		// 	return o;
		}

		float4 FragThin(VaryingsLum i) : SV_Target
		{
			half2 uv = i.texcoord;

			float3 mean[4] = {
			    {0, 0, 0},
			    {0, 0, 0},
			    {0, 0, 0},
			    {0, 0, 0}
			};

			float3 sigma[4] = {
			    {0, 0, 0},
			    {0, 0, 0},
			    {0, 0, 0},
			    {0, 0, 0}
			};

			float2 start[4] = {{-_Radius, -_Radius}, {-_Radius, 0}, {0, -_Radius}, {0, 0}};

			float2 pos;
			float3 col;
			for (int k = 0; k < 4; k++) {
			    for(int i = 0; i <= _Radius; i++) {
			        for(int j = 0; j <= _Radius; j++) {
			            pos = float2(i, j) + start[k];
						col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(pos.x * _MainTex_TexelSize.x, pos.y * _MainTex_TexelSize.y));
			            // col = tex2Dlod(_MainTex, float4(uv + float2(pos.x * _MainTex_TexelSize.x, pos.y * _MainTex_TexelSize.y), 0., 0.)).rgb;
			            mean[k] += col;
			            sigma[k] += col * col;
			        }
			    }
			}

			float sigma2;

			float n = pow(_Radius + 1, 2);
			float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);//tex2D(_MainTex, uv);
			float min = 1;

			for (int l = 0; l < 4; l++) {
			    mean[l] /= n;
			    sigma[l] = abs(sigma[l] / n - mean[l] * mean[l]);
			    sigma2 = sigma[l].r + sigma[l].g + sigma[l].b;

			    if (sigma2 < min) {
			        min = sigma2;
			        color.rgb = mean[l].rgb;
			    }
			}
			return color;

			// half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord[0]);

			// half4 center = tex2D(_CameraDepthNormalsTexture, i.texcoord[0]);
			// half4 sample1 = tex2D(_CameraDepthNormalsTexture, i.texcoord[1]);
			// half4 sample2 = tex2D(_CameraDepthNormalsTexture, i.texcoord[2]);
			
			// // encoded normal
			// half2 centerNormal = center.xy;
			// // decoded depth
			// float centerDepth = DecodeFloatRG(center.zw);
			
			// half edge = 1.0;
			// edge *= CheckSame(centerNormal, centerDepth, sample1);
			// edge *= CheckSame(centerNormal, centerDepth, sample2);

			// return float4(1,0,0,1);

			// return edge * lerp(color, _BgColor, _BgFade);
		}

		//--------------------------------------------------------------------------------------------------------------------------------

		
	ENDHLSL
	
	//--------------------------------------------------------------------------------------------------------------------------------

	Subshader
	{
		Cull Off
		ZWrite Off
		ZTest Always

		Pass
		{
			HLSLPROGRAM
				#pragma vertex VertThin
				#pragma fragment FragThin
			ENDHLSL
		}
	}

	Fallback off
}