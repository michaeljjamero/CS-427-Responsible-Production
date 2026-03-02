using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyVisualController : MonoBehaviour
{
    [Range(0, 100)]
    public float cleanEnergy = 0f;

    [Header("References")]
    public Light sunDirectionalLight;
    public Light[] interiorLightsToFlicker;

    [Header("Skybox (Skybox/Cubemap)")]
    public float skyMinExposure = 0.05f;
    public float skyMaxExposure = 1.00f;
    public Color skyLowEnergyTint = new Color(1.0f, 0.55f, 0.35f);   // orange-ish
    public Color skyHighEnergyTint = new Color(0.75f, 0.85f, 1.0f);  // subtle blue-ish

    [Header("Sun")]
    public float sunMinIntensity = 0.20f;
    public float sunMaxIntensity = 0.90f;
    public Color sunLowEnergyColor = new Color(1.0f, 0.60f, 0.40f);  // warm
    public Color sunHighEnergyColor = new Color(1.0f, 0.98f, 0.90f); // near-white

    [Header("Ambient")]
    public Color ambientLow = new Color(0.05f, 0.05f, 0.05f);
    public Color ambientHigh = new Color(0.70f, 0.70f, 0.70f);

    [Header("Flicker")]
    [Range(0, 100)] public float flickerThreshold = 30f;
    public float flickerAmount = 0.25f;
    public float flickerSpeed = 8f;

    [Header("Reflection Probes (Optional)")]
    public ReflectionProbe[] reflectionProbes;
    [Range(0.5f, 10f)] public float probeRefreshSeconds = 2f;   // how often to refresh
    [Range(0f, 5f)] public float energyRefreshDelta = 5f;       // refresh if energy changes by this much
    public bool refreshProbes = true;

    private float _nextProbeRefreshTime = 0f;
    private float _lastProbeEnergy = -999f;

    float[] baseInteriorIntensities;

    // Skybox/Cubemap property names (Unity built-in)
    static readonly int SkyTintProp = Shader.PropertyToID("_Tint");
    static readonly int SkyExposureProp = Shader.PropertyToID("_Exposure");

    void Start()
    {
        // Ensure we have a skybox assigned
        if (RenderSettings.skybox == null)
            Debug.LogWarning("No skybox material assigned in RenderSettings.");

        // Cache interior light base intensities
        if (interiorLightsToFlicker != null && interiorLightsToFlicker.Length > 0)
        {
            baseInteriorIntensities = new float[interiorLightsToFlicker.Length];
            for (int i = 0; i < interiorLightsToFlicker.Length; i++)
                baseInteriorIntensities[i] = interiorLightsToFlicker[i] ? interiorLightsToFlicker[i].intensity : 0f;
        }
    }

    void Update()
    {
        float t = Mathf.Clamp01(cleanEnergy / 100f);

        // Sun
        if (sunDirectionalLight != null)
        {
            sunDirectionalLight.intensity = Mathf.Lerp(sunMinIntensity, sunMaxIntensity, t);
            sunDirectionalLight.color = Color.Lerp(sunLowEnergyColor, sunHighEnergyColor, t);
        }

        // Ambient (works well since you set Ambient Mode to Realtime)
        RenderSettings.ambientLight = Color.Lerp(ambientLow, ambientHigh, t);

        // Skybox Tint + Exposure
        var sky = RenderSettings.skybox;
        bool skyChanged = false;
        if (sky != null)
        {
            Color newTint = Color.Lerp(skyLowEnergyTint, skyHighEnergyTint, t);
            float newExposure = Mathf.Lerp(skyMinExposure, skyMaxExposure, t);

            // Only set if meaningfully different (avoids pointless work)
            if (sky.GetColor(SkyTintProp) != newTint)
            {
                sky.SetColor(SkyTintProp, newTint);
                skyChanged = true;
            }

            // Skybox/Cubemap uses _Exposure
            if (!Mathf.Approximately(sky.GetFloat(SkyExposureProp), newExposure))
            {
                sky.SetFloat(SkyExposureProp, newExposure);
                skyChanged = true;
            }
        }

        // --- Reflection refresh (Option B) ---
        // If you're using probes, they won't update unless you render them.
        if (refreshProbes && reflectionProbes != null && reflectionProbes.Length > 0)
        {
            bool energyMovedEnough = Mathf.Abs(cleanEnergy - _lastProbeEnergy) >= energyRefreshDelta;
            bool timeToRefresh = Time.time >= _nextProbeRefreshTime;

            // Refresh when either:
            // - Sky changed AND enough time passed, OR
            // - Energy moved enough AND enough time passed
            if ((skyChanged || energyMovedEnough) && timeToRefresh)
            {
                // Updates Unity's environment lighting/reflections derived from the skybox
                DynamicGI.UpdateEnvironment();

                // Re-render probes (expensive, so do it sparingly)
                for (int i = 0; i < reflectionProbes.Length; i++)
                {
                    var p = reflectionProbes[i];
                    if (p == null) continue;

                    // Make sure each probe is set to Realtime in Inspector
                    // and Refresh Mode is Via Scripting (recommended).
                    p.RenderProbe();
                }

                _lastProbeEnergy = cleanEnergy;
                _nextProbeRefreshTime = Time.time + probeRefreshSeconds;
            }
        }

        // Interior flicker when energy is low
        if (interiorLightsToFlicker != null && baseInteriorIntensities != null)
        {
            if (cleanEnergy <= flickerThreshold)
            {
                float strength = 1f - Mathf.InverseLerp(0f, flickerThreshold, cleanEnergy);

                for (int i = 0; i < interiorLightsToFlicker.Length; i++)
                {
                    var l = interiorLightsToFlicker[i];
                    if (l == null) continue;

                    float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, i * 13.37f);
                    float offset = (noise - 0.5f) * 2f * flickerAmount * strength;

                    l.intensity = Mathf.Max(0f, baseInteriorIntensities[i] + offset);
                }
            }
            else
            {
                for (int i = 0; i < interiorLightsToFlicker.Length; i++)
                {
                    var l = interiorLightsToFlicker[i];
                    if (l == null) continue;
                    l.intensity = baseInteriorIntensities[i];
                }
            }
        }
    }
}