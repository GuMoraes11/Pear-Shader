using UnityEditor;
using UnityEngine;

namespace Pear.Editor
{
    public class PearToonShaderGUI : ShaderGUI
    {
        #region Foldout State and Styling

        private static bool presetFoldout = true;
        private static bool baseFoldout = true;
        private static bool toonFoldout = true;
        private static bool stylizedEffectsFoldout = false;
        private static bool outlineFoldout = false;
        private static bool advancedFoldout = false;
        private static bool uiSettingsFoldout = false;

        private const string AccentColorPrefsKey = "Pear_ToonShaderGUI_AccentColor";

        private MaterialEditor materialEditor;
        private MaterialProperty[] properties;

        private static Color accentColor = new Color(0.62f, 0.82f, 0.28f, 1f);

        private static readonly Color HeaderDarkColor = new Color(0.12f, 0.12f, 0.12f, 1f);
        private static readonly Color SectionHeaderColor = new Color(0.24f, 0.24f, 0.24f, 1f);
        private static readonly Color DisabledColor = new Color(0.45f, 0.45f, 0.45f, 1f);

        #endregion

        #region Unity Entry Point

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            this.materialEditor = materialEditor;
            this.properties = properties;

            LoadAccentColor();

            Material material = materialEditor.target as Material;

            DrawHeader(material);

            if (uiSettingsFoldout)
            {
                DrawUISettings();
            }

            DrawPresetSection(material);
            DrawBaseSection(material);
            DrawToonLightingSection(material);
            DrawStylizedEffectsSection(material);
            DrawOutlineSection(material);
            DrawAdvancedSection(material);
        }

        #endregion

        #region Header and Settings UI

        private void DrawHeader(Material material)
        {
            EditorGUILayout.Space(6);

            Rect rect = EditorGUILayout.GetControlRect(false, 40);
            EditorGUI.DrawRect(rect, HeaderDarkColor);

            Rect accentRect = new Rect(rect.x, rect.y, 5, rect.height);
            EditorGUI.DrawRect(accentRect, accentColor);

            Rect titleRect = new Rect(rect.x + 12, rect.y + 5, rect.width - 52, 18);
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                normal = { textColor = GetAccentTextColor() }
            };

            EditorGUI.LabelField(titleRect, "Pear Toon Shader", titleStyle);

            Rect subtitleRect = new Rect(rect.x + 12, rect.y + 22, rect.width - 52, 14);
            GUIStyle subtitleStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = new Color(0.7f, 0.7f, 0.7f, 1f) }
            };

            string shaderName = material != null && material.shader != null
                ? material.shader.name
                : "No shader selected";

            EditorGUI.LabelField(subtitleRect, shaderName, subtitleStyle);

            Rect settingsRect = new Rect(rect.xMax - 30, rect.y + 7, 22, 22);

            GUIStyle settingsStyle = new GUIStyle(EditorStyles.miniButton)
            {
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0)
            };

            if (GUI.Button(settingsRect, GetSettingsIconContent(), settingsStyle))
            {
                uiSettingsFoldout = !uiSettingsFoldout;
            }

            EditorGUILayout.Space(8);
        }

        private void DrawUISettings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("UI Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            Color newAccent = EditorGUILayout.ColorField(
                new GUIContent("Main UI Color", "Controls the accent color used by the Pear material inspector."),
                accentColor
            );

            if (EditorGUI.EndChangeCheck())
            {
                accentColor = newAccent;
                SaveAccentColor();
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(6);
        }

        #endregion

        #region Presets

        private void DrawPresetSection(Material material)
        {
            DrawSection(
                "Presets",
                ref presetFoldout,
                null,
                () =>
                {
                    if (material == null)
                    {
                        return;
                    }

                    EditorGUILayout.LabelField("Quick Looks", EditorStyles.boldLabel);
                    EditorGUILayout.Space(2);

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Soft Anime", GUILayout.Height(26)))
                    {
                        PearPresetApplier.ApplySoftAnime(material);
                    }

                    if (GUILayout.Button("Cel Shadow", GUILayout.Height(26)))
                    {
                        PearPresetApplier.ApplyCelShadow(material);
                    }

                    if (GUILayout.Button("Glossy Toy", GUILayout.Height(26)))
                    {
                        PearPresetApplier.ApplyGlossyToy(material);
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(4);
                    EditorGUILayout.HelpBox("Presets overwrite Pear shader values on the selected material. Duplicate materials before applying if you want to compare looks side by side.", MessageType.None);
                }
            );
        }

        #endregion

        #region Surface Section

        private void DrawBaseSection(Material material)
        {
            DrawSection(
                "Color & Surface",
                ref baseFoldout,
                null,
                () =>
                {
                    EditorGUILayout.LabelField("Base Color", EditorStyles.boldLabel);
                    DrawTextureWithColor(
                        "_BaseMap",
                        "_BaseColor",
                        new GUIContent("Color", "Main texture multiplied by the base color tint.")
                    );

                    DrawSeparator();

                    DrawInlineFeatureButton(
                        material,
                        "_HueShiftEnabled",
                        "_PEAR_HUE_SHIFT_ON",
                        "Texture Hue Shift",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_HueShiftMask", new GUIContent("Hue Shift Mask", "Grayscale mask controlling where hue shifting appears. White means full effect, black means no effect."));
                            DrawProperty("_HueShift", new GUIContent("Hue Shift", "Rotates the base texture hue in degrees before the base color tint."));
                            DrawProperty("_HueSaturation", new GUIContent("Saturation", "Multiplies the base texture saturation inside the hue-shifted area."));
                            DrawProperty("_HueBrightness", new GUIContent("Brightness", "Multiplies the base texture brightness inside the hue-shifted area."));
                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawSeparator();

                    EditorGUILayout.LabelField("Normals", EditorStyles.boldLabel);
                    DrawProperty("_NormalMap", new GUIContent("Normal Map", "Optional tangent-space normal map for surface detail."));
                    DrawProperty("_NormalStrength", new GUIContent("Normal Strength", "Controls how strongly the normal map affects lighting."));
                }
            );
        }

        #endregion

        #region Toon Lighting Section

        private void DrawToonLightingSection(Material material)
        {
            DrawSection(
                "Toon Lighting",
                ref toonFoldout,
                null,
                () =>
                {
                    bool midtoneEnabled = IsPropertyEnabled("_MidtoneEnabled");

                    EditorGUILayout.LabelField("Shadow Response", EditorStyles.boldLabel);
                    DrawProperty("_ShadowColor", new GUIContent("Shadow Color", "Tint used for the darkest stylized shadow band."));

                    DrawSeparator();

                    DrawInlineFeatureButton(
                        material,
                        "_MidtoneEnabled",
                        "_PEAR_MIDTONE_ON",
                        "Midtone Band",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_MidtoneColor", new GUIContent("Midtone Color", "Tint used for the middle lighting band."));
                            DrawThreeBandControls();
                            EditorGUI.indentLevel--;
                        }
                    );

                    if (!midtoneEnabled)
                    {
                        DrawTwoBandControls();
                    }

                    DrawInlineFeatureButton(
                        material,
                        "_RampEnabled",
                        "_PEAR_RAMP_ON",
                        "Ramp Texture",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_RampTex", new GUIContent("Ramp", "Horizontal ramp texture. Left side is shadow, right side is light."));
                            DrawProperty("_RampStrength", new GUIContent("Ramp Strength", "Blends between manual lighting and the ramp texture."));
                            EditorGUILayout.HelpBox("Ramp Texture visually overrides the manual band controls. Use it for custom multi-step shading.", MessageType.None);
                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawSeparator();

                    EditorGUILayout.LabelField("Light Mixing", EditorStyles.boldLabel);
                    DrawProperty("_LightIntensity", new GUIContent("Light Intensity", "Multiplier for the direct toon light contribution."));
                    DrawProperty("_AmbientColor", new GUIContent("Ambient Color", "Simple fill color added to the final result."));
                }
            );
        }

        private void DrawTwoBandControls()
        {
            MaterialProperty lightThreshold = FindProperty("_ToonThreshold", properties, false);
            MaterialProperty lightSoftness = FindProperty("_ToonSoftness", properties, false);

            if (lightThreshold == null || lightSoftness == null)
            {
                DrawMissingPropertyWarning("Toon band controls");
                return;
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Band Controls", EditorStyles.boldLabel);

            lightThreshold.floatValue = EditorGUILayout.Slider(
                new GUIContent("Shadow Size", "Controls how much of the model is in shadow. Higher values create a larger shadow area."),
                lightThreshold.floatValue,
                0f,
                1f
            );

            lightSoftness.floatValue = EditorGUILayout.Slider(
                new GUIContent("Edge Softness", "Controls how soft the transition is between shadow and light. Low values create hard cel shading."),
                lightSoftness.floatValue,
                0.001f,
                0.25f
            );
        }

        private void DrawThreeBandControls()
        {
            MaterialProperty midtoneThreshold = FindProperty("_MidtoneThreshold", properties, false);
            MaterialProperty lightThreshold = FindProperty("_ToonThreshold", properties, false);
            MaterialProperty midtoneSoftness = FindProperty("_MidtoneSoftness", properties, false);
            MaterialProperty lightSoftness = FindProperty("_ToonSoftness", properties, false);

            if (midtoneThreshold == null || lightThreshold == null || midtoneSoftness == null || lightSoftness == null)
            {
                DrawMissingPropertyWarning("Midtone band controls");
                return;
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Band Positions", EditorStyles.boldLabel);

            float shadowToMidtone = midtoneThreshold.floatValue;
            float midtoneToLight = lightThreshold.floatValue;

            EditorGUILayout.MinMaxSlider(
                new GUIContent("Shadow / Midtone / Light", "Left handle controls where shadow becomes midtone. Right handle controls where midtone becomes full light."),
                ref shadowToMidtone,
                ref midtoneToLight,
                0f,
                1f
            );

            const float minimumBandGap = 0.03f;

            shadowToMidtone = Mathf.Clamp(shadowToMidtone, 0f, 1f - minimumBandGap);
            midtoneToLight = Mathf.Clamp(midtoneToLight, shadowToMidtone + minimumBandGap, 1f);

            midtoneThreshold.floatValue = shadowToMidtone;
            lightThreshold.floatValue = midtoneToLight;

            EditorGUI.indentLevel++;

            midtoneThreshold.floatValue = EditorGUILayout.Slider(
                new GUIContent("Shadow To Midtone", "Where the darkest shadow band transitions into the midtone band."),
                midtoneThreshold.floatValue,
                0f,
                lightThreshold.floatValue - minimumBandGap
            );

            lightThreshold.floatValue = EditorGUILayout.Slider(
                new GUIContent("Midtone To Light", "Where the midtone band transitions into the fully lit band."),
                lightThreshold.floatValue,
                midtoneThreshold.floatValue + minimumBandGap,
                1f
            );

            EditorGUI.indentLevel--;

            float sharedSoftness = Mathf.Max(midtoneSoftness.floatValue, lightSoftness.floatValue);

            sharedSoftness = EditorGUILayout.Slider(
                new GUIContent("Edge Softness", "Controls how soft both band transitions are. Low values create hard cel-shaded bands."),
                sharedSoftness,
                0.001f,
                0.25f
            );

            midtoneSoftness.floatValue = sharedSoftness;
            lightSoftness.floatValue = sharedSoftness;
        }

        #endregion

        #region Stylized Effects Section

        private void DrawStylizedEffectsSection(Material material)
        {
            DrawSection(
                "Stylized Effects",
                ref stylizedEffectsFoldout,
                null,
                () =>
                {
                    DrawInlineFeatureButton(
                        material,
                        "_RimEnabled",
                        "_PEAR_RIM_ON",
                        "Rim Light",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_RimColor", new GUIContent("Rim Color", "Color added to the edges of the object."));
                            DrawProperty("_RimPower", new GUIContent("Rim Power", "Controls rim width. Lower values create a wider rim."));
                            DrawProperty("_RimIntensity", new GUIContent("Rim Intensity", "Controls how bright the rim light is."));
                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawInlineFeatureButton(
                        material,
                        "_EmissionEnabled",
                        "_PEAR_EMISSION_ON",
                        "Emission",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_EmissionColor", new GUIContent("Emission Color", "Color added on top of the shaded material."));
                            DrawProperty("_EmissionMask", new GUIContent("Emission Mask", "Grayscale texture controlling where emission appears."));
                            DrawProperty("_EmissionIntensity", new GUIContent("Emission Intensity", "Brightness multiplier for the emission effect."));
                            DrawProperty("_EmissionPulseSpeed", new GUIContent("Pulse Speed", "How fast the emission pulses. Set to 0 to disable movement."));
                            DrawProperty("_EmissionPulseAmount", new GUIContent("Pulse Amount", "How strongly the emission fades in and out."));
                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawInlineFeatureButton(
                        material,
                        "_MatcapEnabled",
                        "_PEAR_MATCAP_ON",
                        "Matcap Reflection",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_MatcapTex", new GUIContent("Matcap Texture", "Spherical reflection texture sampled using the view-space normal."));
                            DrawProperty("_MatcapColor", new GUIContent("Matcap Color", "Tint applied to the matcap texture."));
                            DrawProperty("_MatcapIntensity", new GUIContent("Matcap Intensity", "Brightness multiplier for the matcap layer."));
                            DrawProperty("_MatcapBlend", new GUIContent("Matcap Blend", "How strongly the matcap layer is blended into the final result."));
                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawInlineFeatureButton(
                        material,
                        "_ShimmerEnabled",
                        "_PEAR_SHIMMER_ON",
                        "Glitter Shimmer",
                        () =>
                        {
                            EditorGUI.indentLevel++;
                            DrawProperty("_ShimmerColor", new GUIContent("Shimmer Color", "Color of the tiny sparkle hits."));
                            DrawProperty("_ShimmerIntensity", new GUIContent("Shimmer Intensity", "Brightness of the shimmer sparkles."));
                            DrawProperty("_ShimmerScale", new GUIContent("Sparkle Density", "Higher values create smaller, denser sparkles."));
                            DrawProperty("_ShimmerSpeed", new GUIContent("Flicker Speed", "How quickly the sparkle pattern changes."));
                            DrawProperty("_ShimmerThreshold", new GUIContent("Sparkle Rarity", "Higher values create fewer sparkles."));
                            DrawProperty("_ShimmerViewStrength", new GUIContent("View Angle Boost", "Makes sparkles stronger near grazing angles."));
                            EditorGUI.indentLevel--;
                        }
                    );
                }
            );
        }

        #endregion

        #region Outline and Advanced Sections

        private void DrawOutlineSection(Material material)
        {
            DrawSection(
                "Outline",
                ref outlineFoldout,
                new FeatureToggle("_OutlineEnabled", "_PEAR_OUTLINE_ON"),
                () =>
                {
                    EditorGUI.indentLevel++;
                    DrawProperty("_OutlineColor", new GUIContent("Outline Color", "Color of the inverted hull outline."));
                    DrawProperty("_OutlineWidth", new GUIContent("Outline Width", "Thickness of the outline in world units."));
                    DrawProperty("_OutlineDepthOffset", new GUIContent("Depth Offset", "Small depth push/pull to reduce z-fighting. Leave at 0 unless needed."));
                    EditorGUI.indentLevel--;
                },
                material
            );
        }

        private void DrawAdvancedSection(Material material)
        {
            DrawSection(
                "Advanced",
                ref advancedFoldout,
                null,
                () =>
                {
                    if (material == null)
                    {
                        return;
                    }

                    EditorGUILayout.LabelField("Shader", material.shader.name);
                    EditorGUILayout.LabelField("Render Queue", material.renderQueue.ToString());

                    EditorGUILayout.Space(4);

                    if (GUILayout.Button("Reset Pear Keywords"))
                    {
                        SyncAllKeywords(material);
                    }
                }
            );
        }

        #endregion

        #region Section Drawing

        private void DrawSection(string title, ref bool foldout, FeatureToggle featureToggle, System.Action drawContent, Material material = null)
        {
            bool hasToggle = featureToggle != null;
            bool enabled = !hasToggle || IsPropertyEnabled(featureToggle.PropertyName);

            EditorGUILayout.Space(2);

            Rect headerRect = EditorGUILayout.GetControlRect(false, 25);
            EditorGUI.DrawRect(headerRect, SectionHeaderColor);

            Rect accentRect = new Rect(headerRect.x, headerRect.y, 4, headerRect.height);
            EditorGUI.DrawRect(accentRect, enabled ? accentColor : DisabledColor);

            float cursorX = headerRect.x + 8f;

            Rect toggleRect = Rect.zero;
            if (hasToggle)
            {
                toggleRect = new Rect(cursorX, headerRect.y + 4, 16, 16);
                DrawHeaderToggle(toggleRect, material, featureToggle);
                cursorX += 22f;
            }

            Rect arrowRect = new Rect(cursorX, headerRect.y + 4, 14, 16);

            GUIStyle arrowStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };

            if (GUI.Button(arrowRect, foldout ? "▾" : "▸", arrowStyle))
            {
                foldout = !foldout;
            }

            cursorX += 20f;

            float rightPadding = hasToggle ? 70f : 12f;
            Rect labelRect = new Rect(cursorX, headerRect.y + 3, headerRect.width - (cursorX - headerRect.x) - rightPadding, 18);

            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = Color.white }
            };

            EditorGUI.LabelField(labelRect, title, labelStyle);

            if (hasToggle)
            {
                Rect statusRect = new Rect(headerRect.xMax - 60, headerRect.y + 4, 50, 16);
                GUIStyle statusStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                {
                    alignment = TextAnchor.MiddleRight,
                    normal = { textColor = enabled ? accentColor : DisabledColor }
                };

                EditorGUI.LabelField(statusRect, enabled ? "ON" : "OFF", statusStyle);
            }

            if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
            {
                bool clickedToggle = hasToggle && toggleRect.Contains(Event.current.mousePosition);
                bool clickedArrow = arrowRect.Contains(Event.current.mousePosition);

                if (!clickedToggle && !clickedArrow)
                {
                    foldout = !foldout;
                    Event.current.Use();
                }
            }

            if (!foldout)
            {
                return;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(4);

            if (hasToggle && !enabled)
            {
                EditorGUILayout.HelpBox("This feature is currently disabled. Enable it from the checkbox in the section header to activate the effect.", MessageType.None);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.indentLevel++;
                drawContent?.Invoke();
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.indentLevel++;
                drawContent?.Invoke();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.EndVertical();
        }

        private void DrawHeaderToggle(Rect rect, Material material, FeatureToggle featureToggle)
        {
            MaterialProperty property = FindProperty(featureToggle.PropertyName, properties, false);

            if (property == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            bool enabled = property.floatValue > 0.5f;
            enabled = EditorGUI.Toggle(rect, enabled);

            if (EditorGUI.EndChangeCheck())
            {
                property.floatValue = enabled ? 1f : 0f;
                SetKeyword(material, featureToggle.Keyword, enabled);
            }
        }

        private void DrawInlineFeatureButton(Material material, string propertyName, string keyword, string label, System.Action drawContent)
        {
            MaterialProperty property = FindProperty(propertyName, properties, false);

            if (property == null)
            {
                DrawMissingPropertyWarning(propertyName);
                return;
            }

            bool enabled = property.floatValue > 0.5f;

            EditorGUILayout.Space(2);

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                fixedHeight = 26,
                padding = new RectOffset(12, 8, 0, 0)
            };

            Color previousBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = enabled
                ? Color.Lerp(Color.white, accentColor, 0.45f)
                : new Color(0.75f, 0.75f, 0.75f, 1f);

            string buttonText = enabled ? $"{label}        ON" : $"{label}        OFF";

            if (GUILayout.Button(buttonText, buttonStyle, GUILayout.Height(26)))
            {
                enabled = !enabled;
                property.floatValue = enabled ? 1f : 0f;
                SetKeyword(material, keyword, enabled);
                GUI.changed = true;
            }

            GUI.backgroundColor = previousBackgroundColor;

            if (!enabled)
            {
                EditorGUILayout.Space(2);
                return;
            }

            EditorGUILayout.Space(4);
            EditorGUI.indentLevel++;
            drawContent?.Invoke();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(4);
        }

        #endregion

        #region Drawing Helpers

        private bool IsPropertyEnabled(string propertyName)
        {
            MaterialProperty property = FindProperty(propertyName, properties, false);
            return property != null && property.floatValue > 0.5f;
        }

        private void DrawTextureWithColor(string textureName, string colorName, GUIContent label)
        {
            MaterialProperty textureProperty = FindProperty(textureName, properties, false);
            MaterialProperty colorProperty = FindProperty(colorName, properties, false);

            if (textureProperty != null && colorProperty != null)
            {
                materialEditor.TexturePropertySingleLine(label, textureProperty, colorProperty);
                materialEditor.TextureScaleOffsetProperty(textureProperty);
            }
            else
            {
                DrawMissingPropertyWarning(textureName + " / " + colorName);
            }
        }

        private void DrawProperty(string propertyName, GUIContent label)
        {
            MaterialProperty property = FindProperty(propertyName, properties, false);

            if (property != null)
            {
                materialEditor.ShaderProperty(property, label);
            }
            else
            {
                DrawMissingPropertyWarning(propertyName);
            }
        }

        private void DrawSeparator()
        {
            EditorGUILayout.Space(5);
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, new Color(0.32f, 0.32f, 0.32f, 1f));
            EditorGUILayout.Space(5);
        }

        private void SetKeyword(Material material, string keyword, bool enabled)
        {
            if (material == null)
            {
                return;
            }

            if (enabled)
            {
                material.EnableKeyword(keyword);
            }
            else
            {
                material.DisableKeyword(keyword);
            }

            EditorUtility.SetDirty(material);
        }

        private void SyncAllKeywords(Material material)
        {
            SetKeyword(material, "_PEAR_HUE_SHIFT_ON", IsPropertyEnabled("_HueShiftEnabled"));
            SetKeyword(material, "_PEAR_MIDTONE_ON", IsPropertyEnabled("_MidtoneEnabled"));
            SetKeyword(material, "_PEAR_RAMP_ON", IsPropertyEnabled("_RampEnabled"));
            SetKeyword(material, "_PEAR_RIM_ON", IsPropertyEnabled("_RimEnabled"));
            SetKeyword(material, "_PEAR_EMISSION_ON", IsPropertyEnabled("_EmissionEnabled"));
            SetKeyword(material, "_PEAR_MATCAP_ON", IsPropertyEnabled("_MatcapEnabled"));
            SetKeyword(material, "_PEAR_SHIMMER_ON", IsPropertyEnabled("_ShimmerEnabled"));
            SetKeyword(material, "_PEAR_OUTLINE_ON", IsPropertyEnabled("_OutlineEnabled"));
        }

        private void DrawMissingPropertyWarning(string propertyName)
        {
            EditorGUILayout.HelpBox("Missing shader property: " + propertyName, MessageType.Warning);
        }

        #endregion

        #region Editor Preferences

        private static void LoadAccentColor()
        {
            if (!EditorPrefs.HasKey(AccentColorPrefsKey))
            {
                return;
            }

            string colorHtml = EditorPrefs.GetString(AccentColorPrefsKey);

            if (ColorUtility.TryParseHtmlString(colorHtml, out Color loadedColor))
            {
                accentColor = loadedColor;
            }
        }

        private static void SaveAccentColor()
        {
            string colorHtml = "#" + ColorUtility.ToHtmlStringRGBA(accentColor);
            EditorPrefs.SetString(AccentColorPrefsKey, colorHtml);
        }

        private static Color GetAccentTextColor()
        {
            return Color.Lerp(Color.white, accentColor, 0.75f);
        }

        private static GUIContent GetSettingsIconContent()
        {
            string[] iconNames =
            {
                "SettingsIcon",
                "d_SettingsIcon",
                "_Popup",
                "d__Popup"
            };

            foreach (string iconName in iconNames)
            {
                GUIContent content = EditorGUIUtility.IconContent(iconName);

                if (content != null && content.image != null)
                {
                    return new GUIContent(content.image, "UI Settings");
                }
            }

            return new GUIContent("⋯", "UI Settings");
        }

        #endregion

        #region Data Types

        private class FeatureToggle
        {
            public readonly string PropertyName;
            public readonly string Keyword;

            public FeatureToggle(string propertyName, string keyword)
            {
                PropertyName = propertyName;
                Keyword = keyword;
            }
        }

        #endregion
    }
}
