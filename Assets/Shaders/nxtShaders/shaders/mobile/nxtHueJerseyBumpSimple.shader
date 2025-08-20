///////////////////////////////////////////////////
//
// Author : Raja Sahaya Jose
// Created Date : 12/12/2018
// Modified Date : 12/12/2018
//
///////////////////////////////////////////////////
Shader "Nextwave/Mobile/HueJersey/HueJerseyBumpSimple" {

	Properties
	{
		[Header(JERSEY GENERAL ATTRIBUTES)]
		_HueInput("Hue", Color) = (1,1,1,1)
		[Space(20)]
		_MainTex("Base Texture (RGB)", 2D) = "white" {}
		[Space(20)]
		[NoScaleOffset] _BumpMap("Normalmap", 2D) = "bump" {}
		[Space(20)]

		[Header(JERSEY NUMBER ATTRIBUTES)]
		_NumberTex("Number Texture (RGB)", 2D) = "white" {}
		_NumberDisplayRegion("Number Display Region", Range(0.0, 0.5)) = 1
		_NumberRectMinX("Number Region Min X (%)", Float) = 25
		_NumberRectMaxX("Number Region Max X (%)", Float) = 75
		_NumberRectMinY("Number Region Min Y (%)", Float) = 25
		_NumberRectMaxY("Number Region Max Y (%)", Float) = 75
		[Space(20)]

		[Header(JERSEY NAME ATTRIBUTES)]
		_NameTex("Name Texture (RGB)", 2D) = "white" {}
		_NameDisplayRegion("Name Display Region", Range(0.0, 0.5)) = 1
		_NameRectMinX("Name Region Min X (%)", Float) = 25
		_NameRectMaxX("Name Region Max X (%)", Float) = 75
		_NameRectMinY("Name Region Min Y (%)", Float) = 25
		_NameRectMaxY("Name Region Max Y (%)", Float) = 75
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha
			Lighting On
		    //#pragma target 2.0

			//Cull Off
			CGPROGRAM
			#pragma  surface surf Lambert 
			#include "Assets/Shaders/nxtShaders/include/mobile/nxtColorMathLib.cginc"

			half4     _HueInput;
			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _NumberTex;
			fixed    _NumberDisplayRegion;
			fixed    _NumberRectMinX;
			fixed    _NumberRectMaxX;
			fixed    _NumberRectMinY;
			fixed    _NumberRectMaxY;

			sampler2D _NameTex;
			fixed _NameDisplayRegion;
			fixed _NameRectMinX;
			fixed _NameRectMaxX;
			fixed _NameRectMinY;
			fixed _NameRectMaxY;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_NumberTex;
				float2 uv_NameTex;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				//General Vars
				float2 preNumberRect, preNameRect;
				half numberRectMask, nameRectMask;
				float2 uv_Number_OffsetCoord, uv_Name_OffsetCoord;
				fixed3 MaskWhite = fixed3(1, 1, 1);

				//Below divides all rectangle parameters by 100, so they are not super-sensitive in the unity editor
				_NumberRectMinX /= 100; _NumberRectMaxX /= 100; _NumberRectMinY /= 100; _NumberRectMaxY /= 100;
				_NameRectMinX /= 100; _NameRectMaxX /= 100; _NameRectMinY /= 100; _NameRectMaxY /= 100;

				//preRect x and y each represent 1 dimensional min and max ranges for rectangle. When they are multiplied together, they form a white rectangle mask (where they intersect).
				preNumberRect.x = (IN.uv_MainTex.x >= _NumberRectMinX) - (IN.uv_MainTex.x >= _NumberRectMaxX);
				preNumberRect.y = (IN.uv_MainTex.y >= _NumberRectMinY) - (IN.uv_MainTex.y >= _NumberRectMaxY);
				numberRectMask = preNumberRect.x * preNumberRect.y;
				preNameRect.x = (IN.uv_MainTex.x > _NameRectMinX) - (IN.uv_MainTex.x > _NameRectMaxX);
				preNameRect.y = (IN.uv_MainTex.y > _NameRectMinY) - (IN.uv_MainTex.y > _NameRectMaxY);
				nameRectMask = preNameRect.x * preNameRect.y;

				//uv_OffsetCoord.x and y copy the uv coordinates of the main texture and are offsetted.
				//Then, the old uv coordinates are blended with the new uv coordinates, using the rectangle as a mask.
				uv_Number_OffsetCoord = IN.uv_MainTex;
				uv_Name_OffsetCoord = IN.uv_MainTex;

				//add minimum rectangle limits so the region's lowest UV value is 0
				uv_Number_OffsetCoord.x -= _NumberRectMinX;
				uv_Number_OffsetCoord.y -= _NumberRectMinY;
				uv_Name_OffsetCoord.x -= _NameRectMinX;
				uv_Name_OffsetCoord.y -= _NameRectMinY;

				//multiply values so the highest UV value of the region is 1. Now the region is normalized to a 0-1 range.
				uv_Number_OffsetCoord.x *= (1 / (_NumberRectMaxX - _NumberRectMinX));
				uv_Number_OffsetCoord.y *= (1 / (_NumberRectMaxY - _NumberRectMinY));
				uv_Name_OffsetCoord.x *= (1 / (_NameRectMaxX - _NameRectMinX));
				uv_Name_OffsetCoord.y *= (1 / (_NameRectMaxY - _NameRectMinY));

				//Blend old uv coordinates with new offsetted uv coordinates, using the rectangle as a mask
				IN.uv_NumberTex = uv_Number_OffsetCoord;
				IN.uv_NameTex = uv_Name_OffsetCoord;

				//Apply image map to blended UV coordinates  
				half4 MainTexColor = tex2D(_MainTex, IN.uv_MainTex);
				half4 NumberTexColor = tex2D(_NumberTex, IN.uv_NumberTex);
				half4 NameTexColor = tex2D(_NameTex, IN.uv_NameTex);

				//displays additive green rectangle for setup
				MainTexColor.g += (numberRectMask * _NumberDisplayRegion);
				MainTexColor.g += (nameRectMask * _NameDisplayRegion);

				//Masks
				half3 NumberMask = MaskWhite * numberRectMask;
				half3 NumberInverseMask = 1 - NumberMask;
				half3 NameMask = MaskWhite * nameRectMask;
				half3 NameInverseMask = 1 - NameMask;
				half3 IntersectionMask = NumberMask + NameMask * (1 - NumberMask);
				half3 IntersectionCombinedMask = IntersectionMask - (NumberTexColor.a * numberRectMask) - (NameTexColor.a * nameRectMask);

				//Visibility
				half3 MainTexVisible = (NameInverseMask * NumberInverseMask) * MainTexColor;
				half3 NumberTexVisible = NumberMask * ((NumberTexColor.rgb * NumberTexColor.a));
				half3 NameTexVisible = NameMask * ((NameTexColor.rgb * NameTexColor.a));
				half3 IntersectionOutput = (IntersectionCombinedMask * MainTexColor *IntersectionCombinedMask) + NumberTexVisible + NameTexVisible;
				half3 Output = IntersectionOutput + ((1 - IntersectionMask) * MainTexVisible);

				//Output
				half3 hsvAlbedo = mobRGB2HSV(Output);
				half3 hsvHueInput = mobRGB2HSV(_HueInput.xyz);
				hsvAlbedo.x += hsvHueInput.x;
				if (hsvAlbedo.x > 1.0) { hsvAlbedo.x -= 1.0; }
				o.Albedo = half3(mobHSV2RGB(hsvAlbedo));
				o.Alpha = MainTexColor.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			}
			ENDCG
		}
			FallBack "Diffuse"
}