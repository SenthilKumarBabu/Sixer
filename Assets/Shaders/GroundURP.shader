Shader "Custom/GroundURP"
{
    Properties
    {
        _Grass ("Grass Texture", 2D) = "white" {}
        _Dirt ("Dirt Texture (A=Blend)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            // Lighting variants
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            // URP includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float  fogFactor  : TEXCOORD3;
            };

            TEXTURE2D(_Grass); SAMPLER(sampler_Grass);
            TEXTURE2D(_Dirt);  SAMPLER(sampler_Dirt);

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.uv = IN.uv;

                // Fog factor (new URP way)
                OUT.fogFactor = ComputeFogFactor(OUT.positionCS.z);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Sample textures
                half4 grassCol = SAMPLE_TEXTURE2D(_Grass, sampler_Grass, IN.uv);
                half4 dirtCol  = SAMPLE_TEXTURE2D(_Dirt, sampler_Dirt, IN.uv);

                // Blend using dirt alpha
                half3 baseColor = lerp(grassCol.rgb, dirtCol.rgb, dirtCol.a);

                // Lighting setup
                float3 normalWS = normalize(IN.normalWS);
                half3 color = half3(0,0,0);

                // Main light
                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(normalWS, mainLight.direction));
                color += baseColor * mainLight.color.rgb * NdotL;

                // Additional lights
                #ifdef _ADDITIONAL_LIGHTS
                uint lightCount = GetAdditionalLightsCount();
                for (uint i = 0; i < lightCount; i++)
                {
                    Light light = GetAdditionalLight(i, IN.positionWS);
                    float3 L = normalize(light.direction);
                    float NdotL_add = saturate(dot(normalWS, L));
                    color += baseColor * light.color.rgb * NdotL_add;
                }
                #endif

                // Ambient (spherical harmonics probe)
                color += baseColor * SampleSH(normalWS);

                half4 finalCol = half4(color, 1.0);

                // Fog (new URP way)
                finalCol.rgb = MixFog(finalCol.rgb, IN.fogFactor);

                return finalCol;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
