Shader "Nextwave/Transition2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OffsetX("Texture Offset X",Float) =0
		_OffsetY("Texture Offset Y",Float)=0
		_Blend("Blend",Range(0,1))=0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" "IgnoreProjectors"="True" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float _Blend;
			float _OffsetX;
			float _OffsetY;

			VertexOutput vert (VertexInput v)
			{
				VertexOutput o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = v.uv;;
				return o;
			}
			
			fixed4 frag (VertexOutput i) : SV_Target
			{
				fixed4 main = tex2D(_MainTex, i.uv);
				fixed4 sec = tex2D(_MainTex, i.uv2+float2(_OffsetX,_OffsetY));
				fixed3 col = lerp(main.rgb, sec.rgb, _Blend);
				return fixed4(col,1);
			}
			ENDCG
		}
	}
}
