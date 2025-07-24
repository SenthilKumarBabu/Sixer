Shader "Nextwave/HueBump" {
    Properties {
        _JerseyTexture ("Jersey Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NameTexture ("Name Texture", 2D) = "black" {}
        _NumberTexture ("Number Texture", 2D) = "black" {}
        _Hue ("Hue", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 2.0
            uniform float4 _LightColor0;
            uniform sampler2D _JerseyTexture; uniform float4 _JerseyTexture_ST;
            uniform sampler2D _NameTexture; uniform float4 _NameTexture_ST;
            uniform sampler2D _NumberTexture; uniform float4 _NumberTexture_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Hue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _JerseyTexture_var = tex2D(_JerseyTexture,TRANSFORM_TEX(i.uv0, _JerseyTexture));
                float4 _NumberTexture_var = tex2D(_NumberTexture,TRANSFORM_TEX(i.uv0, _NumberTexture));
                float4 _NameTexture_var = tex2D(_NameTexture,TRANSFORM_TEX(i.uv0, _NameTexture));
                float3 node_856 = lerp(lerp(_JerseyTexture_var.rgb,_NumberTexture_var.rgb,_NumberTexture_var.a),_NameTexture_var.rgb,_NameTexture_var.a);
                float4 node_3955_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_3955_p = lerp(float4(float4(node_856,0.0).zy, node_3955_k.wz), float4(float4(node_856,0.0).yz, node_3955_k.xy), step(float4(node_856,0.0).z, float4(node_856,0.0).y));
                float4 node_3955_q = lerp(float4(node_3955_p.xyw, float4(node_856,0.0).x), float4(float4(node_856,0.0).x, node_3955_p.yzx), step(node_3955_p.x, float4(node_856,0.0).x));
                float node_3955_d = node_3955_q.x - min(node_3955_q.w, node_3955_q.y);
                float node_3955_e = 1.0e-10;
                float3 node_3955 = float3(abs(node_3955_q.z + (node_3955_q.w - node_3955_q.y) / (6.0 * node_3955_d + node_3955_e)), node_3955_d / (node_3955_q.x + node_3955_e), node_3955_q.x);;
                float3 diffuseColor = (lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac((_Hue+node_3955.r)+float3(0.0,-1.0/3.0,1.0/3.0)))-1),node_3955.g)*node_3955.b);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma target 2.0
            uniform float4 _LightColor0;
            uniform sampler2D _JerseyTexture; uniform float4 _JerseyTexture_ST;
            uniform sampler2D _NameTexture; uniform float4 _NameTexture_ST;
            uniform sampler2D _NumberTexture; uniform float4 _NumberTexture_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Hue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _JerseyTexture_var = tex2D(_JerseyTexture,TRANSFORM_TEX(i.uv0, _JerseyTexture));
                float4 _NumberTexture_var = tex2D(_NumberTexture,TRANSFORM_TEX(i.uv0, _NumberTexture));
                float4 _NameTexture_var = tex2D(_NameTexture,TRANSFORM_TEX(i.uv0, _NameTexture));
                float3 node_856 = lerp(lerp(_JerseyTexture_var.rgb,_NumberTexture_var.rgb,_NumberTexture_var.a),_NameTexture_var.rgb,_NameTexture_var.a);
                float4 node_3955_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_3955_p = lerp(float4(float4(node_856,0.0).zy, node_3955_k.wz), float4(float4(node_856,0.0).yz, node_3955_k.xy), step(float4(node_856,0.0).z, float4(node_856,0.0).y));
                float4 node_3955_q = lerp(float4(node_3955_p.xyw, float4(node_856,0.0).x), float4(float4(node_856,0.0).x, node_3955_p.yzx), step(node_3955_p.x, float4(node_856,0.0).x));
                float node_3955_d = node_3955_q.x - min(node_3955_q.w, node_3955_q.y);
                float node_3955_e = 1.0e-10;
                float3 node_3955 = float3(abs(node_3955_q.z + (node_3955_q.w - node_3955_q.y) / (6.0 * node_3955_d + node_3955_e)), node_3955_d / (node_3955_q.x + node_3955_e), node_3955_q.x);;
                float3 diffuseColor = (lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac((_Hue+node_3955.r)+float3(0.0,-1.0/3.0,1.0/3.0)))-1),node_3955.g)*node_3955.b);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Legacy Shaders/Diffuse Detail"
}
