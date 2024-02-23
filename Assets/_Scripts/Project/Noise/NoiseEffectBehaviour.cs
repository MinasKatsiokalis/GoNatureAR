using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace GoNatureAR
{
    public class NoiseEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect _noiseVFX;
        [SerializeField]
        private AudioClip _faintAmbientMusic;
        [SerializeField]
        private AudioClip _loudAmbientMusic;
        [SerializeField]
        private AudioClip _loudAudioEffects;

        private const float loudNoiseVFXScale = 0.8f;
        private const float faintNoiseVFXScale = 0.35f;

        private const float loudTrailsLifetime = 2f;
        private const float loudTrailsRate = 15f;
        private const float loudTrailsTurbulence = 1f;

        private const float faintTrailsLifetime = 1.5f;
        private const float faintTrailsRate = 5f;
        private const float faintTrailsTurbulence = 0f;

        private Camera mainCamera;
        private AudioSource audioSource;

        private void Awake()
        {
            mainCamera = Camera.main;
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            NoiseExposureManager.OnNoiseCalculated += EnableNoiseEffects;
        }

        private void OnDisable()
        {
            NoiseExposureManager.OnNoiseCalculated -= EnableNoiseEffects;
        }

        void Start()
        {
            AttachToCamera();
        }

        // Update is called once per frame
        void Update()
        {
            AttachToCamera();
        }

        //a method that finds main camera and makes this game object a child of it
        private void AttachToCamera()
        {
            if (mainCamera != null)
            {
                this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2f;
            }
        }

        private void EnableNoiseEffects(NoiseExposureIndex index)
        {
            if (index == NoiseExposureIndex.Loud)
            {
                MusicManager.Instance.PlayMusic(_loudAmbientMusic);
                audioSource.clip = _loudAudioEffects;
                audioSource.Play();
                SetEffects(index);
            }
            else
            {
                MusicManager.Instance.PlayMusic(_faintAmbientMusic);
                SetEffects(index);
            }

            _noiseVFX.gameObject.SetActive(true);
        }

        private void SetEffects(NoiseExposureIndex index)
        {   
            var color = new NoiseColors();
            if (index == NoiseExposureIndex.Loud)
            {
                _noiseVFX.SetVector4("TrailsColor", NoiseColors.Color2Vector4(color.LoudTrails));
                _noiseVFX.SetVector4("Color", NoiseColors.Color2Vector4(color.LoudBackground));
                _noiseVFX.SetVector4("ParticlesColor", NoiseColors.Color2Vector4(color.LoudParticles));

                _noiseVFX.SetFloat("TrailsLifetime", loudTrailsLifetime);
                _noiseVFX.SetFloat("TrailsRate", loudTrailsRate);
                _noiseVFX.SetFloat("TrailsTurbulence", loudTrailsTurbulence);

                _noiseVFX.transform.DOScale(loudNoiseVFXScale, 1f);
            }
            else
            {
                _noiseVFX.SetVector4("TrailsColor", NoiseColors.Color2Vector4(color.FaintTrails));
                _noiseVFX.SetVector4("Color", NoiseColors.Color2Vector4(color.FaintBackground));
                _noiseVFX.SetVector4("ParticlesColor", NoiseColors.Color2Vector4(color.FaintParticles));

                _noiseVFX.SetFloat("TrailsLifetime", faintTrailsLifetime);
                _noiseVFX.SetFloat("TrailsRate", faintTrailsRate);
                _noiseVFX.SetFloat("TrailsTurbulence", faintTrailsTurbulence);

                _noiseVFX.transform.DOScale(faintNoiseVFXScale, 1f);
            }
        }
    }
}
