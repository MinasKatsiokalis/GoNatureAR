using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GoNatureAR.Sensors;

namespace GoNatureAR
{
    public class ThermalEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject _coldEffect;
        [SerializeField]
        private GameObject _hotEffect;
        [SerializeField]
        private GameObject _sweatEffect;
        [SerializeField]
        private AudioClip _coldAmbientMusic;
        [SerializeField]
        private AudioClip _hotAmbientMusic;
        [SerializeField]
        private AudioClip _hotSoundFX;
        [SerializeField]
        private AudioClip _comfortableAmbientMusic;
        [SerializeField]
        GameObject _thermalDetailsBackground;

        private Camera mainCamera;
        private AudioSource audioSource;

        private void Awake()
        {
            mainCamera = Camera.main;
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            ThermalComfortManager.OnThermalComfortCalculated += EnableThermalEffects;
        }

        private void OnDisable()
        {
            ThermalComfortManager.OnThermalComfortCalculated -= EnableThermalEffects;
        }

        // Start is called before the first frame update
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
                this.transform.position = mainCamera.transform.position;
                this.transform.rotation = mainCamera.transform.rotation;
            }
        }

        private void EnableThermalEffects(ThermalComofortIndex index, double humidity)
        {
            var color = new ThermalColors();
            switch (index)
            {
                case ThermalComofortIndex.Cold:
                    MusicManager.Instance.PlayMusic(_coldAmbientMusic);
                    _thermalDetailsBackground.GetComponent<MeshRenderer>().material.SetColor("_InnerGlowColor", color.Cold);
                    _coldEffect.SetActive(true); 
                    _hotEffect.SetActive(false);
                    _sweatEffect.SetActive(false);
                    break;
                case ThermalComofortIndex.Hot:
                    MusicManager.Instance.PlayMusic(_hotAmbientMusic);
                    _thermalDetailsBackground.GetComponent<MeshRenderer>().material.SetColor("_InnerGlowColor", color.Hot);
                    audioSource.clip = _hotSoundFX;
                    audioSource.Play();
                    _hotEffect.SetActive(true);
                    _coldEffect.SetActive(false);
                    if (humidity >= 65)
                        _sweatEffect.SetActive(true);
                    break;
                default:
                    MusicManager.Instance.PlayMusic(_comfortableAmbientMusic);
                    _thermalDetailsBackground.GetComponent<MeshRenderer>().material.SetColor("_InnerGlowColor", color.Comfortable);
                    _coldEffect.SetActive(false);
                    _hotEffect.SetActive(false);
                    _sweatEffect.SetActive(false);
                    break;
            }
        }
    }
}
