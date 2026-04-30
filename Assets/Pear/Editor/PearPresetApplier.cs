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

    public static void ApplySoftAnime(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Soft, character-friendly shading with a subtle pear/sage influence.
        SetFloatIfExists(mat, "_ToonThreshold", 0.52f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.075f);
        SetColorIfExists(mat, "_ShadowColor", new Color(0.66f, 0.74f, 0.60f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.15f, 0.17f, 0.13f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.0f);

        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", false);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", true);
        SetColorIfExists(mat, "_RimColor", new Color(0.97f, 1.00f, 0.89f, 1f));
        SetFloatIfExists(mat, "_RimPower", 5.0f);
        SetFloatIfExists(mat, "_RimIntensity", 0.18f);

        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", false);

        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", true);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.28f, 0.29f, 0.20f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.018f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0.0f);

        EndPreset(mat);
    }

    public static void ApplyCelShadow(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Hard, graphic cel-shaded look with olive/pear-inspired banding.
        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", true);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFloatIfExists(mat, "_MidtoneThreshold", 0.32f);
        SetFloatIfExists(mat, "_MidtoneSoftness", 0.002f);
        SetColorIfExists(mat, "_MidtoneColor", new Color(0.74f, 0.82f, 0.56f, 1f));

        SetFloatIfExists(mat, "_ToonThreshold", 0.68f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.002f);

        SetColorIfExists(mat, "_ShadowColor", new Color(0.18f, 0.22f, 0.15f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.06f, 0.08f, 0.05f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.08f);

        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", false);
        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", false);

        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", true);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.07f, 0.09f, 0.06f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.028f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0.0f);

        EndPreset(mat);
    }

    public static void ApplyGlossyToy(Material mat)
    {
        BeginPreset(mat);
        ResetFeatures(mat);
        ApplySharedBaseIfEnabled(mat);

        // Soft toy-like shading with a glossy pear-inspired finish.
        SetFeature(mat, "_MidtoneEnabled", "_PEAR_MIDTONE_ON", false);
        SetFeature(mat, "_RampEnabled", "_PEAR_RAMP_ON", false);

        SetFloatIfExists(mat, "_ToonThreshold", 0.66f);
        SetFloatIfExists(mat, "_ToonSoftness", 0.14f);

        SetColorIfExists(mat, "_ShadowColor", new Color(0.50f, 0.57f, 0.46f, 1f));
        SetColorIfExists(mat, "_AmbientColor", new Color(0.17f, 0.19f, 0.14f, 1f));
        SetFloatIfExists(mat, "_LightIntensity", 1.15f);

        // Rim Light ON
        SetFeature(mat, "_RimEnabled", "_PEAR_RIM_ON", true);
        SetColorIfExists(mat, "_RimColor", new Color(0.98f, 1.00f, 0.90f, 1f));
        SetFloatIfExists(mat, "_RimPower", 2.4f);
        SetFloatIfExists(mat, "_RimIntensity", 0.32f);

        // Emission OFF
        SetFeature(mat, "_EmissionEnabled", "_PEAR_EMISSION_ON", false);
        SetFloatIfExists(mat, "_EmissionIntensity", 0f);
        SetFloatIfExists(mat, "_EmissionPulseSpeed", 0f);
        SetFloatIfExists(mat, "_EmissionPulseAmount", 0f);

        // Matcap Reflection OFF
        SetFeature(mat, "_MatcapEnabled", "_PEAR_MATCAP_ON", false);
        SetFloatIfExists(mat, "_MatcapIntensity", 0f);
        SetFloatIfExists(mat, "_MatcapBlend", 0f);

        // Glitter Shimmer ON
        // These names must match PearToon.shader exactly.
        SetFeature(mat, "_ShimmerEnabled", "_PEAR_SHIMMER_ON", true);
        SetColorIfExists(mat, "_ShimmerColor", new Color(0.95f, 1.00f, 0.72f, 1f));
        SetFloatIfExists(mat, "_ShimmerIntensity", 5.0f);
        SetFloatIfExists(mat, "_ShimmerScale", 150.0f);
        SetFloatIfExists(mat, "_ShimmerSpeed", 20.0f);
        SetFloatIfExists(mat, "_ShimmerThreshold", 0.995f);
        SetFloatIfExists(mat, "_ShimmerViewStrength", 1.0f);

        // Keep outline off to preserve the current glossy look.
        SetFeature(mat, "_OutlineEnabled", "_PEAR_OUTLINE_ON", false);
        SetColorIfExists(mat, "_OutlineColor", new Color(0.18f, 0.20f, 0.14f, 1f));
        SetFloatIfExists(mat, "_OutlineWidth", 0.012f);
        SetFloatIfExists(mat, "_OutlineDepthOffset", 0.0f);

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