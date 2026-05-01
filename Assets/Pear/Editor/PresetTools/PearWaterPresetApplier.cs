using UnityEditor;
using UnityEngine;

public static class PearWaterPresetApplier
{
    private const string RimKeyword = "_PEAR_WATER_RIM_ON";
    private const string SubsurfaceKeyword = "_PEAR_WATER_SUBSURFACE_ON";
    private const string FoamKeyword = "_PEAR_WATER_FOAM_ON";
    private const string ContactFoamKeyword = "_PEAR_WATER_CONTACT_FOAM_ON";
    private const string FlowKeyword = "_PEAR_WATER_FLOW_ON";
    private const string SparkleKeyword = "_PEAR_WATER_SPARKLE_ON";
    private const string VertexWaveKeyword = "_PEAR_WATER_VERTEX_WAVE_ON";

    public static void ApplyCup(Material material)
    {
        if (!Validate(material)) return;
        Undo.RecordObject(material, "Apply Pear Water Cup Preset");

        SetColor(material, "_BaseColor", new Color(0.72f, 0.9f, 0.86f, 0.42f));
        SetFloat(material, "_Opacity", 0.48f);
        SetFloat(material, "_TopSurfaceOnly", 1f);
        SetFloat(material, "_SurfaceTintStrength", 0.8f);
        SetLighting(material, 1.05f, new Color(0.17f, 0.31f, 0.31f, 1f), new Color(0.13f, 0.27f, 0.27f, 1f), 0.42f, 0.34f);
        SetRim(material, true, new Color(0.93f, 1f, 0.72f, 1f), 2.4f, 0.85f);
        SetSubsurface(material, true, new Color(0.72f, 0.96f, 0.88f, 1f), 0.18f, 2.4f, 0.25f, 0.18f, 0.8f);
        SetWaves(material, 0.06f, 1.0f, 0.20f, 5.5f, 0.36f, 0.02f, 0.014f, -0.016f, 0.018f);
        SetFoam(material, false, new Color(0.95f, 1f, 0.75f, 1f), 0f, 4f, 0.75f, 0.2f, 0f);
        SetContactFoam(material, true, new Color(0.88f, 1f, 0.78f, 1f), 0.35f, 0.12f, 0.14f, 0.22f, 5.0f, 0.018f, 0.012f);
        SetFlow(material, true, new Color(0.9f, 1f, 0.82f, 1f), 0.12f, 2.4f, 0.16f, 0.20f, 0.42f);
        SetSparkle(material, false, new Color(0.95f, 1f, 0.65f, 1f), 0f, 0.2f, 0.08f, 90f, 1f, 0.985f, 0.8f);
        SetVertexWave(material, false, 0.004f, 1.1f, 0.4f);

        Finish(material);
    }

    public static void ApplyPond(Material material)
    {
        if (!Validate(material)) return;
        Undo.RecordObject(material, "Apply Pear Water Pond Preset");

        SetColor(material, "_BaseColor", new Color(0.35f, 0.74f, 0.58f, 0.6f));
        SetFloat(material, "_Opacity", 0.62f);
        SetFloat(material, "_TopSurfaceOnly", 1f);
        SetFloat(material, "_SurfaceTintStrength", 1.0f);
        SetLighting(material, 1.0f, new Color(0.12f, 0.28f, 0.22f, 1f), new Color(0.08f, 0.23f, 0.18f, 1f), 0.5f, 0.24f);
        SetRim(material, true, new Color(0.82f, 1f, 0.62f, 1f), 3.4f, 0.72f);
        SetSubsurface(material, true, new Color(0.66f, 1f, 0.58f, 1f), 0.36f, 2.1f, 0.42f, 0.42f, 1.2f);
        SetWaves(material, 0.08f, 1.15f, 0.34f, 7.2f, 0.72f, 0.03f, 0.018f, -0.022f, 0.026f);
        SetFoam(material, true, new Color(0.84f, 1f, 0.68f, 1f), 0.08f, 8.0f, 0.88f, 0.2f, 0f);
        SetContactFoam(material, true, new Color(0.86f, 1f, 0.62f, 1f), 0.72f, 0.32f, 0.22f, 0.52f, 8.0f, 0.036f, 0.022f);
        SetFlow(material, true, new Color(0.82f, 1f, 0.68f, 1f), 0.42f, 3.35f, 0.55f, 0.30f, 0.88f);
        SetSparkle(material, true, new Color(0.9f, 1f, 0.62f, 1f), 1.1f, 0.32f, 0.085f, 90f, 0.8f, 0.985f, 0.85f);
        SetVertexWave(material, true, 0.022f, 1.18f, 0.90f);

        Finish(material);
    }

    public static void ApplyPool(Material material)
    {
        if (!Validate(material)) return;
        Undo.RecordObject(material, "Apply Pear Water Pool Preset");

        SetColor(material, "_BaseColor", new Color(0.42f, 0.9f, 0.94f, 0.5f));
        SetFloat(material, "_Opacity", 0.5f);
        SetFloat(material, "_TopSurfaceOnly", 1f);
        SetFloat(material, "_SurfaceTintStrength", 1.1f);
        SetLighting(material, 1.2f, new Color(0.14f, 0.33f, 0.36f, 1f), new Color(0.1f, 0.28f, 0.34f, 1f), 0.42f, 0.36f);
        SetRim(material, true, new Color(0.82f, 1f, 0.96f, 1f), 2.2f, 1.05f);
        SetSubsurface(material, true, new Color(0.62f, 0.96f, 1f, 1f), 0.32f, 2.0f, 0.48f, 0.34f, 1.4f);
        SetWaves(material, 0.10f, 1.25f, 0.32f, 7.6f, 0.60f, 0.05f, 0.025f, -0.032f, 0.04f);
        SetFoam(material, true, new Color(0.9f, 1f, 0.95f, 1f), 0.06f, 9f, 0.9f, 0.15f, 0f);
        SetContactFoam(material, true, new Color(0.86f, 1f, 0.96f, 1f), 0.55f, 0.24f, 0.18f, 0.30f, 7.0f, 0.028f, 0.016f);
        SetFlow(material, true, new Color(0.82f, 1f, 0.96f, 1f), 0.34f, 2.9f, 0.42f, 0.26f, 0.72f);
        SetSparkle(material, true, new Color(0.95f, 1f, 0.95f, 1f), 1.35f, 0.38f, 0.08f, 110f, 1.0f, 0.985f, 1.15f);
        SetVertexWave(material, true, 0.013f, 1.38f, 0.75f);

        Finish(material);
    }

    public static void ApplyMagic(Material material)
    {
        if (!Validate(material)) return;
        Undo.RecordObject(material, "Apply Pear Water Magic Preset");

        SetColor(material, "_BaseColor", new Color(0.62f, 0.82f, 1f, 0.56f));
        SetFloat(material, "_Opacity", 0.56f);
        SetFloat(material, "_TopSurfaceOnly", 1f);
        SetFloat(material, "_SurfaceTintStrength", 1.25f);
        SetLighting(material, 1.35f, new Color(0.2f, 0.22f, 0.45f, 1f), new Color(0.18f, 0.15f, 0.38f, 1f), 0.46f, 0.32f);
        SetRim(material, true, new Color(0.95f, 1f, 0.52f, 1f), 2f, 1.55f);
        SetSubsurface(material, true, new Color(0.72f, 0.82f, 1f, 1f), 0.62f, 1.7f, 0.75f, 0.55f, 1.1f);
        SetWaves(material, 0.12f, 1.35f, 0.44f, 8.8f, 0.95f, 0.06f, 0.04f, -0.045f, 0.055f);
        SetFoam(material, true, new Color(0.95f, 1f, 0.55f, 1f), 0.10f, 8.5f, 0.88f, 0.14f, 0f);
        SetContactFoam(material, true, new Color(0.95f, 1f, 0.48f, 1f), 1.05f, 0.38f, 0.20f, 0.60f, 9.0f, 0.050f, 0.035f);
        SetFlow(material, true, new Color(0.95f, 1f, 0.52f, 1f), 0.74f, 4.0f, 0.90f, 0.34f, 1.12f);
        SetSparkle(material, true, new Color(0.95f, 1f, 0.45f, 1f), 2.35f, 0.52f, 0.14f, 125f, 1.5f, 0.978f, 1.75f);
        SetVertexWave(material, true, 0.040f, 1.28f, 1.18f);

        Finish(material);
    }

    public static void SyncAllKeywords(Material material)
    {
        if (!material) return;
        SetKeyword(material, RimKeyword, GetFloat(material, "_RimEnabled") > 0.5f);
        SetKeyword(material, SubsurfaceKeyword, GetFloat(material, "_SubsurfaceEnabled") > 0.5f);
        SetKeyword(material, FoamKeyword, GetFloat(material, "_FoamEnabled") > 0.5f);
        SetKeyword(material, ContactFoamKeyword, GetFloat(material, "_ContactFoamEnabled") > 0.5f);
        SetKeyword(material, FlowKeyword, GetFloat(material, "_FlowEnabled") > 0.5f);
        SetKeyword(material, SparkleKeyword, GetFloat(material, "_SparkleEnabled") > 0.5f);
        SetKeyword(material, VertexWaveKeyword, GetFloat(material, "_VertexWaveEnabled") > 0.5f);
    }

    private static void SetLighting(Material mat, float intensity, Color ambient, Color shadow, float threshold, float softness)
    {
        SetFloat(mat, "_LightIntensity", intensity);
        SetColor(mat, "_AmbientColor", ambient);
        SetColor(mat, "_ShadowColor", shadow);
        SetFloat(mat, "_ToonThreshold", threshold);
        SetFloat(mat, "_ToonSoftness", softness);
    }

    private static void SetRim(Material mat, bool enabled, Color color, float power, float intensity)
    {
        SetFloat(mat, "_RimEnabled", enabled ? 1f : 0f);
        SetColor(mat, "_RimColor", color);
        SetFloat(mat, "_RimPower", power);
        SetFloat(mat, "_RimIntensity", intensity);
    }

    private static void SetSubsurface(Material mat, bool enabled, Color color, float strength, float viewPower, float lightStrength, float depthStrength, float depthDistance)
    {
        SetFloat(mat, "_SubsurfaceEnabled", enabled ? 1f : 0f);
        SetColor(mat, "_SubsurfaceColor", color);
        SetFloat(mat, "_SubsurfaceStrength", strength);
        SetFloat(mat, "_SubsurfaceViewPower", viewPower);
        SetFloat(mat, "_SubsurfaceLightStrength", lightStrength);
        SetFloat(mat, "_SubsurfaceDepthStrength", depthStrength);
        SetFloat(mat, "_SubsurfaceDepthDistance", depthDistance);
    }

    private static void SetWaves(Material mat, float texStrength, float texScale, float procStrength, float procScale, float procSpeed, float speedAX, float speedAY, float speedBX, float speedBY)
    {
        SetFloat(mat, "_WaveStrength", texStrength);
        SetFloat(mat, "_WaveScale", texScale);
        SetFloat(mat, "_ProceduralWaveStrength", procStrength);
        SetFloat(mat, "_ProceduralWaveScale", procScale);
        SetFloat(mat, "_ProceduralWaveSpeed", procSpeed);
        SetVector(mat, "_WaveSpeedA", new Vector4(speedAX, speedAY, 0f, 0f));
        SetVector(mat, "_WaveSpeedB", new Vector4(speedBX, speedBY, 0f, 0f));
    }

    private static void SetFoam(Material mat, bool enabled, Color color, float amount, float scale, float threshold, float softness, float textureBlend)
    {
        SetFloat(mat, "_FoamEnabled", enabled ? 1f : 0f);
        SetColor(mat, "_FoamColor", color);
        SetFloat(mat, "_FoamAmount", amount);
        SetFloat(mat, "_FoamScale", scale);
        SetVector(mat, "_FoamSpeed", new Vector4(0.03f, 0.02f, 0f, 0f));
        SetFloat(mat, "_FoamThreshold", threshold);
        SetFloat(mat, "_FoamSoftness", softness);
        SetFloat(mat, "_FoamTextureBlend", textureBlend);
    }

    private static void SetContactFoam(Material mat, bool enabled, Color color, float strength, float distance, float softness, float noiseAmount, float noiseScale, float speedX, float speedY)
    {
        SetFloat(mat, "_ContactFoamEnabled", enabled ? 1f : 0f);
        SetColor(mat, "_ContactFoamColor", color);
        SetFloat(mat, "_ContactFoamStrength", strength);
        SetFloat(mat, "_ContactFoamDistance", distance);
        SetFloat(mat, "_ContactFoamSoftness", softness);
        SetFloat(mat, "_ContactFoamNoiseAmount", noiseAmount);
        SetFloat(mat, "_ContactFoamNoiseScale", noiseScale);
        SetVector(mat, "_ContactFoamNoiseSpeed", new Vector4(speedX, speedY, 0f, 0f));
    }

    private static void SetFlow(Material mat, bool enabled, Color color, float intensity, float scale, float speed, float thickness, float distortion)
    {
        SetFloat(mat, "_FlowEnabled", enabled ? 1f : 0f);
        SetColor(mat, "_FlowColor", color);
        SetFloat(mat, "_FlowIntensity", intensity);
        SetFloat(mat, "_FlowScale", scale);
        SetFloat(mat, "_FlowSpeed", speed);
        SetFloat(mat, "_FlowThickness", thickness);
        SetFloat(mat, "_FlowDistortion", distortion);
    }

    private static void SetSparkle(Material mat, bool enabled, Color color, float intensity, float amount, float size, float density, float speed, float threshold, float viewStrength)
    {
        SetFloat(mat, "_SparkleEnabled", enabled ? 1f : 0f);
        SetColor(mat, "_SparkleColor", color);
        SetFloat(mat, "_SparkleIntensity", intensity);
        SetFloat(mat, "_SparkleAmount", amount);
        SetFloat(mat, "_SparkleSize", size);
        SetFloat(mat, "_SparkleScale", density);
        SetFloat(mat, "_SparkleSpeed", speed);
        SetFloat(mat, "_SparkleThreshold", threshold);
        SetFloat(mat, "_SparkleViewStrength", viewStrength);
    }

    private static void SetVertexWave(Material mat, bool enabled, float amount, float scale, float speed)
    {
        SetFloat(mat, "_VertexWaveEnabled", enabled ? 1f : 0f);
        SetFloat(mat, "_VertexWaveAmount", amount);
        SetFloat(mat, "_VertexWaveScale", scale);
        SetFloat(mat, "_VertexWaveSpeed", speed);
    }

    private static void Finish(Material material)
    {
        SyncAllKeywords(material);
        EditorUtility.SetDirty(material);
    }

    private static bool Validate(Material material)
    {
        if (material != null) return true;
        Debug.LogWarning("Pear Water preset failed because no material was selected.");
        return false;
    }

    private static void SetKeyword(Material material, string keyword, bool enabled)
    {
        if (enabled) material.EnableKeyword(keyword);
        else material.DisableKeyword(keyword);
    }

    private static float GetFloat(Material material, string property)
    {
        return material.HasProperty(property) ? material.GetFloat(property) : 0f;
    }

    private static void SetFloat(Material material, string property, float value)
    {
        if (material.HasProperty(property)) material.SetFloat(property, value);
    }

    private static void SetColor(Material material, string property, Color value)
    {
        if (material.HasProperty(property)) material.SetColor(property, value);
    }

    private static void SetVector(Material material, string property, Vector4 value)
    {
        if (material.HasProperty(property)) material.SetVector(property, value);
    }
}
