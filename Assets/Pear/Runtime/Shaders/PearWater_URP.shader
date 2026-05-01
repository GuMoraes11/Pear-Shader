Shader "Pear/URP/Water"
{
    Properties
    {
        [Header(Surface)]
        _BaseColor ("Base Water Color", Color) = (0.45, 0.9, 0.75, 0.55)
        _Opacity ("Opacity", Range(0, 1)) = 0.55
        _TopSurfaceOnly ("Top Surface Only", Float) = 1
        _SurfaceTintStrength ("Surface Tint Strength", Range(0, 2)) = 1.0

        [Header(Toon Lighting)]
        _LightIntensity ("Light Intensity", Range(0, 3)) = 1.0
        _AmbientColor ("Ambient Color", Color) = (0.22, 0.42, 0.36, 1)
        _ShadowColor ("Shadow Color", Color) = (0.18, 0.38, 0.32, 1)
        _ToonThreshold ("Shadow Size", Range(0, 1)) = 0.45
        _ToonSoftness ("Edge Softness", Range(0.001, 1)) = 0.28

        [Header(Rim Highlight)]
        _RimEnabled ("Rim Enabled", Float) = 1
        _RimColor ("Rim Color", Color) = (0.9, 1.0, 0.72, 1)
        _RimPower ("Rim Power", Range(0.1, 12)) = 3.0
        _RimIntensity ("Rim Intensity", Range(0, 4)) = 0.85

        [Header(Subsurface Glow)]
        _SubsurfaceEnabled ("Subsurface Glow Enabled", Float) = 1
        _SubsurfaceColor ("Glow Color", Color) = (0.72, 1.0, 0.72, 1)
        _SubsurfaceStrength ("Glow Strength", Range(0, 3)) = 0.35
        _SubsurfaceViewPower ("View Power", Range(0.1, 8)) = 2.2
        _SubsurfaceLightStrength ("Light Influence", Range(0, 2)) = 0.55
        _SubsurfaceDepthStrength ("Depth Influence", Range(0, 2)) = 0.35
        _SubsurfaceDepthDistance ("Depth Distance", Range(0.001, 5)) = 1.2

        [Header(Waves)]
        _WaveNormalA ("Wave Normal A", 2D) = "bump" {}
        _WaveNormalB ("Wave Normal B", 2D) = "bump" {}
        _WaveStrength ("Texture Wave Strength", Range(0, 2)) = 0.25
        _WaveScale ("Texture Wave Scale", Range(0.01, 10)) = 1.0
        _WaveSpeedA ("Wave Speed A", Vector) = (0.05, 0.03, 0, 0)
        _WaveSpeedB ("Wave Speed B", Vector) = (-0.035, 0.045, 0, 0)
        _ProceduralWaveStrength ("Procedural Wave Strength", Range(0, 2)) = 0.22
        _ProceduralWaveScale ("Procedural Wave Scale", Range(0.1, 30)) = 5.0
        _ProceduralWaveSpeed ("Procedural Wave Speed", Range(0, 10)) = 0.35

        [Header(Foam)]
        _FoamEnabled ("Foam Enabled", Float) = 1
        _FoamNoise ("Foam Noise", 2D) = "white" {}
        _FoamColor ("Foam Color", Color) = (0.95, 1.0, 0.75, 1)
        _FoamAmount ("Foam Amount", Range(0, 1)) = 0.08
        _FoamScale ("Foam Scale", Range(0.01, 20)) = 8.0
        _FoamSpeed ("Foam Speed", Vector) = (0.03, 0.02, 0, 0)
        _FoamThreshold ("Foam Rarity", Range(0, 1)) = 0.88
        _FoamSoftness ("Foam Softness", Range(0.001, 1)) = 0.18
        _FoamTextureBlend ("Foam Texture Blend", Range(0, 1)) = 0.0

        [Header(Contact Foam)]
        _ContactFoamEnabled ("Contact Foam Enabled", Float) = 1
        _ContactFoamColor ("Contact Foam Color", Color) = (0.95, 1.0, 0.72, 1)
        _ContactFoamStrength ("Contact Foam Strength", Range(0, 3)) = 0.65
        _ContactFoamDistance ("Contact Distance", Range(0.001, 5)) = 0.25
        _ContactFoamSoftness ("Edge Softness", Range(0.001, 1)) = 0.18
        _ContactFoamNoiseAmount ("Breakup Amount", Range(0, 1)) = 0.35
        _ContactFoamNoiseScale ("Breakup Scale", Range(0.1, 30)) = 7.0
        _ContactFoamNoiseSpeed ("Breakup Speed", Vector) = (0.04, 0.025, 0, 0)

        [Header(Surface Flow)]
        _FlowEnabled ("Surface Flow Enabled", Float) = 1
        _FlowColor ("Flow Highlight Color", Color) = (0.88, 1.0, 0.7, 1)
        _FlowIntensity ("Flow Brightness", Range(0, 3)) = 0.28
        _FlowScale ("Flow Scale", Range(0.1, 30)) = 6.0
        _FlowSpeed ("Flow Speed", Range(-5, 5)) = 0.35
        _FlowThickness ("Flow Line Thickness", Range(0.001, 1)) = 0.18
        _FlowDistortion ("Flow Distortion", Range(0, 2)) = 0.55

        [Header(Sparkle)]
        _SparkleEnabled ("Sparkle Enabled", Float) = 1
        _SparkleColor ("Sparkle Color", Color) = (0.95, 1.0, 0.65, 1)
        _SparkleIntensity ("Sparkle Brightness", Range(0, 8)) = 1.25
        _SparkleAmount ("Sparkle Amount", Range(0, 1)) = 0.35
        _SparkleSize ("Sparkle Size", Range(0.01, 0.25)) = 0.085
        _SparkleScale ("Sparkle Density", Range(1, 300)) = 95
        _SparkleSpeed ("Flicker Speed", Range(0, 20)) = 1.5
        _SparkleThreshold ("Advanced Sparkle Rarity", Range(0, 1)) = 0.985
        _SparkleViewStrength ("View Angle Boost", Range(0, 3)) = 1.1

        [Header(Vertex Wave)]
        _VertexWaveEnabled ("Mesh Wave Enabled", Float) = 0
        _VertexWaveAmount ("Mesh Wave Amount", Range(0, 0.12)) = 0.012
        _VertexWaveScale ("Mesh Wave Size", Range(0.01, 10)) = 1.2
        _VertexWaveSpeed ("Mesh Wave Speed", Range(0, 10)) = 0.6
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
        }

        Pass
        {
            Name "Pear Water Forward"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #pragma shader_feature_local _PEAR_WATER_RIM_ON
            #pragma shader_feature_local _PEAR_WATER_SUBSURFACE_ON
            #pragma shader_feature_local _PEAR_WATER_FOAM_ON
            #pragma shader_feature_local _PEAR_WATER_CONTACT_FOAM_ON
            #pragma shader_feature_local _PEAR_WATER_FLOW_ON
            #pragma shader_feature_local _PEAR_WATER_SPARKLE_ON
            #pragma shader_feature_local _PEAR_WATER_VERTEX_WAVE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_WaveNormalA);
            SAMPLER(sampler_WaveNormalA);
            TEXTURE2D(_WaveNormalB);
            SAMPLER(sampler_WaveNormalB);
            TEXTURE2D(_FoamNoise);
            SAMPLER(sampler_FoamNoise);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Opacity;
                float _TopSurfaceOnly;
                float _SurfaceTintStrength;

                float _LightIntensity;
                float4 _AmbientColor;
                float4 _ShadowColor;
                float _ToonThreshold;
                float _ToonSoftness;

                float _RimEnabled;
                float4 _RimColor;
                float _RimPower;
                float _RimIntensity;

                float _SubsurfaceEnabled;
                float4 _SubsurfaceColor;
                float _SubsurfaceStrength;
                float _SubsurfaceViewPower;
                float _SubsurfaceLightStrength;
                float _SubsurfaceDepthStrength;
                float _SubsurfaceDepthDistance;

                float4 _WaveNormalA_ST;
                float4 _WaveNormalB_ST;
                float _WaveStrength;
                float _WaveScale;
                float4 _WaveSpeedA;
                float4 _WaveSpeedB;
                float _ProceduralWaveStrength;
                float _ProceduralWaveScale;
                float _ProceduralWaveSpeed;

                float _FoamEnabled;
                float4 _FoamNoise_ST;
                float4 _FoamColor;
                float _FoamAmount;
                float _FoamScale;
                float4 _FoamSpeed;
                float _FoamThreshold;
                float _FoamSoftness;
                float _FoamTextureBlend;

                float _ContactFoamEnabled;
                float4 _ContactFoamColor;
                float _ContactFoamStrength;
                float _ContactFoamDistance;
                float _ContactFoamSoftness;
                float _ContactFoamNoiseAmount;
                float _ContactFoamNoiseScale;
                float4 _ContactFoamNoiseSpeed;

                float _FlowEnabled;
                float4 _FlowColor;
                float _FlowIntensity;
                float _FlowScale;
                float _FlowSpeed;
                float _FlowThickness;
                float _FlowDistortion;

                float _SparkleEnabled;
                float4 _SparkleColor;
                float _SparkleIntensity;
                float _SparkleAmount;
                float _SparkleSize;
                float _SparkleScale;
                float _SparkleSpeed;
                float _SparkleThreshold;
                float _SparkleViewStrength;

                float _VertexWaveEnabled;
                float _VertexWaveAmount;
                float _VertexWaveScale;
                float _VertexWaveSpeed;
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
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float4 tangentWS : TEXCOORD2;
                float2 uv : TEXCOORD3;
                float3 viewDirWS : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
            };

            float Hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            float2 Hash22(float2 p)
            {
                float n = Hash21(p);
                return frac(float2(n, Hash21(p + n + 19.19)) * float2(437.31, 289.17));
            }

            float2 Rotate2D(float2 p, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                return float2(c * p.x - s * p.y, s * p.x + c * p.y);
            }

            float ValueNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                float a = Hash21(i);
                float b = Hash21(i + float2(1.0, 0.0));
                float c = Hash21(i + float2(0.0, 1.0));
                float d = Hash21(i + float2(1.0, 1.0));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float FBM(float2 uv)
            {
                float value = 0.0;
                float amp = 0.5;
                float freq = 1.0;

                [unroll]
                for (int i = 0; i < 4; i++)
                {
                    value += ValueNoise(uv * freq) * amp;
                    freq *= 2.0;
                    amp *= 0.5;
                }

                return value;
            }

            float RidgedNoise(float2 uv)
            {
                float n = ValueNoise(uv);
                return 1.0 - abs(n * 2.0 - 1.0);
            }

            float RidgedFBM(float2 uv)
            {
                float value = 0.0;
                float amp = 0.5;
                float freq = 1.0;

                [unroll]
                for (int i = 0; i < 4; i++)
                {
                    value += RidgedNoise(uv * freq) * amp;
                    uv = Rotate2D(uv * 1.07 + 3.17, 0.58);
                    freq *= 1.95;
                    amp *= 0.5;
                }

                return value;
            }

            Varyings Vert(Attributes input)
            {
                Varyings output;
                float3 positionOS = input.positionOS.xyz;

                #if defined(_PEAR_WATER_VERTEX_WAVE_ON)
                    if (_VertexWaveEnabled > 0.5)
                    {
                        float topMask = smoothstep(0.65, 0.95, input.normalOS.y);
                        float waveA = sin((positionOS.x + _Time.y * _VertexWaveSpeed) * _VertexWaveScale);
                        float waveB = cos((positionOS.z + _Time.y * _VertexWaveSpeed * 0.72) * _VertexWaveScale);
                        positionOS.y += waveA * waveB * _VertexWaveAmount * topMask;
                    }
                #endif

                VertexPositionInputs positionInputs = GetVertexPositionInputs(positionOS);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionHCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.normalWS = normalize(normalInputs.normalWS);
                output.tangentWS = float4(normalize(normalInputs.tangentWS), input.tangentOS.w);
                output.uv = input.uv;
                output.viewDirWS = normalize(GetWorldSpaceViewDir(positionInputs.positionWS));
                output.screenPos = ComputeScreenPos(output.positionHCS);

                return output;
            }

            float3 GetWaterNormal(Varyings input)
            {
                float2 uvA = TRANSFORM_TEX(input.uv, _WaveNormalA) * _WaveScale + _Time.y * _WaveSpeedA.xy;
                float2 uvB = TRANSFORM_TEX(input.uv, _WaveNormalB) * _WaveScale + _Time.y * _WaveSpeedB.xy;

                float3 normalA = UnpackNormalScale(SAMPLE_TEXTURE2D(_WaveNormalA, sampler_WaveNormalA, uvA), _WaveStrength);
                float3 normalB = UnpackNormalScale(SAMPLE_TEXTURE2D(_WaveNormalB, sampler_WaveNormalB, uvB), _WaveStrength);

                float2 procUV = input.uv * _ProceduralWaveScale;
                float time = _Time.y * _ProceduralWaveSpeed;
                float waveX = sin(procUV.x + time) + sin((procUV.x + procUV.y) * 0.72 + time * 1.35);
                float waveY = cos(procUV.y + time * 0.82) + cos((procUV.x - procUV.y) * 0.63 + time * 1.12);
                float3 proceduralNormal = normalize(float3(waveX, waveY, 4.0 / max(_ProceduralWaveStrength, 0.001)));

                float3 tangentNormal = normalize(float3(normalA.xy + normalB.xy + proceduralNormal.xy * _ProceduralWaveStrength, normalA.z * normalB.z));

                float3 normalWS = normalize(input.normalWS);
                float3 tangentWS = normalize(input.tangentWS.xyz);
                float tangentSign = input.tangentWS.w * GetOddNegativeScale();
                float3 bitangentWS = cross(normalWS, tangentWS) * tangentSign;

                float3x3 tangentToWorld = float3x3(tangentWS, bitangentWS, normalWS);
                return normalize(mul(tangentNormal, tangentToWorld));
            }

            half4 Frag(Varyings input) : SV_Target
            {
                if (_TopSurfaceOnly > 0.5 && input.normalWS.y < 0.25)
                {
                    discard;
                }

                float3 normalWS = GetWaterNormal(input);
                float3 viewDirWS = normalize(input.viewDirWS);

                Light mainLight = GetMainLight();
                float3 lightDirWS = normalize(mainLight.direction);

                float ndl = saturate(dot(normalWS, lightDirWS));
                float toon = smoothstep(_ToonThreshold - _ToonSoftness, _ToonThreshold + _ToonSoftness, ndl);

                float3 baseColor = _BaseColor.rgb;
                float3 litColor = baseColor * mainLight.color.rgb * _LightIntensity;
                float3 shadowColor = baseColor * _ShadowColor.rgb;
                float3 color = lerp(shadowColor, litColor, toon);
                color += _AmbientColor.rgb * baseColor * _SurfaceTintStrength;

                #if defined(_PEAR_WATER_RIM_ON)
                    if (_RimEnabled > 0.5)
                    {
                        float rim = pow(1.0 - saturate(dot(viewDirWS, normalWS)), _RimPower);
                        color += _RimColor.rgb * rim * _RimIntensity;
                    }
                #endif

                #if defined(_PEAR_WATER_SUBSURFACE_ON)
                    if (_SubsurfaceEnabled > 0.5)
                    {
                        // Stylized fake subsurface/transmission glow.
                        // This is not physically accurate SSS; it is an art-directable glow that helps water feel thicker and more luminous.
                        float viewScatter = pow(1.0 - saturate(dot(viewDirWS, normalWS)), _SubsurfaceViewPower);
                        float lightScatter = pow(saturate(dot(viewDirWS, -lightDirWS)), 2.0) * _SubsurfaceLightStrength;

                        float2 subsurfaceScreenUV = input.screenPos.xy / input.screenPos.w;
                        float rawSubsurfaceDepth = SampleSceneDepth(subsurfaceScreenUV);
                        float sceneSubsurfaceEyeDepth = LinearEyeDepth(rawSubsurfaceDepth, _ZBufferParams);
                        float waterSubsurfaceEyeDepth = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                        float subsurfaceDepthDiff = max(sceneSubsurfaceEyeDepth - waterSubsurfaceEyeDepth, 0.0);
                        float depthGlow = saturate(subsurfaceDepthDiff / max(_SubsurfaceDepthDistance, 0.001)) * _SubsurfaceDepthStrength;

                        float subsurface = saturate((viewScatter + lightScatter + depthGlow) * _SubsurfaceStrength);
                        color += _SubsurfaceColor.rgb * subsurface;
                    }
                #endif

                #if defined(_PEAR_WATER_FOAM_ON)
                    if (_FoamEnabled > 0.5)
                    {
                        float2 foamUV = input.uv * _FoamScale + _Time.y * _FoamSpeed.xy;
                        float proceduralFoam = FBM(foamUV);

                        float2 textureFoamUV = TRANSFORM_TEX(input.uv, _FoamNoise) * _FoamScale + _Time.y * _FoamSpeed.xy;
                        float textureFoam = SAMPLE_TEXTURE2D(_FoamNoise, sampler_FoamNoise, textureFoamUV).r;
                        float foamNoise = lerp(proceduralFoam, textureFoam, _FoamTextureBlend);

                        float foam = smoothstep(_FoamThreshold, saturate(_FoamThreshold + _FoamSoftness), foamNoise);
                        color = lerp(color, _FoamColor.rgb, foam * _FoamAmount);
                    }
                #endif

                #if defined(_PEAR_WATER_CONTACT_FOAM_ON)
                    if (_ContactFoamEnabled > 0.5)
                    {
                        float2 screenUV = input.screenPos.xy / input.screenPos.w;
                        float rawSceneDepth = SampleSceneDepth(screenUV);
                        float sceneEyeDepth = LinearEyeDepth(rawSceneDepth, _ZBufferParams);
                        float waterEyeDepth = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                        float depthDiff = sceneEyeDepth - waterEyeDepth;

                        // Only show contact foam where solid scene geometry is close behind the water surface.
                        float contact = 1.0 - smoothstep(0.0, max(_ContactFoamDistance, 0.001), depthDiff);
                        contact *= step(0.0001, depthDiff);

                        float2 contactUV = input.positionWS.xz * _ContactFoamNoiseScale + _Time.y * _ContactFoamNoiseSpeed.xy;
                        float breakup = FBM(contactUV);
                        float brokenContact = contact * lerp(1.0, smoothstep(0.18, 0.86, breakup), _ContactFoamNoiseAmount);
                        brokenContact = smoothstep(0.0, max(_ContactFoamSoftness, 0.001), brokenContact);

                        color = lerp(color, _ContactFoamColor.rgb, saturate(brokenContact * _ContactFoamStrength));
                    }
                #endif

                #if defined(_PEAR_WATER_FLOW_ON)
                    if (_FlowEnabled > 0.5)
                    {
                        // Organic surface motion.
                        // Older versions used stripe/ribbon patterns that could read like a grid.
                        // This version uses domain-warped ridged noise to create softer caustic-like patches.
                        float2 flowBase = input.positionWS.xz * max(_FlowScale, 0.001) * 0.24;
                        float flowTime = _Time.y * _FlowSpeed;

                        float2 driftA = float2(flowTime * 0.12, -flowTime * 0.08);
                        float2 driftB = float2(-flowTime * 0.09, flowTime * 0.14);

                        float2 warp;
                        warp.x = FBM(Rotate2D(flowBase * 1.13 + driftA + 5.1, 0.37));
                        warp.y = FBM(Rotate2D(flowBase * 1.41 + driftB - 11.2, -0.83));

                        flowBase += (warp - 0.5) * (_FlowDistortion * 3.2);

                        float veinsA = RidgedFBM(flowBase + driftA);
                        float veinsB = RidgedFBM(Rotate2D(flowBase * 1.71 + 17.3, 1.14) + driftB);
                        float veinsC = RidgedFBM(Rotate2D(flowBase * 0.66 - 23.6, -0.61) + float2(flowTime * 0.05, flowTime * 0.03));

                        float flowField = saturate(veinsA * 0.52 + veinsB * 0.33 + veinsC * 0.15);
                        float thickness = saturate(_FlowThickness);

                        // Thicker settings give broader highlight clusters, thinner settings give wispier detail.
                        float flow = smoothstep(0.58 - thickness * 0.18, 0.82 - thickness * 0.10, flowField);

                        // Soft breakup to avoid evenly distributed motion everywhere.
                        float breakup = smoothstep(0.34, 0.92, FBM(flowBase * 0.72 - driftB * 0.4));
                        flow *= breakup;

                        float fresnel = pow(1.0 - saturate(dot(viewDirWS, normalWS)), 1.5);
                        color += _FlowColor.rgb * flow * _FlowIntensity * (0.35 + fresnel * 0.65);
                    }
                #endif

                #if defined(_PEAR_WATER_SPARKLE_ON)
                    if (_SparkleEnabled > 0.5)
                    {
                        // Randomized world-space sparkles with per-cell jitter over time.
                        // This avoids a clear directional drift when sparkles are large or viewed up close.
                        float sparkleAnim = _Time.y * max(_SparkleSpeed, 0.001);
                        float baseScale = max(_SparkleScale, 1.0) * 0.12;
                        // User-facing amount is easier than editing a raw threshold.
                        // 0 = rare/subtle, 1 = lots of sparkles. Threshold remains as an advanced cap.
                        float amountThreshold = lerp(0.997, 0.88, saturate(_SparkleAmount));
                        float threshold = min(saturate(_SparkleThreshold), amountThreshold);
                        float sparkleSize = max(_SparkleSize, 0.001);

                        float2 uv0 = input.positionWS.xz * baseScale;
                        float2 uv1 = Rotate2D(input.positionWS.xz * baseScale * 1.37 + 31.7, 1.13);
                        float2 uv2 = Rotate2D(input.positionWS.xz * baseScale * 0.73 - 18.2, -0.71);

                        float sparkle = 0.0;

                        float frame0 = floor(sparkleAnim * 0.73);
                        float frame1 = floor(sparkleAnim * 1.01 + 3.7);
                        float frame2 = floor(sparkleAnim * 1.29 + 8.3);

                        float2 cell0 = floor(uv0);
                        float2 frac0 = frac(uv0);
                        float phase0 = smoothstep(0.0, 1.0, frac(sparkleAnim * 0.73 + Hash21(cell0 + 19.2)));
                        float2 point0a = Hash22(cell0 + frame0 * 0.37 + 2.1);
                        float2 point0b = Hash22(cell0 + (frame0 + 1.0) * 0.37 + 2.1);
                        float2 point0 = lerp(point0a, point0b, phase0);
                        float gate0a = smoothstep(threshold, 1.0, Hash21(cell0 + frame0 + 7.1));
                        float gate0b = smoothstep(threshold, 1.0, Hash21(cell0 + frame0 + 8.9));
                        float gate0 = lerp(gate0a, gate0b, phase0);
                        sparkle += smoothstep(sparkleSize, 0.0, length(frac0 - point0)) * gate0;

                        float2 cell1 = floor(uv1);
                        float2 frac1 = frac(uv1);
                        float phase1 = smoothstep(0.0, 1.0, frac(sparkleAnim * 1.01 + Hash21(cell1 + 43.7)));
                        float2 point1a = Hash22(cell1 + frame1 * 0.53 + 11.4);
                        float2 point1b = Hash22(cell1 + (frame1 + 1.0) * 0.53 + 11.4);
                        float2 point1 = lerp(point1a, point1b, phase1);
                        float gate1a = smoothstep(threshold, 1.0, Hash21(cell1 + frame1 * 1.31 + 23.8));
                        float gate1b = smoothstep(threshold, 1.0, Hash21(cell1 + (frame1 + 1.0) * 1.31 + 24.9));
                        float gate1 = lerp(gate1a, gate1b, phase1);
                        sparkle += smoothstep(sparkleSize * 0.82, 0.0, length(frac1 - point1)) * gate1 * 0.75;

                        float2 cell2 = floor(uv2);
                        float2 frac2 = frac(uv2);
                        float phase2 = smoothstep(0.0, 1.0, frac(sparkleAnim * 1.29 + Hash21(cell2 + 71.4)));
                        float2 point2a = Hash22(cell2 + frame2 * 0.79 + 5.6);
                        float2 point2b = Hash22(cell2 + (frame2 + 1.0) * 0.79 + 5.6);
                        float2 point2 = lerp(point2a, point2b, phase2);
                        float gate2a = smoothstep(threshold, 1.0, Hash21(cell2 + frame2 * 1.73 + 41.2));
                        float gate2b = smoothstep(threshold, 1.0, Hash21(cell2 + (frame2 + 1.0) * 1.73 + 44.1));
                        float gate2 = lerp(gate2a, gate2b, phase2);
                        sparkle += smoothstep(sparkleSize * 0.68, 0.0, length(frac2 - point2)) * gate2 * 0.5;

                        sparkle = saturate(sparkle);
                        float viewBoost = pow(1.0 - saturate(dot(viewDirWS, normalWS)), 2.0) * _SparkleViewStrength;
                        color += _SparkleColor.rgb * sparkle * _SparkleIntensity * (0.55 + viewBoost);
                    }
                #endif

                float alpha = saturate(_BaseColor.a * _Opacity);
                return half4(color, alpha);
            }

            ENDHLSL
        }
    }

    FallBack Off
    CustomEditor "PearWaterShaderGUI"
}
