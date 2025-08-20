///////////////////////////////////////////////////
//
// Author : Raja Sahaya Jose
// Created Date : 12/12/2018
// Modified Date : 12/12/2018
//
///////////////////////////////////////////////////
Shader "Nextwave/Mobile/Hue/HueBumpDiffuse" {

	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		[NoScaleOffset] _BumpMap("Normalmap", 2D) = "bump" {}
		_HueInput("Hue Input", Color) = (1,1,1,1)
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 250

			CGPROGRAM
			#pragma surface surf Lambert noforwardadd
			#include "UnityCG.cginc"
			#include "Assets/Shaders/nxtShaders/include/mobile/nxtColorMathLib.cginc"
			#pragma target 2.0

			sampler2D _MainTex;
			sampler2D _BumpMap;
			half4     _HueInput;

			struct Input {
				half2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				half3 hsvAlbedo = mobRGB2HSV(o.Albedo.xyz);
				half3 hsvHueInput = mobRGB2HSV(_HueInput.xyz);
				hsvAlbedo.x += hsvHueInput.x;
				if (hsvAlbedo.x > 1.0) { hsvAlbedo.x -= 1.0; }
				o.Albedo = half3(mobHSV2RGB(hsvAlbedo));
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			}
			ENDCG
		}
			FallBack "Diffuse"
}
