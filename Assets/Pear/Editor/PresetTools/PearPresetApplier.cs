using UnityEditor;
using UnityEngine;

public static class PearPresetApplier
{
    #region Demo Base Color

    // Turn this on only if you want all presets to force the same base color.
    // Good for clean comparison screenshots.
    private static readonly bool ForceSharedDemoBaseColor = false;

    // Slightly more pear-like base color.
    private static readonly Color SharedDemoBaseColor = new Color(0.84f, 0.95f, 0.42f, 1f);

    #endregion

    #region Presets


    public static void ApplyDefaultPear(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Pear: clean neutral starter material.
        SetColorIfExists(mat, "_BaseColor", Color.white);
        SetFloatIfExists(mat, "_NormalStrength", 1f);

        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", false);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFloatIfExists(mat, "_ToonThreshold", 0.409f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.20f);
        SetColorIfExists(mat, "_ShadowColor", new Color(0.63f, 0.72f, 0.56f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.13f, 0.16f, 0.11f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.0f);

        SetColorIfExists(mat, "_MidtoneColor", new Color(0.74f, 0.82f, 0.56f, 1f));
        SetFloatIfExists(mat, "_MidtoneThreshold", 0.35f);
        SetFloatIfExists(mat, "_MidtoneSoftness", 0.04f);
        SetFloatIfExists(mat, "_RampStrength", 1f);

        SetColorIfExists(mat, "_RimColor", new Color(0.98f, 1.00f, 0.90f, 1f));
        SetFloatIfExists(mat, "_RimPower", 3.5f);
        SetFloatIfExists(mat, "_RimIntensity", 0f);

        SetColorIfExists(mat, "_EmissionColor", new Color(0.75f, 1.0f, 0.75f, 1f));
        SetColorIfExists(mat, "_MatcapColor", Color.white);

        SetColorIfExists(mat, "_ShimmerColor", new Color(0.95f, 1.00f, 0.72f, 1f));
        SetFloatIfExists(mat, "_ShimmerScale", 48f);
        SetFloatIfExists(mat, "_ShimmerSpeed", 4f);
        SetFloatIfExists(mat, "_ShimmerThreshold", 0.92f);
        SetFloatIfExists(mat, "_ShimmerViewStrength", 0.6f);

        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", false);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.12f, 0.16f, 0.08f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.015f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0f);

        SyncAllKeywords(mat);
        EndPreset(mat);
    }

    public static void ApplySoftAnime(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Cozy: warm, gentle toon preset with soft rim and thin outline.
        SetFloatIfExists(mat, "_ToonThreshold", 0.52f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.075f);
        SetColorIfExists(mat, "_ShadowColor", new Color(0.66f, 0.74f, 0.60f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.15f, 0.17f, 0.13f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.0f);

        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", false);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", true);
        SetColorIfExists(mat, "_RimColor", new Color(0.97f, 1.00f, 0.89f, 1f));
        SetFloatIfExists(mat, "_RimPower", 3.15f);
        SetFloatIfExists(mat, "_RimIntensity", 0.48f);

        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", false);

        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", true);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.52f, 0.62f, 0.30f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.0201f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", -0.0146f);

        SyncAllKeywords(mat);
        EndPreset(mat);
    }

    public static void ApplyCelShadow(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Bold: hard graphic bands, heavier silhouette, and high contrast pear tones.
        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", true);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFloatIfExists(mat, "_MidtoneThreshold", 0.32f);
        SetFloatIfExists(mat, "_MidtoneSoftness", 0.001f);
        SetColorIfExists(mat, "_MidtoneColor", new Color(0.82f, 0.91f, 0.58f, 1f));

        SetFloatIfExists(mat, "_ToonThreshold", 0.68f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.001f);

        SetColorIfExists(mat, "_ShadowColor", new Color(0.20f, 0.25f, 0.16f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.055f, 0.075f, 0.045f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.08f);

        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", false);
        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", false);

        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", true);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.08f, 0.12f, 0.05f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.03f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0.0f);

        SyncAllKeywords(mat);
        EndPreset(mat);
    }

    public static void ApplyGlossyToy(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Gloss: pearly, sparkly, toy-like finish.
        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", false);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFloatIfExists(mat, "_ToonThreshold", 0.66f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.25f);

        SetColorIfExists(mat, "_ShadowColor", new Color(0.50f, 0.57f, 0.46f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.17f, 0.19f, 0.14f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.15f);

        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", true);
        SetColorIfExists(mat, "_RimColor", new Color(0.98f, 1.00f, 0.76f, 1f));
        SetFloatIfExists(mat, "_RimPower", 4.97f);
        SetFloatIfExists(mat, "_RimIntensity", 0.98f);

        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFloatIfExists(mat, "_EmissionIntensity", 0f);
        SetFloatIfExists(mat, "_EmissionPulseSpeed", 0f);
        SetFloatIfExists(mat, "_EmissionPulseAmount", 0f);

        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFloatIfExists(mat, "_MatcapIntensity", 0f);
        SetFloatIfExists(mat, "_MatcapBlend", 0f);

        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", true);
        SetColorIfExists(mat, "_ShimmerColor", new Color(0.95f, 1.00f, 0.72f, 1f));
        SetFloatIfExists(mat, "_ShimmerIntensity", 5.0f);
        SetFloatIfExists(mat, "_ShimmerScale", 150.0f);
        SetFloatIfExists(mat, "_ShimmerSpeed", 20.0f);
        SetFloatIfExists(mat, "_ShimmerThreshold", 0.995f);
        SetFloatIfExists(mat, "_ShimmerViewStrength", 1.0f);

        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", false);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.18f, 0.20f, 0.14f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.012f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0.0f);

        SyncAllKeywords(mat);
        EndPreset(mat);
    }

    #endregion

    #region Reset

    private static void ResetFeatures(Material mat)
    {
        SetFeature(mat, "_HueShiftEnabled", "_PEAR_HUE_SHIFT_ON", false);
        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", false);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);
        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", false);
        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", false);
        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", false);

        SetFloatIfExists(mat, "_HueShift", 0f);
        SetFloatIfExists(mat, "_HueShiftSpeed", 0f);
        SetFloatIfExists(mat, "_HueSaturation", 1f);
        SetFloatIfExists(mat, "_HueBrightness", 1f);

        SetFloatIfExists(mat, "_EmissionIntensity", 0f);
        SetFloatIfExists(mat, "_EmissionPulseSpeed", 0f);
        SetFloatIfExists(mat, "_EmissionPulseAmount", 0f);

        SetFloatIfExists(mat, "_MatcapIntensity", 0f);
        SetFloatIfExists(mat, "_MatcapBlend", 0f);

        SetFloatIfExists(mat, "_RimIntensity", 0f);

        SetFloatIfExists(mat, "_ShimmerIntensity", 0f);
        SetFloatIfExists(mat, "_ShimmerSpeed", 0f);
        SetFloatIfExists(mat, "_ShimmerScale", 48f);
        SetFloatIfExists(mat, "_ShimmerThreshold", 0.92f);
        SetFloatIfExists(mat, "_ShimmerViewStrength", 0.6f);

        SetFloatIfExists(mat, "_OutlineWidth", 0f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0f);
    }

    private static void ApplySharedBaseIfEnabled(Material mat)
    {
        if (ForceSharedDemoBaseColor)
        {
            SetColorIfExists(mat, "_BaseColor", SharedDemoBaseColor);
        }
    }

    #endregion

    #region Preset Lifecycle

    private static void BeginPreset(Material mat)
    {
        if (mat == null)
        {
            return;
        }

        Undo.RecordObject(mat, "Apply Pear Preset");
    }

    private static void EndPreset(Material mat)
    {
        if (mat == null)
        {
            return;
        }

        EditorUtility.SetDirty(mat);

#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
    }

    #endregion

    #region Material Helpers


    public static void SyncAllKeywords(Material mat)
    {
        if (mat == null)
        {
            return;
        }

        SetFeature(mat, "_HueShiftEnabled", "_PEAR_HUE_SHIFT_ON", IsEnabled(mat, "_HueShiftEnabled"));
        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", IsEnabled(mat, "_MidtoneEnabled"));
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", IsEnabled(mat, "_RampEnabled"));
        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", IsEnabled(mat, "_RimEnabled"));
        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", IsEnabled(mat, "_EmissionEnabled"));
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", IsEnabled(mat, "_MatcapEnabled"));
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", IsEnabled(mat, "_ShimmerEnabled"));
        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", IsEnabled(mat, "_OutlineEnabled"));
    }

    private static bool IsEnabled(Material mat, string propertyName)
    {
        return mat != null && mat.HasProperty(propertyName) && mat.GetFloat(propertyName) > 0.5f;
    }

    private static void SetFeature(Material mat, string propertyName, string keyword, bool enabled)
    {
        if (mat == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(propertyName) && mat.HasProperty(propertyName))
        {
            mat.SetFloat(propertyName, enabled ? 1f : 0f);
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            if (enabled)
            {
                mat.EnableKeyword(keyword);
            }
            else
            {
                mat.DisableKeyword(keyword);
            }
        }
    }

    private static void SetFloatIfExists(Material mat, string propertyName, float value)
    {
        if (mat != null && mat.HasProperty(propertyName))
        {
            mat.SetFloat(propertyName, value);
        }
    }

    private static void SetColorIfExists(Material mat, string propertyName, Color value)
    {
        if (mat != null && mat.HasProperty(propertyName))
        {
            mat.SetColor(propertyName, value);
        }
    }

    #endregion
}