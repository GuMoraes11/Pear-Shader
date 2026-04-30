using UnityEditor;
using UnityEngine;

namespace Pear.Editor
{
    public class PearToonShaderGUI : ShaderGUI
    {
        private static bool baseFoldout = true;
        private static bool toonFoldout = true;
        private static bool rimFoldout = false;
        private static bool emissionFoldout = false;
        private static bool matcapFoldout = false;
        private static bool advancedFoldout = false;
        private static bool uiSettingsFoldout = false;

        private const string AccentColorPrefsKey = "Pear_ToonShaderGUI_AccentColor";

        private MaterialEditor materialEditor;
        private MaterialProperty[] properties;

        private static Color accentColor = new Color(0.62f, 0.82f, 0.28f, 1f);

        private static readonly Color HeaderDarkColor = new Color(0.12f, 0.12f, 0.12f, 1f);
        private static readonly Color SectionHeaderColor = new Color(0.24f, 0.24f, 0.24f, 1f);
        private static readonly Color DisabledColor = new Color(0.45f, 0.45f, 0.45f, 1f);

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

            DrawBaseSection();
            DrawToonLightingSection(material);
            DrawRimLightSection(material);
            DrawEmissionSection(material);
            DrawMatcapSection(material);
            DrawAdvancedSection(material);
        }

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
                new GUIContent(
                    "Main UI Color",
                    "Controls the accent color used by the Pear material inspector."
                ),
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

        private void DrawBaseSection()
        {
            DrawSection(
                "Color & Normals",
                ref baseFoldout,
                null,
                () =>
                {
                    DrawTextureWithColor(
                        "_BaseMap",
                        "_BaseColor",
                        new GUIContent(
                            "Color",
                            "Main texture multiplied by the base color tint."
                        )
                    );

                    DrawProperty(
                        "_NormalMap",
                        new GUIContent(
                            "Normal Map",
                            "Optional tangent-space normal map for surface detail."
                        )
                    );

                    DrawProperty(
                        "_NormalStrength",
                        new GUIContent(
                            "Normal Strength",
                            "Controls how strongly the normal map affects lighting."
                        )
                    );
                }
            );
        }

        private void DrawToonLightingSection(Material material)
        {
            DrawSection(
                "Toon Lighting",
                ref toonFoldout,
                null,
                () =>
                {
                    DrawProperty(
                        "_ToonThreshold",
                        new GUIContent(
                            "Light Threshold",
                            "Controls where the material transitions into the fully lit band."
                        )
                    );

                    DrawProperty(
                        "_ToonSoftness",
                        new GUIContent(
                            "Light Softness",
                            "Controls how sharp or soft the transition into the lit band is."
                        )
                    );

                    DrawProperty(
                        "_ShadowColor",
                        new GUIContent(
                            "Shadow Color",
                            "Tint used for the darkest stylized shadow band."
                        )
                    );

                    DrawSeparator();

                    DrawInlineFeatureButton(
                        material,
                        "_MidtoneEnabled",
                        "_PEAR_MIDTONE_ON",
                        "Midtone Band",
                        () =>
                        {
                            EditorGUI.indentLevel++;

                            DrawProperty(
                                "_MidtoneColor",
                                new GUIContent(
                                    "Midtone Color",
                                    "Tint used for the middle lighting band."
                                )
                            );

                            DrawProperty(
                                "_MidtoneThreshold",
                                new GUIContent(
                                    "Midtone Threshold",
                                    "Controls where the material transitions from shadow to midtone."
                                )
                            );

                            DrawProperty(
                                "_MidtoneSoftness",
                                new GUIContent(
                                    "Midtone Softness",
                                    "Controls how sharp or soft the transition into the midtone band is."
                                )
                            );

                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawInlineFeatureButton(
                        material,
                        "_RampEnabled",
                        "_PEAR_RAMP_ON",
                        "Ramp Texture",
                        () =>
                        {
                            EditorGUI.indentLevel++;

                            DrawProperty(
                                "_RampTex",
                                new GUIContent(
                                    "Ramp",
                                    "Horizontal ramp texture. Left side is shadow, right side is light."
                                )
                            );

                            DrawProperty(
                                "_RampStrength",
                                new GUIContent(
                                    "Ramp Strength",
                                    "Blends between manual lighting and the ramp texture."
                                )
                            );

                            EditorGUI.indentLevel--;
                        }
                    );

                    DrawSeparator();

                    DrawProperty(
                        "_LightIntensity",
                        new GUIContent(
                            "Light Intensity",
                            "Multiplier for the direct toon light contribution."
                        )
                    );

                    DrawProperty(
                        "_AmbientColor",
                        new GUIContent(
                            "Ambient Color",
                            "Simple fill color added to the final result."
                        )
                    );
                }
            );
        }

        private void DrawRimLightSection(Material material)
        {
            DrawSection(
                "Rim Light",
                ref rimFoldout,
                new FeatureToggle("_RimEnabled", "_PEAR_RIM_ON"),
                () =>
                {
                    EditorGUI.indentLevel++;

                    DrawProperty(
                        "_RimColor",
                        new GUIContent(
                            "Rim Color",
                            "Color added to the edges of the object."
                        )
                    );

                    DrawProperty(
                        "_RimPower",
                        new GUIContent(
                            "Rim Power",
                            "Controls rim width. Lower values create a wider rim."
                        )
                    );

                    DrawProperty(
                        "_RimIntensity",
                        new GUIContent(
                            "Rim Intensity",
                            "Controls how bright the rim light is."
                        )
                    );

                    EditorGUI.indentLevel--;
                },
                material
            );
        }

        private void DrawEmissionSection(Material material)
        {
            DrawSection(
                "Emission",
                ref emissionFoldout,
                new FeatureToggle("_EmissionEnabled", "_PEAR_EMISSION_ON"),
                () =>
                {
                    EditorGUI.indentLevel++;

                    DrawProperty(
                        "_EmissionColor",
                        new GUIContent(
                            "Emission Color",
                            "Color added on top of the shaded material."
                        )
                    );

                    DrawProperty(
                        "_EmissionMask",
                        new GUIContent(
                            "Emission Mask",
                            "Grayscale texture controlling where emission appears."
                        )
                    );

                    DrawProperty(
                        "_EmissionIntensity",
                        new GUIContent(
                            "Emission Intensity",
                            "Brightness multiplier for the emission effect."
                        )
                    );

                    DrawProperty(
                        "_EmissionPulseSpeed",
                        new GUIContent(
                            "Pulse Speed",
                            "How fast the emission pulses. Set to 0 to disable movement."
                        )
                    );

                    DrawProperty(
                        "_EmissionPulseAmount",
                        new GUIContent(
                            "Pulse Amount",
                            "How strongly the emission fades in and out."
                        )
                    );

                    EditorGUI.indentLevel--;
                },
                material
            );
        }

        private void DrawMatcapSection(Material material)
        {
            DrawSection(
                "Matcap Reflection",
                ref matcapFoldout,
                new FeatureToggle("_MatcapEnabled", "_PEAR_MATCAP_ON"),
                () =>
                {
                    EditorGUI.indentLevel++;

                    DrawProperty(
                        "_MatcapTex",
                        new GUIContent(
                            "Matcap Texture",
                            "Spherical reflection texture sampled using the view-space normal."
                        )
                    );

                    DrawProperty(
                        "_MatcapColor",
                        new GUIContent(
                            "Matcap Color",
                            "Tint applied to the matcap texture."
                        )
                    );

                    DrawProperty(
                        "_MatcapIntensity",
                        new GUIContent(
                            "Matcap Intensity",
                            "Brightness multiplier for the matcap layer."
                        )
                    );

                    DrawProperty(
                        "_MatcapBlend",
                        new GUIContent(
                            "Matcap Blend",
                            "How strongly the matcap layer is blended into the final result."
                        )
                    );

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

        private void DrawSection(
            string title,
            ref bool foldout,
            FeatureToggle featureToggle,
            System.Action drawContent,
            Material material = null
        )
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
            Rect labelRect = new Rect(
                cursorX,
                headerRect.y + 3,
                headerRect.width - (cursorX - headerRect.x) - rightPadding,
                18
            );

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

            if (hasToggle && !enabled)
            {
                return;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(4);
            EditorGUI.indentLevel++;
            drawContent?.Invoke();
            EditorGUI.indentLevel--;
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

        private void DrawInlineFeatureButton(
            Material material,
            string propertyName,
            string keyword,
            string label,
            System.Action drawContent
        )
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

            string buttonText = enabled
                ? $"{label}        ON"
                : $"{label}        OFF";

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
            SetKeyword(material, "_PEAR_MIDTONE_ON", IsPropertyEnabled("_MidtoneEnabled"));
            SetKeyword(material, "_PEAR_RAMP_ON", IsPropertyEnabled("_RampEnabled"));
            SetKeyword(material, "_PEAR_RIM_ON", IsPropertyEnabled("_RimEnabled"));
            SetKeyword(material, "_PEAR_EMISSION_ON", IsPropertyEnabled("_EmissionEnabled"));
            SetKeyword(material, "_PEAR_MATCAP_ON", IsPropertyEnabled("_MatcapEnabled"));
        }

        private void DrawMissingPropertyWarning(string propertyName)
        {
            EditorGUILayout.HelpBox(
                "Missing shader property: " + propertyName,
                MessageType.Warning
            );
        }

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
    }
}