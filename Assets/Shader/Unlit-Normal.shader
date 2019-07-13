// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Normal"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
		
		[Toggle(CubeMapOn)] _CubeMapOn("CubeMapOn", Float) = 0
		_Cubemap ("CubeMap", CUBE) = ""{}  
        _ReflAmount ("Reflection Amount", Range(0.01, 1)) = 0.5 

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		[Toggle(VertexColorOn)] _VertexColor("VertexColor", Float) = 0
		[Toggle(LightmapOn)] _LightMap("LightMap", Float) = 0

		[KeywordEnum(Off, Front, Back)] _Cull("Culling", Float) = 2 

		[Toggle(PowerOn)] _Power("Power", Float) = 0
		_PowerValue("Power Value",Range(0,10)) = 1

		[Toggle(CurveOn)] _Curve("Curve", Float) = 0
		_CurveValue("Curve Value", Range(50,300)) = 100

		// Blending state
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector][Toggle] _ZWrite ("__zw", Float) = 1.0
		[HideInInspector][Toggle] _UseCutout("__useCutout", Float) = 0
	}

	SubShader
	{		
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull [_Cull]
			Lighting Off
			ZWrite [_ZWrite]

			Name "MAIN"
			CGPROGRAM

			#pragma shader_feature _ _USECUTOUT_ON
			#pragma shader_feature CubeMapOn
			#pragma shader_feature VertexColorOn
			#pragma shader_feature PowerOn
			#pragma shader_feature LightmapOn
			//#pragma CurveOn
			#pragma multi_compile __ CurveOn

			#pragma fragmentoption ARB_precision_hint_fastest
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				#ifdef LightmapOn
					float2 uv1 : TEXCOORD1;
				#endif
				#ifdef VertexColorOn
					fixed4 color : COLOR0;
				#endif
				#ifdef CubeMapOn
				float3 normal : NORMAL;
				#endif
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				#ifdef LightmapOn
					half2 uvLM : TEXCOORD1;
				#endif
				#ifdef VertexColorOn
					fixed4 vertColor : COLOR0;
				#endif
				#ifdef CubeMapOn
				float3 refl : TEXCOORD2;
				#endif
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _MaskTex;
			
			#ifdef CubeMapOn
			samplerCUBE _Cubemap; 
			float _ReflAmount;  
			#endif

			#ifdef PowerOn
			fixed _PowerValue;
			#endif

			#ifdef CurveOn
			fixed _CurveValue;
			#endif

			#ifdef _USECUTOUT_ON
			fixed _Cutoff;
			#endif
			
			v2f vert (appdata v)
			{
				v2f o;

				#ifdef CurveOn
					float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
					float zOff = vPos.z/_CurveValue;
					vPos += float4(0,0,-15,0)*zOff*zOff;

					o.vertex = mul (UNITY_MATRIX_P, vPos);
				#else
					o.vertex = UnityObjectToClipPos(v.vertex);				
				#endif

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				#ifdef CubeMapOn
				float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);  
				o.refl = reflect(-WorldSpaceViewDir(v.vertex), worldNormal);  
				#endif

				#ifdef LightmapOn
					o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				#ifdef VertexColorOn
					o.vertColor = v.color;
				#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 maskCol = tex2D(_MaskTex, i.uv);
				
				// sample the texture
				fixed4 col = _Color * tex2D(_MainTex, i.uv);
				
				//col.rgb *= maskCol.rgb;
				
				#ifdef VertexColorOn
					col = col * i.vertColor;
				#endif						

				#ifdef LightmapOn  
					col.rgb *= DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));  
					//fixed4 lightmapColor = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy);
					//fixed3 lm = (lightmapColor.a * 5.0) * lightmapColor.rgb;
					//col.rgb*=lm;  
                #endif  

				#ifdef CubeMapOn
					col.rgb += texCUBE(_Cubemap,i.refl) * _ReflAmount * maskCol.rgb; 
				#endif

				#ifdef PowerOn
					col.rgb = col.rgb * _PowerValue;
				#endif

				#ifdef _USECUTOUT_ON
					clip(col.a - _Cutoff);
				#endif				
				return col;
			}
			ENDCG
		}

		// Pass to render object as a shadow caster
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
			#include "UnityCG.cginc"

			struct v2f { 
				V2F_SHADOW_CASTER;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert( appdata_base v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag( v2f i ) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG

		}
	}

	CustomEditor "UnlitShaderGUI"

	//Fallback "Diffuse"
}
