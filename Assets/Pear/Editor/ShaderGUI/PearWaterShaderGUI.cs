using UnityEditor;
using UnityEngine;

public class PearWaterShaderGUI : ShaderGUI
{
    #region Foldout State and Styling

    private static bool presetFoldout = true;
    private static bool surfaceFoldout = true;
    private static bool lightingFoldout = true;
    private static bool stylizedEffectsFoldout = true;
    private static bool motionFoldout = true;
    private static bool advancedFoldout = false;
    private static bool uiSettingsFoldout = false;

    private const string AccentColorPrefsKey = "Pear_WaterShaderGUI_AccentColor";

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

        EditorGUI.BeginChangeCheck();

        DrawPresetSection(material);
        DrawSurfaceSection(material);
        DrawLightingSection(material);
        DrawStylizedEffectsSection(material);
        DrawMotionSection(material);
        DrawAdvancedSection(material);

        if (EditorGUI.EndChangeCheck())
        {
            foreach (Object target in materialEditor.targets)
            {
                if (target is Material mat)
                {
                    PearWaterPresetApplier.SyncAllKeywords(mat);
                    EditorUtility.SetDirty(mat);
                }
            }
        }
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

        EditorGUI.LabelField(titleRect, "Pear Water Shader", titleStyle);

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
            new GUIContent("Main UI Color", "Controls the accent color used by the Pear Water material inspector."),
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

                if (GUILayout.Button("Cup", GUILayout.Height(26)))
                {
                    ApplyToTargets(PearWaterPresetApplier.ApplyCup);
                }

                if (GUILayout.Button("Pond", GUILayout.Height(26)))
                {
                    ApplyToTargets(PearWaterPresetApplier.ApplyPond);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Pool", GUILayout.Height(26)))
                {
                    ApplyToTargets(PearWaterPresetApplier.ApplyPool);
                }

                if (GUILayout.Button("Magic", GUILayout.Height(26)))
                {
                    ApplyToTargets(PearWaterPresetApplier.ApplyMagic);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(4);
                EditorGUILayout.HelpBox("Presets overwrite Pear Water values on the selected material. Duplicate materials before applying if you want to compare looks side by side.", MessageType.None);
            }
        );
    }

    #endregion

    #region Surface Section

    private void DrawSurfaceSection(Material material)
    {
        DrawSection(
            "Color & Surface",
            ref surfaceFoldout,
            null,
            () =>
            {
                EditorGUILayout.LabelField("Water Surface", EditorStyles.boldLabel);
                DrawProperty("_BaseColor", new GUIContent("Water Color", "Main transparent water color."));
                DrawProperty("_Opacity", new GUIContent("Opacity", "Overall water transparency. Lower values are clearer."));
                DrawProperty("_SurfaceTintStrength", new GUIContent("Tint Strength", "Controls how strongly ambient water tint affects the final result."));

                DrawSeparator();

                DrawInlineFeatureButton(
                    material,
                    "_TopSurfaceOnly",
                    null,
                    "Top Surface Only",
                    () =>
                    {
                        EditorGUILayout.HelpBox("Hides steep side faces. Keep this ON for water planes, pools, and thick demo slabs.", MessageType.None);
                    }
                );
            }
        );
    }

    #endregion

    #region Lighting Section

    private void DrawLightingSection(Material material)
    {
        DrawSection(
            "Water Lighting",
            ref lightingFoldout,
            null,
            () =>
            {
                EditorGUILayout.LabelField("Toon Response", EditorStyles.boldLabel);
                DrawProperty("_ShadowColor", new GUIContent("Shadow Color", "Tint used for darker water-facing areas."));
                DrawProperty("_ToonThreshold", new GUIContent("Shadow Size", "Controls how much of the water is pushed into the shadow tint."));
                DrawProperty("_ToonSoftness", new GUIContent("Edge Softness", "Controls how soft the transition is between shadow and light."));

                DrawSeparator();

                EditorGUILayout.LabelField("Light Mixing", EditorStyles.boldLabel);
                DrawProperty("_LightIntensity", new GUIContent("Light Intensity", "Multiplier for the main light contribution."));
                DrawProperty("_AmbientColor", new GUIContent("Ambient Color", "Simple fill color added to the final water result."));
            }
        );
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
                    "_PEAR_WATER_RIM_ON",
                    "Rim Highlight",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_RimColor", new GUIContent("Rim Color", "Color added near grazing angles."));
                        DrawProperty("_RimPower", new GUIContent("Rim Power", "Controls rim width. Lower values create a wider rim."));
                        DrawProperty("_RimIntensity", new GUIContent("Rim Intensity", "Controls how bright the rim highlight is."));
                        EditorGUI.indentLevel--;
                    }
                );

                DrawInlineFeatureButton(
                    material,
                    "_SubsurfaceEnabled",
                    "_PEAR_WATER_SUBSURFACE_ON",
                    "Subsurface Glow",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_SubsurfaceColor", new GUIContent("Glow Color", "Color added to make water feel thicker and softly lit from within."));
                        DrawProperty("_SubsurfaceStrength", new GUIContent("Glow Strength", "Overall strength of the fake subsurface/transmission glow."));
                        DrawProperty("_SubsurfaceViewPower", new GUIContent("View Power", "Controls how much glow appears at grazing angles. Lower values make it wider."));
                        DrawProperty("_SubsurfaceLightStrength", new GUIContent("Light Influence", "How much the main light direction contributes to the glow."));
                        DrawProperty("_SubsurfaceDepthStrength", new GUIContent("Depth Influence", "How much scene depth below the water contributes to the glow. Requires Depth Texture."));
                        DrawProperty("_SubsurfaceDepthDistance", new GUIContent("Depth Distance", "How much depth is needed before the depth glow reaches full strength."));
                        EditorGUILayout.HelpBox("Subsurface Glow is a stylized fake. It helps water feel thicker, brighter, and less like a flat transparent plane.", MessageType.None);
                        EditorGUI.indentLevel--;
                    }
                );

                DrawInlineFeatureButton(
                    material,
                    "_FoamEnabled",
                    "_PEAR_WATER_FOAM_ON",
                    "Foam / Surface Noise",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_FoamColor", new GUIContent("Foam Color", "Color blended into the water where foam/noise appears."));
                        DrawProperty("_FoamAmount", new GUIContent("Foam Amount", "Controls how strongly foam affects the water color."));
                        DrawProperty("_FoamScale", new GUIContent("Foam Scale", "Controls foam/noise size. Higher values create smaller patches."));
                        DrawProperty("_FoamSpeed", new GUIContent("Foam Speed", "Scrolling direction and speed for the foam pattern."));
                        DrawProperty("_FoamThreshold", new GUIContent("Foam Rarity", "Higher values create fewer foam patches."));
                        DrawProperty("_FoamSoftness", new GUIContent("Foam Softness", "Softens the edge of foam patches."));
                        DrawProperty("_FoamNoise", new GUIContent("Optional Foam Texture", "Optional grayscale texture blended with procedural foam."));
                        DrawProperty("_FoamTextureBlend", new GUIContent("Texture Blend", "0 uses procedural foam only. 1 uses the assigned texture fully."));
                        EditorGUILayout.HelpBox("Procedural foam works without a texture. Higher Foam Rarity values create fewer patches.", MessageType.None);
                        EditorGUI.indentLevel--;
                    }
                );

                DrawInlineFeatureButton(
                    material,
                    "_ContactFoamEnabled",
                    "_PEAR_WATER_CONTACT_FOAM_ON",
                    "Contact Foam",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_ContactFoamColor", new GUIContent("Foam Color", "Color added where the water intersects scene geometry."));
                        DrawProperty("_ContactFoamStrength", new GUIContent("Strength", "How strongly the contact foam blends into the water."));
                        DrawProperty("_ContactFoamDistance", new GUIContent("Contact Distance", "How far from intersecting geometry the foam can appear. Increase this if the contact edge is too thin."));
                        DrawProperty("_ContactFoamSoftness", new GUIContent("Edge Softness", "Softens the fade of the contact foam edge."));
                        DrawProperty("_ContactFoamNoiseAmount", new GUIContent("Breakup Amount", "Adds organic breakup so the contact foam is not a perfect ring."));
                        DrawProperty("_ContactFoamNoiseScale", new GUIContent("Breakup Scale", "Size of the breakup/noise pattern. Higher values create smaller breakup detail."));
                        DrawProperty("_ContactFoamNoiseSpeed", new GUIContent("Breakup Speed", "Scrolling direction and speed for contact foam breakup."));
                        EditorGUILayout.HelpBox("Contact Foam uses URP scene depth. If it does not appear, enable Depth Texture in the URP Asset or on the Camera.", MessageType.Info);
                        EditorGUI.indentLevel--;
                    }
                );

                DrawInlineFeatureButton(
                    material,
                    "_FlowEnabled",
                    "_PEAR_WATER_FLOW_ON",
                    "Surface Flow",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_FlowColor", new GUIContent("Highlight Color", "Color of animated surface-flow ribbons."));
                        DrawProperty("_FlowIntensity", new GUIContent("Brightness", "How visible the animated flow highlights are."));
                        DrawProperty("_FlowScale", new GUIContent("Scale", "Size of the moving surface-flow pattern."));
                        DrawProperty("_FlowSpeed", new GUIContent("Speed", "Animation speed and direction for surface-flow highlights."));
                        DrawProperty("_FlowThickness", new GUIContent("Line Thickness", "Width of the surface-flow ribbons."));
                        DrawProperty("_FlowDistortion", new GUIContent("Distortion", "Breaks up the flow ribbons so they feel less perfect and less stripe-like."));
                        EditorGUILayout.HelpBox("Surface Flow adds animated highlight ribbons so flat water has visible motion even without texture maps.", MessageType.None);
                        EditorGUI.indentLevel--;
                    }
                );

                DrawInlineFeatureButton(
                    material,
                    "_SparkleEnabled",
                    "_PEAR_WATER_SPARKLE_ON",
                    "Surface Sparkle",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_SparkleColor", new GUIContent("Sparkle Color", "Color of random water sparkle hits."));
                        DrawProperty("_SparkleIntensity", new GUIContent("Brightness", "How bright each sparkle is. Increase this first if sparkles are too subtle."));
                        DrawProperty("_SparkleAmount", new GUIContent("Amount", "The easiest control for how many sparkles appear. 0 is rare, 1 is many."));
                        DrawProperty("_SparkleSize", new GUIContent("Size", "Visual size of each sparkle dot."));
                        DrawProperty("_SparkleScale", new GUIContent("Density", "Higher values create smaller, denser sparkle placement cells."));
                        DrawProperty("_SparkleSpeed", new GUIContent("Flicker Speed", "How quickly sparkle positions refresh over time."));
                        DrawProperty("_SparkleViewStrength", new GUIContent("View Angle Boost", "Makes sparkles stronger near grazing angles."));
                        DrawProperty("_SparkleThreshold", new GUIContent("Advanced Rarity Cap", "Advanced limiter. Usually leave this around 0.98-0.99 and use Amount instead."));
                        EditorGUILayout.HelpBox("Use Brightness + Amount first. Size controls dot readability. Advanced Rarity Cap is mostly for fine tuning after the look is close.", MessageType.None);
                        EditorGUI.indentLevel--;
                    }
                );
            }
        );
    }

    #endregion

    #region Motion and Advanced Sections

    private void DrawMotionSection(Material material)
    {
        DrawSection(
            "Surface Motion",
            ref motionFoldout,
            null,
            () =>
            {
                EditorGUILayout.LabelField("Procedural Wave Normals", EditorStyles.boldLabel);
                DrawProperty("_ProceduralWaveStrength", new GUIContent("Wave Strength", "Procedural normal strength. This works without normal maps."));
                DrawProperty("_ProceduralWaveScale", new GUIContent("Wave Scale", "Size of procedural wave bands."));
                DrawProperty("_ProceduralWaveSpeed", new GUIContent("Wave Speed", "Animation speed for procedural wave normals."));

                DrawSeparator();

                EditorGUILayout.LabelField("Optional Normal Maps", EditorStyles.boldLabel);
                DrawProperty("_WaveNormalA", new GUIContent("Wave Normal A", "Optional tangent-space normal map layer."));
                DrawProperty("_WaveNormalB", new GUIContent("Wave Normal B", "Optional tangent-space normal map layer."));
                DrawProperty("_WaveStrength", new GUIContent("Texture Strength", "Strength of assigned normal map layers."));
                DrawProperty("_WaveScale", new GUIContent("Texture Scale", "Scale for assigned normal map layers."));
                DrawProperty("_WaveSpeedA", new GUIContent("Wave Speed A", "Scroll direction and speed for Wave Normal A."));
                DrawProperty("_WaveSpeedB", new GUIContent("Wave Speed B", "Scroll direction and speed for Wave Normal B."));

                DrawSeparator();

                DrawInlineFeatureButton(
                    material,
                    "_VertexWaveEnabled",
                    "_PEAR_WATER_VERTEX_WAVE_ON",
                    "Mesh Wave",
                    () =>
                    {
                        EditorGUI.indentLevel++;
                        DrawProperty("_VertexWaveAmount", new GUIContent("Movement Amount", "Controls how much the mesh physically waves up and down. This is the main mesh motion control."));
                        DrawProperty("_VertexWaveScale", new GUIContent("Wave Size", "Size/frequency of the mesh wave. Lower values create broader waves."));
                        DrawProperty("_VertexWaveSpeed", new GUIContent("Wave Speed", "Animation speed for the mesh wave."));
                        EditorGUILayout.HelpBox("Movement Amount controls how much the mesh itself waves around. This needs a subdivided water mesh to look smooth; simple planes have too few vertices.", MessageType.None);
                        EditorGUI.indentLevel--;
                    }
                );
            }
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

                if (GUILayout.Button(new GUIContent("Reset Pear Water Keywords", "Rebuilds all Pear Water shader keywords from the material toggle values.")))
                {
                    ApplyToTargets(PearWaterPresetApplier.SyncAllKeywords);
                }

                EditorGUILayout.Space(8);
                DrawMaterialValidation(material);

                EditorGUILayout.Space(6);
                materialEditor.RenderQueueField();
                materialEditor.EnableInstancingField();
                materialEditor.DoubleSidedGIField();
            }
        );
    }

    #endregion

    #region Material Validation

    private void DrawMaterialValidation(Material material)
    {
        if (material == null)
        {
            return;
        }

        EditorGUILayout.LabelField("Material Validation", EditorStyles.boldLabel);

        bool hasWarnings = false;

        if (IsMaterialFeatureEnabled(material, "_RimEnabled") && GetMaterialFloat(material, "_RimIntensity") <= 0.001f)
        {
            DrawValidationWarning("Rim Highlight is enabled, but Rim Intensity is 0. The feature is active but invisible.");
            hasWarnings = true;
        }

        if (IsMaterialFeatureEnabled(material, "_SubsurfaceEnabled"))
        {
            DrawValidationInfo("Subsurface Glow is a stylized transmission effect. Depth Influence uses URP Depth Texture when available.");

            if (GetMaterialFloat(material, "_SubsurfaceStrength") <= 0.001f)
            {
                DrawValidationWarning("Subsurface Glow is enabled, but Glow Strength is 0. The feature is active but invisible.");
                hasWarnings = true;
            }
        }

        if (IsMaterialFeatureEnabled(material, "_FoamEnabled") && GetMaterialFloat(material, "_FoamAmount") <= 0.001f)
        {
            DrawValidationWarning("Foam is enabled, but Foam Amount is 0. The feature is active but invisible.");
            hasWarnings = true;
        }

        if (IsMaterialFeatureEnabled(material, "_ContactFoamEnabled"))
        {
            DrawValidationInfo("Contact Foam requires URP Depth Texture. If it does not appear, enable Depth Texture on the URP Asset or Camera.");

            if (GetMaterialFloat(material, "_ContactFoamStrength") <= 0.001f)
            {
                DrawValidationWarning("Contact Foam is enabled, but Strength is 0. The feature is active but invisible.");
                hasWarnings = true;
            }
        }

        if (IsMaterialFeatureEnabled(material, "_FlowEnabled"))
        {
            DrawValidationInfo("Surface Motion adds animated organic highlight patches. Increase Brightness or Patch Thickness if the water still feels too flat.");

            if (GetMaterialFloat(material, "_FlowIntensity") <= 0.001f)
            {
                DrawValidationWarning("Surface Motion is enabled, but Brightness is 0. The feature is active but invisible.");
                hasWarnings = true;
            }
        }

        if (IsMaterialFeatureEnabled(material, "_SparkleEnabled"))
        {
            DrawValidationInfo("Surface Sparkle uses layered procedural randomness. It is less grid-like, but very high density can still look busy.");

            if (GetMaterialFloat(material, "_SparkleIntensity") <= 0.001f)
            {
                DrawValidationWarning("Surface Sparkle is enabled, but Brightness is 0. The feature is active but invisible.");
                hasWarnings = true;
            }
        }

        if (IsMaterialFeatureEnabled(material, "_VertexWaveEnabled"))
        {
            DrawValidationInfo("Mesh Wave physically moves vertices. Use subdivided water meshes for smoother motion.");

            if (GetMaterialFloat(material, "_VertexWaveAmount") <= 0.001f)
            {
                DrawValidationWarning("Mesh Wave is enabled, but Movement Amount is 0.");
                hasWarnings = true;
            }

            if (GetMaterialFloat(material, "_VertexWaveAmount") > 0.025f)
            {
                DrawValidationWarning("Mesh Wave Movement Amount is high. Thick meshes may wobble or stretch.");
                hasWarnings = true;
            }
        }

        if (IsMaterialFeatureEnabled(material, "_TopSurfaceOnly"))
        {
            DrawValidationInfo("Top Surface Only discards steep side faces. This is useful for demo slabs and pool surfaces.");
        }

        if (!hasWarnings)
        {
            EditorGUILayout.HelpBox("No obvious Pear Water material issues found.", MessageType.Info);
        }
    }

    private static void DrawValidationWarning(string message)
    {
        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }

    private static void DrawValidationInfo(string message)
    {
        EditorGUILayout.HelpBox(message, MessageType.None);
    }

    private static bool HasProperty(Material material, string propertyName)
    {
        return material != null && material.HasProperty(propertyName);
    }

    private static bool IsMaterialFeatureEnabled(Material material, string propertyName)
    {
        return HasProperty(material, propertyName) && material.GetFloat(propertyName) > 0.5f;
    }

    private static float GetMaterialFloat(Material material, string propertyName)
    {
        return HasProperty(material, propertyName) ? material.GetFloat(propertyName) : 0f;
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

            if (!string.IsNullOrEmpty(keyword))
            {
                SetKeyword(material, keyword, enabled);
            }

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
        if (material == null || string.IsNullOrEmpty(keyword))
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

    private void DrawMissingPropertyWarning(string propertyName)
    {
        EditorGUILayout.HelpBox("Missing shader property: " + propertyName, MessageType.Warning);
    }

    private void ApplyToTargets(System.Action<Material> action)
    {
        foreach (Object target in materialEditor.targets)
        {
            if (target is Material mat)
            {
                action(mat);
            }
        }
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
