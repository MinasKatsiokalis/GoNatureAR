using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.VFX;

namespace GoNatureAR
{
    public class AirQualityEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject _airQualityPrefab;
        [SerializeField]
        private VisualEffect[] _airQualityVFX;
        [SerializeField]
        private AudioClip _goodAirQualityAmbientMusic;
        [SerializeField]
        private AudioClip _unhealthyAirQualityAmbientMusic;

        private Camera mainCamera;
        private AudioSource audioSource;


        private void OnEnable()
        {
            AirQualityManager.OnAirQualityCalculated += EnableAirEffects;
        }

        private void OnDisable()
        {
            AirQualityManager.OnAirQualityCalculated -= EnableAirEffects;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void EnableAirEffects(AirIndex index)
        {

        }
    }
}
