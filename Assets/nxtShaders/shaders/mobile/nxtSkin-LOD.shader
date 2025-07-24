///////////////////////////////////////////////////
//
// Author : Raja Sahaya Jose
// Created Date : 17/12/2018
// Modified Date : 26/12/2018
//
///////////////////////////////////////////////////

Shader "Nextwave/Mobile/Skin/Skin-LOD"
{
	Properties
	{
		  [Header(SKIN QUALITY CONTROLLER)]
		  [Space]
          [KeywordEnum(Low, Medium, High)] _SkinQuality("Skin LOD", Float) = 2
		  [Space(20)]
		  [Header(SKIN TEXTURE ATTRIBUTES)]
		  [Space]
		  _SkinDiffuseMap("Skin Diffuse (RGB)", 2D) = "White" {}
		  _SkinNoramlMap("Skin Normal (Normal)", 2D) = "bump" {}
		  _SkinThicknessMap("Skin Thickness (RGB)", 2D) = "White" {}
		  [Space]
		  _SkinColorInput("Skin Color", Color) = (1,1,1,1)
		  [Space]
		  [KeywordEnum(Normal,Multiply ,Overlay,Softlight)] _Blend("Blend mode", Float) = 0
		  [Space]
		  _SkinColorInputOpacity("Skin Color Opactiy",Range(0,1)) = .70
		  [Space(20)]
		  [Header(HIGH LEVEL SKIN ATTRIBUTES)]
		  [Space]
		  _SkinTranslucentColorInputHigh("Skin Translucent Color", Color) = (1,1,1,1)
		  [Space]
		  _SkinSmoothnessHigh("Skin Smoothness", Range(0, 1)) = 0.0
		  [Space]
		  _SkinShininessHigh("Skin Shininess", Range(0, 1)) = 0.0
		  [Space]
		  _SkinTranslucentDistortionHigh("Skin Translucent Distortion", Range(0, 1)) = 0.0
		  [Space]
		  _SkinTranslucentPowerHigh("Skin Translucent Power", Range(0, 10)) = 0.1
		  [Space]
		  _SkinTranslucentScaleHigh("Skin Translucent Scale", Range(0, 10)) = 0.0
		  [Space(20)]
		  [Header(MEDIUM LEVEL SKIN ATTRIBUTES)]
		  [Space]
		  _SkinRimColorMedium("Skin Rim Color", Color) = (0.26,0.19,0.16,0.0)
		  [Space]
		  _SkinRimPowerMedium("Skin Rim Power", Range(0.5,8.0)) = 3.0
		  [Space(20)]
		  [Header(LOW LEVEL SKIN ATTRIBUTES)]
		  [Space]
		  _SkinRimColorLow("Skin Rim Color", Color) = (0.26,0.19,0.16,0.0)
		  [Space]
		  _SkinRimPowerLow("Skin Rim Power", Range(0.5,8.0)) = 3.0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
               
			CGPROGRAM
			#pragma surface surf StandardTranslucent fullforwardshadows
            #pragma multi_compile _SKINQUALITY_LOW  _SKINQUALITY_MEDIUM  _SKINQUALITY_HIGH
			#pragma multi_compile _BLEND_NORMAL _BLEND_MULTIPLY _BLEND_OVERLAY _BLEND_SOFTLIGHT
			#include "Assets/nxtShaders/include/Blends.cginc"
			#pragma target 2.0
            
            // Skin Common Vars
			half      _SkinQuality;
			sampler2D _SkinDiffuseMap;
			sampler2D _SkinNoramlMap;
			sampler2D _SkinThicknessMap;
			fixed4    _SkinColorInput;
			fixed     _SkinColorInputOpacity;
			fixed4    diffuseColor;

			// Skin High Vars
			fixed4 _SkinTranslucentColorInputHigh;
			half   _SkinSmoothnessHigh;
			half   _SkinShininessHigh;
			half   _SkinTranslucentDistortionHigh;
			half   _SkinTranslucentPowerHigh;
			half   _SkinTranslucentScaleHigh;
			half   _SkinThicknessHigh;

			// Skin Medium Vars
			fixed4 _SkinRimColorMedium;
			half   _SkinRimPowerMedium;

			// Skin Low Vars
			fixed4 _SkinRimColorLow;
			half   _SkinRimPowerLow;


			struct Input {
				half2 uv_SkinDiffuseMap;

#if defined(_SKINQUALITY_MEDIUM)
				float2 uv_SkinNoramlMap;
#endif

#if defined(_SKINQUALITY_MEDIUM) || defined(_SKINQUALITY_LOW)
				float3 viewDir;
#endif
			};

            #include "UnityPBSLighting.cginc"
			inline fixed4 LightingStandardTranslucent(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
			{ 
				// Original colour
				fixed4 pbr = LightingStandard(s, viewDir, gi);

				// --- Translucency ---
			  	half3 L = gi.light.dir;
				half3 V = viewDir;
				half3 N = s.Normal;

				half3 H = normalize(L + N * _SkinTranslucentDistortionHigh);
				half VdotH = pow(saturate(dot(V, -H)), _SkinTranslucentPowerHigh) *  _SkinTranslucentScaleHigh;
				half3 I = (VdotH + unity_AmbientSky) * _SkinThicknessHigh;

				// Final add
				pbr.rgb = pbr.rgb + (gi.light.color * _SkinTranslucentColorInputHigh.rgb) * I;
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

#if defined(_SKINQUALITY_HIGH)
				o.Albedo = diffuseColor.rgb;
				o.Metallic = _SkinShininessHigh;
				o.Smoothness = _SkinSmoothnessHigh;
				o.Alpha = diffuseColor.a;
				o.Normal = UnpackNormal(tex2D(_SkinNoramlMap, IN.uv_SkinDiffuseMap));
				_SkinThicknessHigh = tex2D(_SkinThicknessMap, IN.uv_SkinDiffuseMap).r;
#endif

#if defined(_SKINQUALITY_MEDIUM)			
				o.Albedo = diffuseColor.rgb;
				o.Normal = UnpackNormal(tex2D(_SkinNoramlMap, IN.uv_SkinNoramlMap));
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = _SkinRimColorMedium.rgb * pow(rim, _SkinRimPowerMedium);
#endif

#if defined(_SKINQUALITY_LOW)
				o.Albedo = diffuseColor.rgb;
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = _SkinRimColorLow.rgb * pow(rim, _SkinRimPowerLow);
#endif

			}
			ENDCG
		  }
			  FallBack "Diffuse"
}



