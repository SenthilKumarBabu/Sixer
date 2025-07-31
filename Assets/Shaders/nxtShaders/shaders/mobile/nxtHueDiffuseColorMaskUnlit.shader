///////////////////////////////////////////////////
//
//   Author : Raja Sahaya Jose
//   Created Date : 11/12/2018
//   Modified Date : 12/12/2018
//
///////////////////////////////////////////////////

Shader "Nextwave/Mobile/Hue/Unlit/HueDiffuseColorMask" {
	Properties
	{
		_MainTex("Texture Input", 2D) = "white" {}
		_InfluenceMask("Influence Mask Input", 2D) = "white" {}
		_DiffuseColorInput("Diffuse Color", Color) = (1,1,1,1)
		_HueInput("Hue Input", Color) = (1,1,1,1)
		_NonInfluenceBrightness("NonInfluence Brightness", Range(0.0, 1.0)) = 0.5
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 150
			//#pragma target 3.0

			CGPROGRAM
			 #pragma surface surf NoLighting noambient
			#include "UnityCG.cginc"
			#include "Assets/nxtShaders/include/mobile/nxtColorMathLib.cginc"

			sampler2D _MainTex;
			sampler2D _InfluenceMask;
			half4     _DiffuseColorInput;
			half4     _HueInput;
			half      _NonInfluenceBrightness;

			struct Input
			{
				half2 uv_MainTex;
				half2 uv_InfluenceMask;
			};

			fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
				return fixed4(s.Albedo, s.Alpha);
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 MainTexColor = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 InfluenceMask = tex2D(_InfluenceMask, IN.uv_InfluenceMask);
				fixed4 NoInfluenceMask = 1 - InfluenceMask;
				fixed4 InfluenceColor = MainTexColor * InfluenceMask;
				fixed4 NonInfluenceColor = MainTexColor * NoInfluenceMask;
				fixed4 NonInfluenceRegion = MainTexColor != InfluenceColor;
				fixed4 InfluenceFinalColor = InfluenceColor * _DiffuseColorInput;

				half3 hsvAlbedo = mobRGB2HSV(InfluenceFinalColor.rgb);
				half3 hsvHueInput = mobRGB2HSV(_HueInput.xyz);
				hsvAlbedo.x += hsvHueInput.x;
				if (hsvAlbedo.x > 1.0) { hsvAlbedo.x -= 1.0; }
				o.Albedo = half3(mobHSV2RGB(hsvAlbedo)) + ((NonInfluenceRegion + NoInfluenceMask) * (NonInfluenceColor * _NonInfluenceBrightness));
				o.Alpha = MainTexColor.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
