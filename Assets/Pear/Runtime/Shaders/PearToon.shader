Shader "Pear/Toon"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)

        [Normal] _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0, 2)) = 1

        _LightIntensity ("Light Intensity", Range(0, 4)) = 1
        _AmbientColor ("Ambient Color", Color) = (0.15, 0.15, 0.15, 1)

        _ToonThreshold ("Light Threshold", Range(0, 1)) = 0.65
        _ToonSoftness ("Light Softness", Range(0.001, 1)) = 0.04
        _ShadowColor ("Shadow Color", Color) = (0.45, 0.5, 0.65, 1)

        [Toggle(_PEAR_MIDTONE_ON)] _MidtoneEnabled ("Enable Midtone Band", Float) = 0
        _MidtoneColor ("Midtone Color", Color) = (0.75, 0.8, 0.95, 1)
        _MidtoneThreshold ("Midtone Threshold", Range(0, 1)) = 0.35
        _MidtoneSoftness ("Midtone Softness", Range(0.001, 1)) = 0.04

        [Toggle(_PEAR_RAMP_ON)] _RampEnabled ("Enable Ramp Texture", Float) = 0
        _RampTex ("Ramp Texture", 2D) = "white" {}
        _RampStrength ("Ramp Strength", Range(0, 1)) = 1

        [Toggle(_PEAR_RIM_ON)] _RimEnabled ("Enable Rim Light", Float) = 0
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", Range(0.25, 8)) = 3
        _RimIntensity ("Rim Intensity", Range(0, 5)) = 1

        [Toggle(_PEAR_EMISSION_ON)] _EmissionEnabled ("Enable Emission", Float) = 0
        _EmissionColor ("Emission Color", Color) = (0, 1, 1, 1)
        _EmissionMask ("Emission Mask", 2D) = "white" {}
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1
        _EmissionPulseSpeed ("Emission Pulse Speed", Range(0, 10)) = 0
        _EmissionPulseAmount ("Emission Pulse Amount", Range(0, 1)) = 0

        [Toggle(_PEAR_MATCAP_ON)] _MatcapEnabled ("Enable Matcap", Float) = 0
        _MatcapTex ("Matcap Texture", 2D) = "gray" {}
        _MatcapColor ("Matcap Color", Color) = (1, 1, 1, 1)
        _MatcapIntensity ("Matcap Intensity", Range(0, 5)) = 1
        _MatcapBlend ("Matcap Blend", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"

            Tags
            {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #pragma shader_feature_local _PEAR_RIM_ON
            #pragma shader_feature_local _PEAR_EMISSION_ON
            #pragma shader_feature_local _PEAR_MIDTONE_ON
            #pragma shader_feature_local _PEAR_RAMP_ON

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma shader_feature_local _PEAR_RIM_ON
            #pragma shader_feature_local _PEAR_EMISSION_ON
            #pragma shader_feature_local _PEAR_MIDTONE_ON
            #pragma shader_feature_local _PEAR_RAMP_ON
            #pragma shader_feature_local _PEAR_MATCAP_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            TEXTURE2D(_EmissionMask);
            SAMPLER(sampler_EmissionMask);

            TEXTURE2D(_RampTex);
            SAMPLER(sampler_RampTex);

            TEXTURE2D(_MatcapTex);
            SAMPLER(sampler_MatcapTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;

                float4 _NormalMap_ST;
                float _NormalStrength;

                float _LightIntensity;
                float4 _AmbientColor;

                float _ToonThreshold;
                float _ToonSoftness;
                float4 _ShadowColor;

                float _MidtoneEnabled;
                float4 _MidtoneColor;
                float _MidtoneThreshold;
                float _MidtoneSoftness;

                float _RampEnabled;
                float4 _RampTex_ST;
                float _RampStrength;

                float _RimEnabled;
                float4 _RimColor;
                float _RimPower;
                float _RimIntensity;

                float _EmissionEnabled;
                float4 _EmissionColor;
                float4 _EmissionMask_ST;
                float _EmissionIntensity;
                float _EmissionPulseSpeed;
                float _EmissionPulseAmount;

                float _MatcapEnabled;
                float4 _MatcapTex_ST;
                float4 _MatcapColor;
                float _MatcapIntensity;
                float _MatcapBlend;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 bitangentWS : TEXCOORD3;
                float3 viewDirWS : TEXCOORD5;
                float2 uv : TEXCOORD4;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);

                output.normalWS = normalInputs.normalWS;
                output.tangentWS = normalInputs.tangentWS;
                output.bitangentWS = normalInputs.bitangentWS;

                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return output;
            }

            half3 GetNormalWS(Varyings input)
            {
                half4 normalSample = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv);
                half3 normalTS = UnpackNormalScale(normalSample, _NormalStrength);

                half3x3 tangentToWorld = half3x3(
                    normalize(input.tangentWS),
                    normalize(input.bitangentWS),
                    normalize(input.normalWS)
                );

                return normalize(TransformTangentToWorld(normalTS, tangentToWorld));
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 baseTexture = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half3 albedo = baseTexture.rgb * _BaseColor.rgb;

                half3 normalWS = GetNormalWS(input);

                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord);

                half NdotL = saturate(dot(normalWS, mainLight.direction));

                half shadowAttenuation = mainLight.shadowAttenuation;

                half rawLightAmount = NdotL * shadowAttenuation;

                half3 litColor = albedo * mainLight.color;
                half3 shadowColor = albedo * _ShadowColor.rgb;

                #if defined(_PEAR_RAMP_ON)
                    half rampU = saturate(rawLightAmount);
                    half3 rampColor = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, half2(rampU, 0.5h)).rgb;

                    half3 regularLighting = lerp(shadowColor, litColor, rampU);
                    half3 rampLighting = albedo * rampColor * mainLight.color;

                    half3 toonLighting = lerp(regularLighting, rampLighting, _RampStrength);
                #else
                    #if defined(_PEAR_MIDTONE_ON)
                        half3 midtoneColor = albedo * _MidtoneColor.rgb;

                        half midtoneBand = smoothstep(
                            _MidtoneThreshold - _MidtoneSoftness,
                            _MidtoneThreshold + _MidtoneSoftness,
                            rawLightAmount
                        );

                        half litBand = smoothstep(
                            _ToonThreshold - _ToonSoftness,
                            _ToonThreshold + _ToonSoftness,
                            rawLightAmount
                        );

                        half3 toonLighting = lerp(shadowColor, midtoneColor, midtoneBand);
                        toonLighting = lerp(toonLighting, litColor, litBand);
                    #else
                        half toonLight = smoothstep(
                            _ToonThreshold - _ToonSoftness,
                            _ToonThreshold + _ToonSoftness,
                            rawLightAmount
                        );

                        half3 toonLighting = lerp(shadowColor, litColor, toonLight);
                    #endif
                #endif

                toonLighting *= _LightIntensity;
                toonLighting = saturate(toonLighting);

                half3 ambientLighting = albedo * _AmbientColor.rgb;

                half3 finalColor = toonLighting + ambientLighting;

                #if defined(_PEAR_RIM_ON)
                    half3 viewDirWS = normalize(input.viewDirWS);

                    half rimAmount = 1.0h - saturate(dot(normalWS, viewDirWS));
                    rimAmount = pow(rimAmount, _RimPower);

                    finalColor += _RimColor.rgb * rimAmount * _RimIntensity;
                #endif

                #if defined(_PEAR_EMISSION_ON)
                    half emissionMask = SAMPLE_TEXTURE2D(_EmissionMask, sampler_EmissionMask, input.uv).r;

                    half pulse = 1.0h;
                    if (_EmissionPulseSpeed > 0.001h && _EmissionPulseAmount > 0.001h)
                    {
                        half wave = sin(_Time.y * _EmissionPulseSpeed) * 0.5h + 0.5h;
                        pulse = lerp(1.0h, wave, _EmissionPulseAmount);
                    }

                    finalColor += _EmissionColor.rgb * emissionMask * _EmissionIntensity * pulse;
                #endif

                #if defined(_PEAR_MATCAP_ON)
                    half3 normalVS = TransformWorldToViewDir(normalWS, true);
                    half2 matcapUV = normalVS.xy * 0.5h + 0.5h;

                    half3 matcapSample = SAMPLE_TEXTURE2D(_MatcapTex, sampler_MatcapTex, matcapUV).rgb;
                    half3 matcapLayer = matcapSample * _MatcapColor.rgb * _MatcapIntensity;

                    finalColor = lerp(finalColor, finalColor + matcapLayer, _MatcapBlend);
                #endif

                return half4(finalColor, baseTexture.a * _BaseColor.a);
                }

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"

            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;

                float4 _NormalMap_ST;
                float _NormalStrength;

                float _LightIntensity;
                float4 _AmbientColor;

                float _ToonThreshold;
                float _ToonSoftness;
                float4 _ShadowColor;

                float _MidtoneEnabled;
                float4 _MidtoneColor;
                float _MidtoneThreshold;
                float _MidtoneSoftness;

                float _RampEnabled;
                float4 _RampTex_ST;
                float _RampStrength;

                float _RimEnabled;
                float4 _RimColor;
                float _RimPower;
                float _RimIntensity;

                float _EmissionEnabled;
                float4 _EmissionColor;
                float4 _EmissionMask_ST;
                float _EmissionIntensity;
                float _EmissionPulseSpeed;
                float _EmissionPulseAmount;

                float _MatcapEnabled;
                float4 _MatcapTex_ST;
                float4 _MatcapColor;
                float _MatcapIntensity;
                float _MatcapBlend;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);

                float3 lightDirectionWS = GetMainLight().direction;

                output.positionCS = TransformWorldToHClip(
                    ApplyShadowBias(
                        positionInputs.positionWS,
                        normalInputs.normalWS,
                        lightDirectionWS
                    )
                );

                #if UNITY_REVERSED_Z
                    output.positionCS.z = min(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #else
                    output.positionCS.z = max(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #endif

                return output;
            }

            half4 ShadowPassFragment(Varyings input) : SV_Target
            {
                return 0;
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"

    CustomEditor "Pear.Editor.PearToonShaderGUI"
}