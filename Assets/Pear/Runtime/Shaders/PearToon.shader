Shader "Pear/URP/Toon"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)

        [Toggle(_PEAR_HUE_SHIFT_ON)] _HueShiftEnabled ("Enable Texture Hue Shift", Float) = 0
        _HueShift ("Hue Shift", Range(-180, 180)) = 0
        _HueSaturation ("Saturation", Range(0, 2)) = 1
        _HueBrightness ("Brightness", Range(0, 2)) = 1
        _HueShiftMask ("Hue Shift Mask", 2D) = "white" {}

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

        [Toggle(_PEAR_SHIMMER_ON)] _ShimmerEnabled ("Enable Glitter Shimmer", Float) = 0
        _ShimmerColor ("Shimmer Color", Color) = (1, 1, 0.65, 1)
        _ShimmerIntensity ("Shimmer Intensity", Range(0, 5)) = 0.8
        _ShimmerScale ("Shimmer Scale", Range(4, 150)) = 48
        _ShimmerSpeed ("Shimmer Speed", Range(0, 20)) = 4
        _ShimmerThreshold ("Shimmer Rarity", Range(0, 0.995)) = 0.92
        _ShimmerViewStrength ("View Angle Boost", Range(0, 1)) = 0.6

        [Toggle(_PEAR_OUTLINE_ON)] _OutlineEnabled ("Enable Outline", Float) = 0
        _OutlineColor ("Outline Color", Color) = (0.05, 0.06, 0.05, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.08)) = 0.015
        _OutlineDepthOffset ("Outline Depth Offset", Range(-0.02, 0.02)) = 0
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
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #pragma shader_feature_local _PEAR_HUE_SHIFT_ON
            #pragma shader_feature_local _PEAR_MIDTONE_ON
            #pragma shader_feature_local _PEAR_RAMP_ON
            #pragma shader_feature_local _PEAR_RIM_ON
            #pragma shader_feature_local _PEAR_EMISSION_ON
            #pragma shader_feature_local _PEAR_MATCAP_ON
            #pragma shader_feature_local _PEAR_SHIMMER_ON

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_HueShiftMask);
            SAMPLER(sampler_HueShiftMask);

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

                float _HueShiftEnabled;
                float _HueShift;
                float _HueSaturation;
                float _HueBrightness;
                float4 _HueShiftMask_ST;

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

                float _ShimmerEnabled;
                float4 _ShimmerColor;
                float _ShimmerIntensity;
                float _ShimmerScale;
                float _ShimmerSpeed;
                float _ShimmerThreshold;
                float _ShimmerViewStrength;

                float _OutlineEnabled;
                float4 _OutlineColor;
                float _OutlineWidth;
                float _OutlineDepthOffset;
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
                float2 uv : TEXCOORD4;
                float3 viewDirWS : TEXCOORD5;
            };

            float Hash13(float3 p)
            {
                p = frac(p * 0.1031);
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float3 PearRgbToHsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;

                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 PearHsvToRgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            half3 ApplyTextureHueShift(half3 color, float2 uv)
            {
                #if defined(_PEAR_HUE_SHIFT_ON)
                    half mask = SAMPLE_TEXTURE2D(_HueShiftMask, sampler_HueShiftMask, uv).r;
                    float3 hsv = PearRgbToHsv(saturate(color));

                    hsv.x = frac(hsv.x + (_HueShift / 360.0) * mask);
                    hsv.y = saturate(hsv.y * lerp(1.0, _HueSaturation, mask));
                    hsv.z = saturate(hsv.z * lerp(1.0, _HueBrightness, mask));

                    return PearHsvToRgb(hsv);
                #else
                    return color;
                #endif
            }

            Varyings Vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.normalWS = normalInputs.normalWS;
                output.tangentWS = normalInputs.tangentWS;
                output.bitangentWS = normalInputs.bitangentWS;
                output.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);
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
                half3 shiftedBaseColor = ApplyTextureHueShift(baseTexture.rgb, input.uv);
                half3 albedo = shiftedBaseColor * _BaseColor.rgb;

                half3 normalWS = GetNormalWS(input);
                half3 viewDirWS = normalize(input.viewDirWS);

                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord);

                half NdotL = saturate(dot(normalWS, mainLight.direction));
                half rawLightAmount = NdotL * mainLight.shadowAttenuation;

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

                #if defined(_PEAR_SHIMMER_ON)
                    float shimmerFrame = floor(_Time.y * max(_ShimmerSpeed, 0.001));
                    float3 shimmerCell = floor((input.positionWS + normalWS * 0.37) * _ShimmerScale);
                    half shimmerNoise = Hash13(shimmerCell + shimmerFrame);
                    half shimmerMask = smoothstep(_ShimmerThreshold, 1.0h, shimmerNoise);

                    half shimmerFresnel = 1.0h - saturate(dot(normalWS, viewDirWS));
                    shimmerFresnel = pow(shimmerFresnel, 1.5h);
                    half viewBoost = lerp(1.0h, shimmerFresnel, _ShimmerViewStrength);

                    finalColor += _ShimmerColor.rgb * shimmerMask * _ShimmerIntensity * viewBoost;
                #endif

                return half4(finalColor, baseTexture.a * _BaseColor.a);
            }

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

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

                float _HueShiftEnabled;
                float _HueShift;
                float _HueSaturation;
                float _HueBrightness;
                float4 _HueShiftMask_ST;

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

                float _ShimmerEnabled;
                float4 _ShimmerColor;
                float _ShimmerIntensity;
                float _ShimmerScale;
                float _ShimmerSpeed;
                float _ShimmerThreshold;
                float _ShimmerViewStrength;

                float _OutlineEnabled;
                float4 _OutlineColor;
                float _OutlineWidth;
                float _OutlineDepthOffset;
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

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM

            #pragma vertex OutlineVertex
            #pragma fragment OutlineFragment
            #pragma shader_feature_local _PEAR_OUTLINE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;

                float _HueShiftEnabled;
                float _HueShift;
                float _HueSaturation;
                float _HueBrightness;
                float4 _HueShiftMask_ST;

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

                float _ShimmerEnabled;
                float4 _ShimmerColor;
                float _ShimmerIntensity;
                float _ShimmerScale;
                float _ShimmerSpeed;
                float _ShimmerThreshold;
                float _ShimmerViewStrength;

                float _OutlineEnabled;
                float4 _OutlineColor;
                float _OutlineWidth;
                float _OutlineDepthOffset;
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

            Varyings OutlineVertex(Attributes input)
            {
                Varyings output;

                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                positionWS += normalize(normalWS) * _OutlineWidth;

                output.positionCS = TransformWorldToHClip(positionWS);
                output.positionCS.z += _OutlineDepthOffset * output.positionCS.w;

                return output;
            }

            half4 OutlineFragment(Varyings input) : SV_Target
            {
                #if defined(_PEAR_OUTLINE_ON)
                    return _OutlineColor;
                #else
                    clip(-1);
                    return 0;
                #endif
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "Pear.Editor.PearToonShaderGUI"
}
