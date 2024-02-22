using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace GoNatureAR
{
    public class NoiseExposureManager : MonoBehaviour
    {
        public static Action<NoiseExposureIndex> OnNoiseCalculated;

        [SerializeField]
        VisualEffect _noiseDetailsVFX;
        [SerializeField]
        GameObject _noiseDetailsBackground;
        [SerializeField]
        TextMesh _noiseDetailsText;

        private NoiseExposureIndex noiseIndex;
        private double noise;

        private void OnEnable()
        {
            GameManager.OnChangeState += ChangeStateHandle;
            NarrationManager.OnConfirmed += TransferToLoudConditions;
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= ChangeStateHandle;
            NarrationManager.OnConfirmed -= TransferToLoudConditions;
        }

        private void ChangeStateHandle(State state)
        {
            if (state == State.Noise)
            {
                if (DataManager.Instance.NoiseData != null)
                    noise = DataManager.Instance.NoiseData[NoiseElements.Sound].Value;
                else
                    noise = 35;

                SetPanelBasedOnNoise(noise);
            }
        }

        private void SetPanelBasedOnNoise(double noise)
        {
            var color = new NoiseColors();
            if (noise > 55)
            {
                noiseIndex = NoiseExposureIndex.Loud;
                SetNoiseDetails(noiseIndex);
                _noiseDetailsVFX.SetVector4("TrailsColor", NoiseColors.Color2Vector4(color.LoudTrails));
                _noiseDetailsVFX.SetFloat("TrailsTurbulence", 1.5f);
                _noiseDetailsBackground.GetComponent<MeshRenderer>().material.SetColor("_Color", color.LoudBackground);
            }
            else
            {
                noiseIndex = NoiseExposureIndex.Faint;
                SetNoiseDetails(noiseIndex);
                _noiseDetailsVFX.SetVector4("TrailsColor", NoiseColors.Color2Vector4(color.FaintTrails));
                _noiseDetailsVFX.SetFloat("TrailsTurbulence", 0f);
                _noiseDetailsBackground.GetComponent<MeshRenderer>().material.SetColor("_Color", color.FaintBackground);
            }

            OnNoiseCalculated?.Invoke(noiseIndex);
        }

        private void SetNoiseDetails(NoiseExposureIndex index)
        {
            _noiseDetailsText.text = $"Noise Volume: {index}\nNoise: {noise} dB";
        }

        private void TransferToLoudConditions()
        {
            noise = 70;
            SetPanelBasedOnNoise(noise);
        }
    }

    public class NoiseColors
    {
        public Color FaintBackground;
        public Color FaintTrails;
        public Color FaintParticles;

        public Color LoudBackground;
        public Color LoudTrails;
        public Color LoudParticles;

        public NoiseColors()
        {
            FaintBackground = new Color(0.0f, 0.56f, 1.0f, 0.8f);
            FaintTrails = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            FaintParticles = new Color(0.5f, 1.0f, 0.9f, 1.0f);

            LoudBackground = new Color(1.0f, 0.1f, 0.0f, 0.8f);
            LoudTrails = new Color(1.0f, 0.5f, 0.0f, 1.0f);
            LoudParticles = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Vector4 Color2Vector4(Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
    }

}
