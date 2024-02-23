using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace GoNatureAR
{
    public class NoiseExposureManager : MonoBehaviour
    {
        public static Action<NoiseExposureIndex> OnNoiseCalculated;
        public static Action<NoiseExposureIndex> OnHandMenuEnabled;

        [SerializeField]
        HandConstraintPalmUp _handConstraintPalmUp;
        [SerializeField]
        VisualEffect _noiseDetailsVFX;
        [SerializeField]
        GameObject _noiseDetailsBackground;
        [SerializeField]
        TextMesh _noiseDetailsText;

        private NoiseExposureIndex noiseIndex;
        private double noise;
        private bool isHandMenuActive = false;
        private bool isHandMenuEnabled = false;

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

        private void Start()
        {
            _handConstraintPalmUp.OnHandActivate.AddListener(() => SetPalmUp(true));
            _handConstraintPalmUp.OnHandDeactivate.AddListener(() => SetPalmUp(false));
            _handConstraintPalmUp.OnHandActivate.AddListener(MenuEnabled);
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
            var renderer = _noiseDetailsBackground.GetComponent<MeshRenderer>();

            if (noise > 55)
            {
                noiseIndex = NoiseExposureIndex.Loud;
                SetNoiseDetails(noiseIndex);
                _noiseDetailsVFX.SetVector4("TrailsColor", NoiseColors.Color2Vector4(color.LoudTrails));
                _noiseDetailsVFX.SetFloat("TrailsTurbulence", 1.5f);
                renderer.material.SetColor("_Color", color.LoudBackground);
                renderer.material.SetColor("_InnerGlowColor", color.LoudBackground);
            }
            else
            {
                noiseIndex = NoiseExposureIndex.Faint;
                SetNoiseDetails(noiseIndex);
                _noiseDetailsVFX.SetVector4("TrailsColor", NoiseColors.Color2Vector4(color.FaintTrails));
                _noiseDetailsVFX.SetFloat("TrailsTurbulence", 0f);
                renderer.material.SetColor("_Color", color.FaintBackground);
                renderer.material.SetColor("_InnerGlowColor", color.FaintBackground);
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
            isHandMenuEnabled = false;
            SetPanelBasedOnNoise(noise);
        }

        private float timer = 0f;
        private async void MenuEnabled()
        {   
            if(isHandMenuEnabled)
                return;

            Debug.Log("Menu On");
            while (isHandMenuActive)
            {
                timer += Time.deltaTime;
                await Task.Yield();

                if(timer >= 2f)
                    break;
            }
            timer = 0f;
            Debug.Log("Two seconds have passed"); 
            OnHandMenuEnabled?.Invoke(noiseIndex);
            isHandMenuEnabled = true;
        }


        private void SetPalmUp(bool palmUp)
        {
            isHandMenuActive = palmUp;
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
