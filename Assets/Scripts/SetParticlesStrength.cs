using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class SetParticlesStrength : MonoBehaviour
{
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

    public void OnValueChange(SliderEventData sliderEventData)
    {
        vfx_CO2.SetFloat("Strength", (defaultStrengthCO2 * sliderEventData.NewValue));
        vfx_NO2.SetFloat("Strength", (defaultStrengthNO2 * sliderEventData.NewValue));
        vfx_O3.SetFloat("Strength", (defaultStrengthO3 * sliderEventData.NewValue));
        vfx_PM2_5.SetFloat("Strength", (defaultStrengthPM2_5 * sliderEventData.NewValue));
        vfx_PM10.SetFloat("Strength", (defaultStrengthPM10 * sliderEventData.NewValue));

        if (sliderEventData.NewValue <= 0)
            fogParticles.SetActive(false);
        else
            fogParticles.SetActive(true);

    }
}
