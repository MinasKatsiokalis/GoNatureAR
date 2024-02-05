using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class SetParticlesStrength : MonoBehaviour
{
    public static SetParticlesStrength Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    [SerializeField] VisualEffect vfx_CO2;
    [SerializeField] VisualEffect vfx_NO2;
    [SerializeField] VisualEffect vfx_O3;
    [SerializeField] VisualEffect vfx_PM2_5;
    [SerializeField] VisualEffect vfx_PM10;
    [SerializeField] GameObject fogParticles;

    [SerializeField] int defaultStrengthCO2;
    [SerializeField] int defaultStrengthNO2;
    [SerializeField] int defaultStrengthO3;
    [SerializeField] int defaultStrengthPM2_5;
    [SerializeField] int defaultStrengthPM10;

    public void Disable()
    {
        vfx_CO2.SetFloat("Strength", 0);
        vfx_NO2.SetFloat("Strength", 0);
        vfx_O3.SetFloat("Strength", 0);
        vfx_PM2_5.SetFloat("Strength", 0);
        vfx_PM10.SetFloat("Strength", 0);

        fogParticles.SetActive(false);
    }
}
