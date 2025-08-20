///////////////////////////////////////////////////
//
//   Author : Raja Sahaya Jose
//   Created Date : 11/12/2018
//   Modified Date : 12/12/2018
//
///////////////////////////////////////////////////

Shader "Nextwave/Mobile/Hue/HueDiffuseMask" {
	Properties
	{
		_MainTex("Texture Input", 2D) = "white" {}
		_InfluenceMask("Influence Mask Input", 2D) = "white" {}
		_HueInput("Hue Input", Color) = (1,1,1,1)
		_NonInfluenceBrightness("NonInfluence Brightness", Range(0.0, 1.0)) = 0.5
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 150

			CGPROGRAM
			#pragma surface surf Lambert noforwardadd
			#include "UnityCG.cginc"
			#include "Assets/Shaders/nxtShaders/include/mobile/nxtColorMathLib.cginc"
			#pragma target 2.0

			sampler2D _MainTex;
			sampler2D _InfluenceMask;
			half4     _HueInput;
			half _NonInfluenceBrightness;

			struct Input
			{
				half2 uv_MainTex;
				half2 uv_InfluenceMask;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 MainTexColor = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 InfluenceMask = tex2D(_InfluenceMask, IN.uv_InfluenceMask);
				fixed4 NoInfluenceMask = 1 - InfluenceMask;
				fixed4 InfluenceColor = MainTexColor * InfluenceMask;
				fixed4 NonInfluenceColor = MainTexColor * NoInfluenceMask;
				fixed4 NonInfluenceRegion = MainTexColor != InfluenceColor;

				half3 hsvAlbedo = mobRGB2HSV(InfluenceColor.rgb);
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
