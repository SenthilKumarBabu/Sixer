Shader "Hidden/SSAO"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Float) = 0.5
        _Bias ("Bias", Float) = 0.025
        _Intensity ("Intensity", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        ZWrite Off ZTest Always Cull Off

        Pass
        {
            Name "SSAO"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

            // ---------------------------------
            // SSAO KERNEL
            #define SAMPLE_COUNT 16
            static const float3 sampleKernel[SAMPLE_COUNT] = {
                float3(0.5381, 0.1856, -0.4319),
                float3(0.1379, 0.2486, 0.4430),
                float3(0.3371, 0.5679, -0.0057),
                float3(-0.6999,-0.0451, -0.0019),
                float3(0.0689,-0.1598,-0.8547),
                float3(0.0560, 0.0069,-0.1843),
                float3(-0.0146, 0.1402, 0.0762),
                float3(0.0100,-0.1924,-0.0344),
                float3(-0.3577,-0.5301,-0.4358),
                float3(-0.3169, 0.1063, 0.0158),
                float3(0.0103,-0.5869, 0.0046),
                float3(-0.0897,-0.4940, 0.3287),
                float3(0.7119,-0.0154,-0.0918),
                float3(-0.0533, 0.0596,-0.5411),
                float3(0.0352,-0.0631, 0.5460),
                float3(-0.4776, 0.2847,-0.0271)
            };

            // ---------------------------------
            // Properties
            TEXTURE2D(_MainTex);       SAMPLER(sampler_MainTex);

            float _Radius;
            float _Bias;
            float _Intensity;

            float4x4 _CameraViewProj;            // UNITY_MATRIX_VP
            float4x4 _CameraViewProjInv;         // inverse(VP)

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            Varyings Vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            float3 ReconstructPositionWS(float2 uv, float depth01)
            {
                float4 ndc = float4(uv * 2.0 - 1.0, depth01, 1.0);
                float4 posWS = mul(_CameraViewProjInv, ndc);
                posWS /= posWS.w;
                return posWS.xyz;
            }

            half4 Frag (Varyings i) : SV_Target
            {
                float depth = SampleSceneDepth(i.uv);   // from DeclareDepthTexture.hlsl
                if (depth >= 1.0)
                    return half4(1,1,1,1);

                float3 normal = SampleSceneNormals(i.uv); // from DeclareNormalsTexture.hlsl
                float3 posWS  = ReconstructPositionWS(i.uv, depth);

                float occlusion = 0.0;

                [unroll]
                for (int j = 0; j < SAMPLE_COUNT; j++)
                {
                    float3 samplePos = posWS + sampleKernel[j] * _Radius;
                    float4 offsetCS  = mul(_CameraViewProj, float4(samplePos,1.0));
                    float2 offsetUV  = (offsetCS.xy / offsetCS.w) * 0.5 + 0.5;

                    float sampleDepth = SampleSceneDepth(offsetUV);
                    float3 sampleWS   = ReconstructPositionWS(offsetUV, sampleDepth);

                    float rangeCheck = smoothstep(0.0, 1.0, _Radius / abs(posWS.z - sampleWS.z));
                    occlusion += (sampleWS.z >= samplePos.z + _Bias ? 1.0 : 0.0) * rangeCheck;
                }

                occlusion = 1.0 - (occlusion / SAMPLE_COUNT);
                return half4(pow(occlusion, _Intensity).xxx, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
