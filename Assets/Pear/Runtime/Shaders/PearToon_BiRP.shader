Shader "Pear/BiRP/Toon"
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
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardBase"
            Tags { "LightMode" = "ForwardBase" }

            Cull Back
            ZWrite On
            ZTest LEqual

            CGPROGRAM

            #pragma target 3.0
            #pragma vertex Vert
            #pragma fragment Frag

            #pragma multi_compile_fwdbase

            #pragma shader_feature_local _PEAR_HUE_SHIFT_ON
            #pragma shader_feature_local _PEAR_MIDTONE_ON
            #pragma shader_feature_local _PEAR_RAMP_ON
            #pragma shader_feature_local _PEAR_RIM_ON
            #pragma shader_feature_local _PEAR_EMISSION_ON
            #pragma shader_feature_local _PEAR_MATCAP_ON
            #pragma shader_feature_local _PEAR_SHIMMER_ON

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float4 _BaseColor;

            float _HueShiftEnabled;
            float _HueShift;
            float _HueSaturation;
            float _HueBrightness;
            sampler2D _HueShiftMask;
            float4 _HueShiftMask_ST;

            sampler2D _NormalMap;
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
            sampler2D _RampTex;
            float4 _RampTex_ST;
            float _RampStrength;

            float _RimEnabled;
            float4 _RimColor;
            float _RimPower;
            float _RimIntensity;

            float _EmissionEnabled;
            float4 _EmissionColor;
            sampler2D _EmissionMask;
            float4 _EmissionMask_ST;
            float _EmissionIntensity;
            float _EmissionPulseSpeed;
            float _EmissionPulseAmount;

            float _MatcapEnabled;
            sampler2D _MatcapTex;
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

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
                float3 viewDirWS : TEXCOORD5;
                SHADOW_COORDS(6)
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

            fixed3 ApplyTextureHueShift(fixed3 color, float2 uv)
            {
                #if defined(_PEAR_HUE_SHIFT_ON)
                    fixed mask = tex2D(_HueShiftMask, uv).r;
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

                output.pos = UnityObjectToClipPos(input.vertex);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                output.worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
                output.normalWS = UnityObjectToWorldNormal(input.normal);
                output.tangentWS = UnityObjectToWorldDir(input.tangent.xyz);

                float tangentSign = input.tangent.w * unity_WorldTransformParams.w;
                output.bitangentWS = cross(output.normalWS, output.tangentWS) * tangentSign;

                output.normalWS = normalize(output.normalWS);
                output.tangentWS = normalize(output.tangentWS);
                output.bitangentWS = normalize(output.bitangentWS);

                output.viewDirWS = UnityWorldSpaceViewDir(output.worldPos);

                TRANSFER_SHADOW(output);

                return output;
            }

            fixed3 GetNormalWS(Varyings input)
            {
                fixed4 normalSample = tex2D(_NormalMap, input.uv);
                fixed3 normalTS = UnpackNormal(normalSample);
                normalTS.xy *= _NormalStrength;
                normalTS.z = sqrt(saturate(1.0 - dot(normalTS.xy, normalTS.xy)));

                float3x3 tangentToWorld = float3x3(
                    normalize(input.tangentWS),
                    normalize(input.bitangentWS),
                    normalize(input.normalWS)
                );

                return normalize(mul(normalTS, tangentToWorld));
            }

            fixed4 Frag(Varyings input) : SV_Target
            {
                fixed4 baseTexture = tex2D(_BaseMap, input.uv);
                fixed3 shiftedBaseColor = ApplyTextureHueShift(baseTexture.rgb, input.uv);
                fixed3 albedo = shiftedBaseColor * _BaseColor.rgb;

                fixed3 normalWS = GetNormalWS(input);
                fixed3 viewDirWS = normalize(input.viewDirWS);

                fixed3 lightDirectionWS = normalize(_WorldSpaceLightPos0.xyz);
                fixed shadowAttenuation = SHADOW_ATTENUATION(input);

                fixed NdotL = saturate(dot(normalWS, lightDirectionWS));
                fixed rawLightAmount = NdotL * shadowAttenuation;

                fixed3 litColor = albedo * _LightColor0.rgb;
                fixed3 shadowColor = albedo * _ShadowColor.rgb;

                #if defined(_PEAR_RAMP_ON)
                    fixed rampU = saturate(rawLightAmount);
                    fixed3 rampColor = tex2D(_RampTex, float2(rampU, 0.5)).rgb;

                    fixed3 regularLighting = lerp(shadowColor, litColor, rampU);
                    fixed3 rampLighting = albedo * rampColor * _LightColor0.rgb;

                    fixed3 toonLighting = lerp(regularLighting, rampLighting, _RampStrength);
                #else
                    #if defined(_PEAR_MIDTONE_ON)
                        fixed3 midtoneColor = albedo * _MidtoneColor.rgb;

                        fixed midtoneBand = smoothstep(
                            _MidtoneThreshold - _MidtoneSoftness,
                            _MidtoneThreshold + _MidtoneSoftness,
                            rawLightAmount
                        );

                        fixed litBand = smoothstep(
                            _ToonThreshold - _ToonSoftness,
                            _ToonThreshold + _ToonSoftness,
                            rawLightAmount
                        );

                        fixed3 toonLighting = lerp(shadowColor, midtoneColor, midtoneBand);
                        toonLighting = lerp(toonLighting, litColor, litBand);
                    #else
                        fixed toonLight = smoothstep(
                            _ToonThreshold - _ToonSoftness,
                            _ToonThreshold + _ToonSoftness,
                            rawLightAmount
                        );

                        fixed3 toonLighting = lerp(shadowColor, litColor, toonLight);
                    #endif
                #endif

                toonLighting *= _LightIntensity;
                toonLighting = saturate(toonLighting);

                fixed3 ambientLighting = albedo * _AmbientColor.rgb;
                fixed3 finalColor = toonLighting + ambientLighting;

                #if defined(_PEAR_RIM_ON)
                    fixed rimAmount = 1.0 - saturate(dot(normalWS, viewDirWS));
                    rimAmount = pow(rimAmount, _RimPower);
                    finalColor += _RimColor.rgb * rimAmount * _RimIntensity;
                #endif

                #if defined(_PEAR_EMISSION_ON)
                    fixed emissionMask = tex2D(_EmissionMask, input.uv).r;

                    fixed pulse = 1.0;
                    if (_EmissionPulseSpeed > 0.001 && _EmissionPulseAmount > 0.001)
                    {
                        fixed wave = sin(_Time.y * _EmissionPulseSpeed) * 0.5 + 0.5;
                        pulse = lerp(1.0, wave, _EmissionPulseAmount);
                    }

                    finalColor += _EmissionColor.rgb * emissionMask * _EmissionIntensity * pulse;
                #endif

                #if defined(_PEAR_MATCAP_ON)
                    fixed3 normalVS = mul((float3x3)UNITY_MATRIX_V, normalWS);
                    fixed2 matcapUV = normalVS.xy * 0.5 + 0.5;

                    fixed3 matcapSample = tex2D(_MatcapTex, matcapUV).rgb;
                    fixed3 matcapLayer = matcapSample * _MatcapColor.rgb * _MatcapIntensity;

                    finalColor = lerp(finalColor, finalColor + matcapLayer, _MatcapBlend);
                #endif

                #if defined(_PEAR_SHIMMER_ON)
                    float shimmerFrame = floor(_Time.y * max(_ShimmerSpeed, 0.001));
                    float3 shimmerCell = floor((input.worldPos + normalWS * 0.37) * _ShimmerScale);
                    fixed shimmerNoise = Hash13(shimmerCell + shimmerFrame);
                    fixed shimmerMask = smoothstep(_ShimmerThreshold, 1.0, shimmerNoise);

                    fixed shimmerFresnel = 1.0 - saturate(dot(normalWS, viewDirWS));
                    shimmerFresnel = pow(shimmerFresnel, 1.5);
                    fixed viewBoost = lerp(1.0, shimmerFresnel, _ShimmerViewStrength);

                    finalColor += _ShimmerColor.rgb * shimmerMask * _ShimmerIntensity * viewBoost;
                #endif

                return fixed4(finalColor, baseTexture.a * _BaseColor.a);
            }

            ENDCG
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            CGPROGRAM

            #pragma target 3.0
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct Varyings
            {
                V2F_SHADOW_CASTER;
            };

            Varyings Vert(appdata_base v)
            {
                Varyings o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 Frag(Varyings i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }

            ENDCG
        }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "Always" }

            Cull Front
            ZWrite On
            ZTest LEqual

            CGPROGRAM

            #pragma target 3.0
            #pragma vertex OutlineVertex
            #pragma fragment OutlineFragment
            #pragma shader_feature_local _PEAR_OUTLINE_ON

            #include "UnityCG.cginc"

            float4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineDepthOffset;

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
            };

            Varyings OutlineVertex(Attributes input)
            {
                Varyings output;

                float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
                float3 normalWS = UnityObjectToWorldNormal(input.normal);

                worldPos += normalize(normalWS) * _OutlineWidth;

                output.pos = UnityWorldToClipPos(worldPos);
                output.pos.z += _OutlineDepthOffset * output.pos.w;

                return output;
            }

            fixed4 OutlineFragment(Varyings input) : SV_Target
            {
                #if defined(_PEAR_OUTLINE_ON)
                    return _OutlineColor;
                #else
                    clip(-1);
                    return 0;
                #endif
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
    CustomEditor "Pear.Editor.PearToonShaderGUI"
}
