///////////////////////////////////////////////////
//
//   Author : Raja Sahaya Jose
//   Created Date : 11/12/2018
//   Modified Date : 12/12/2018
//
///////////////////////////////////////////////////

Shader "Nextwave/Mobile/Hue/HueDiffuse" {

	Properties
	{
		_MainTex("Texture Input", 2D) = "white" {}
		_HueInput("Hue Input", Color) = (1,1,1,1)
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 150

			CGPROGRAM
			#pragma surface surf Lambert noforwardadd
			#include "UnityCG.cginc"
			#include "Assets/nxtShaders/include/mobile/nxtColorMathLib.cginc"
			#pragma target 2.0

			sampler2D _MainTex;
			half4    _HueInput;

			struct Input
			{
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
			}
			ENDCG
		}
			FallBack "Diffuse"
}
