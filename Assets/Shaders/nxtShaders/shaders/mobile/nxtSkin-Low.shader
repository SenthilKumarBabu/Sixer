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


Shader "Nextwave/Mobile/Skin/Skin-Low"
{
	Properties
	{
		  [Header(SKIN TEXTURE ATTRIBUTES)]
		  [Space]
		  _SkinDiffuseMap("Skin Diffuse (RGB)", 2D) = "White" {}
		  [Space(20)]
		  [Header(LOW LEVEL SKIN ATTRIBUTES)]
		  [Space]
		  _SkinColorInput("Skin Color", Color) = (1,1,1,1)
		  [Space]
		  _SkinColorInputOpacity("Skin Color Opactiy",Range(0,1)) = .70
		  [Space]
		  [KeywordEnum(Normal,Multiply ,Overlay,Softlight)] _Blend("Blend mode", Float) = 0
		  [Space]
		  _SkinRimColor("Skin Rim Color", Color) = (0.26,0.19,0.16,0.0)
		  [Space]
		  _SkinRimPower("Skin Rim Power", Range(0.5,8.0)) = 3.0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }

			CGPROGRAM
			  //#pragma surface surf Lambert
			  #pragma surface surf Lambert noforwardadd
			  #include "UnityCG.cginc"
			  #include "Assets/Shaders/nxtShaders/include/Blends.cginc"
			  #pragma multi_compile _BLEND_NORMAL _BLEND_MULTIPLY _BLEND_OVERLAY _BLEND_SOFTLIGHT
			  #pragma target 2.0

			  sampler2D _SkinDiffuseMap;
			  fixed4 _SkinColorInput;
			  fixed _SkinColorInputOpacity;
			  fixed4 _SkinRimColor;
			  half   _SkinRimPower;
			  fixed4 diffuseColor;

			  struct Input {
				  float2 uv_SkinDiffuseMap;
				  float3 viewDir;
			  };

			  void surf(Input IN, inout SurfaceOutput o)
			  {
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
				  half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				  o.Emission = _SkinRimColor.rgb * pow(rim, _SkinRimPower);

			  }
			  ENDCG
		  }
			  FallBack "Diffuse"
}



