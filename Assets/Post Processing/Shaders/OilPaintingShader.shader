// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/OilPainting"
{
	HLSLINCLUDE

		#include "PostProcessing/Shaders/StdLib.hlsl"

		//Functions and macros from UnityCG, because we can't include it here (causes duplicates from StdLib)
		//Copied from UnityCG.cginc v2017.1.0f3

		//--------------------------------------------------------------------------------------------------------------------------------

		//Source image
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float4 _MainTex_ST;
		float4 _MainTex_TexelSize;

		//Settings
		float _Radius;

		//--------------------------------------------------------------------------------------------------------------------------------

		struct VaryingsLum
		{
			float4 vertex : SV_POSITION;
			float2 texcoord : TEXCOORD0;
		};

		VaryingsLum VertThin(AttributesDefault v)
		{
			VaryingsDefault vd = VertDefault(v);
			VaryingsLum o;
			o.vertex = vd.vertex;
			o.texcoord = TRANSFORM_TEX(vd.texcoord, _MainTex);
			return o;
			
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

			// float4x4 mean
			// float4x4 sigma

			float3 sigma[4] = {
			    {0, 0, 0},
			    {0, 0, 0},
			    {0, 0, 0},
			    {0, 0, 0}
			};

			// float4x4 start

			float2 start[4] = {
				{-_Radius, -_Radius}, 
				{-_Radius, 0}, 
				{0, -_Radius}, 
				{0, 0}
			};

			float2 pos;
			float3 col;
			for (int k = 0; k < 4; k++) {
			    for(int i = 0; i <= _Radius; i += _Radius/2) {
			        for(int j = 0; j <= _Radius; j += _Radius/2) {
			            pos = float2(i, j) + start[k];
						col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(pos.x * _MainTex_TexelSize.x, pos.y * _MainTex_TexelSize.y));
			            mean[k] += col;
			            sigma[k] += col * col;
			        }
			    }
			}

			// for i
			// for j
			// 	float4x4 posMat = {i,j,0,0}, (x4)
			// 	posMat = posMat + start
			//  

			float sigma2;

			float n = pow(3,2);//pow(_Radius + 1, 2); //
			float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
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
		}

		
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