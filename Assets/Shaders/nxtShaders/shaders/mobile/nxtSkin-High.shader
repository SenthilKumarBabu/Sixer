///////////////////////////////////////////////////
//
// Author : Raja Sahaya Jose
// Created Date : 17/12/2018
// Modified Date : 26/12/2018
//
///////////////////////////////////////////////////

//#pragma shader_feature _USE_HIGHLIGHT_GRID_ON
//#if _USE_HIGHLIGHT_GRID_ON
//#endif


Shader "Nextwave/Mobile/Skin/Skin-High"
{
	Properties
	{
		  [Header(SKIN TEXTURE ATTRIBUTES)]
		  [Space]
		  _SkinDiffuseMap("Skin Diffuse (RGB)", 2D) = "White" {}
		  _SkinNoramlMap("Skin Normal (Normal)", 2D) = "bump" {}
		  _SkinThicknessMap("Skin Thickness (RGB)", 2D) = "White" {}
		  [Space(20)]
		  [Header(HIGH LEVEL SKIN ATTRIBUTES)]
		  [Space]
		  _SkinColorInput("Skin Color", Color) = (1,1,1,1)
		  [Space]
		  _SkinColorInputOpacity("Skin Color Opactiy",Range(0,1)) = .70
		  [Space]
		  [KeywordEnum(Normal,Multiply ,Overlay,Softlight)] _Blend("Blend mode", Float) = 0
		  [Space]
		  _SkinTranslucentColorInput("Skin Translucent Color", Color) = (1,1,1,1)
		  [Space]
		  _SkinSmoothness("Skin Smoothness", Range(0, 1)) = 0.0
		  [Space]
		  _SkinShininess("Skin Shininess", Range(0, 1)) = 0.0
		  [Space]
		  _SkinTranslucentDistortion("Skin Translucent Distortion", Range(0, 1)) = 0.0
		  [Space]
		  _SkinTranslucentPower("Skin Translucent Power", Range(0, 10)) = 0.1
		  [Space]
		  _SkinTranslucentScale("Skin Translucent Scale", Range(0, 10)) = 0.0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf StandardTranslucent fullforwardshadows
			#include "Assets/Shaders/nxtShaders/include/Blends.cginc"
			#pragma multi_compile _BLEND_NORMAL _BLEND_MULTIPLY _BLEND_OVERLAY _BLEND_SOFTLIGHT
			#pragma target 2.0

			sampler2D _SkinDiffuseMap;
			sampler2D _SkinNoramlMap;
			sampler2D _SkinThicknessMap;

			struct Input {
				half2 uv_SkinDiffuseMap;
			};

			fixed4 _SkinColorInput;
			fixed4 _SkinTranslucentColorInput;
			half   _SkinSmoothness;
			half   _SkinShininess;
			half   _SkinTranslucentDistortion;
			half   _SkinTranslucentPower;
			half   _SkinTranslucentScale;
			half   _SkinThickness;
			fixed _SkinColorInputOpacity;
			fixed4 diffuseColor;

			#include "UnityPBSLighting.cginc"

			inline fixed4 LightingStandardTranslucent(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
			{
				// Original colour
				fixed4 pbr = LightingStandard(s, viewDir, gi);

				// --- Translucency ---
				half3 L = gi.light.dir;
				half3 V = viewDir;
				half3 N = s.Normal;

				half3 H = normalize(L + N * _SkinTranslucentDistortion);
				half VdotH = pow(saturate(dot(V, -H)), _SkinTranslucentPower) *  _SkinTranslucentScale;
				half3 I = (VdotH + unity_AmbientSky) * _SkinThickness;

				// Final add
				pbr.rgb = pbr.rgb + (gi.light.color * _SkinTranslucentColorInput.rgb) * I;
				return pbr;
			}

			void LightingStandardTranslucent_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
			{
				LightingStandard_GI(s, data, gi);
			}

			void surf(Input IN, inout SurfaceOutputStandard o) {

				fixed4 diffuseColorInput = tex2D(_SkinDiffuseMap, IN.uv_SkinDiffuseMap);

#if _BLEND_NORMAL
				diffuseColor = lerp(diffuseColorInput, _SkinColorInput, _SkinColorInputOpacity);
#endif

#if _BLEND_MULTIPLY
				_SkinColorInput *= _SkinColorInputOpacity;
				diffuseColor = Multiply(diffuseColorInput, _SkinColorInput);
#endif

#if _BLEND_OVERLAY
				_SkinColorInput *= _SkinColorInputOpacity;
				diffuseColor = Overlay(diffuseColorInput, _SkinColorInput);
#endif

#if _BLEND_SOFTLIGHT
				_SkinColorInput *= _SkinColorInputOpacity;
				diffuseColor = SoftLight(diffuseColorInput, _SkinColorInput);
#endif

				o.Albedo = diffuseColor.rgb;
				o.Metallic = _SkinShininess;
				o.Smoothness = _SkinSmoothness;
				o.Alpha = diffuseColor.a;
				o.Normal = UnpackNormal(tex2D(_SkinNoramlMap, IN.uv_SkinDiffuseMap));
				_SkinThickness = tex2D(_SkinThicknessMap, IN.uv_SkinDiffuseMap).r;
			}
			ENDCG
		  }
			  FallBack "Diffuse"
}



