using UnityEngine;
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
        private GameObject _fogVFX;
        [SerializeField]
        private AudioClip _goodAirQualityAmbientMusic;
        [SerializeField]
        private AudioClip _unhealthyAirQualityAmbientMusic;

        private Camera mainCamera;
        private AudioSource audioSource;
        private float[] strengths;

        private void Awake()
        {
            mainCamera = Camera.main;
            audioSource = GetComponent<AudioSource>();

            strengths = new float[_airQualityVFX.Length];

            for (int i = 0; i < _airQualityVFX.Length; i++)
                strengths[i] = _airQualityVFX[i].GetFloat("Strength");
        }

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
            AttachToCamera();
        }

        // Update is called once per frame
        void Update()
        {
            AttachToCamera();
        }

        private void AttachToCamera()
        {
            if (mainCamera != null)
            {
                this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2f;
            }
        }

        private void EnableAirEffects(AirIndex index)
        {
            DisableVFX();

            if (index == AirIndex.Good)
            {
                MusicManager.Instance.PlayMusic(_goodAirQualityAmbientMusic);
                for (int i = 0; i < _airQualityVFX.Length; i++)
                    _airQualityVFX[i].SetFloat("Strength", strengths[i]/8f);
            }
            else
            {
                MusicManager.Instance.PlayMusic(_unhealthyAirQualityAmbientMusic);
                _fogVFX.SetActive(true);
                for (int i = 0; i < _airQualityVFX.Length; i++)
                    _airQualityVFX[i].SetFloat("Strength", strengths[i]);
            }

            if (DataManager.Instance.AirData != null)
            {
                foreach (var item in DataManager.Instance.AirData)
                {
                    if (item.Value == 0)
                        continue;
                    
                    foreach (var vfx in _airQualityVFX)
                    {
                        if (item.Key.ToString() == vfx.gameObject.name)
                            vfx.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (var vfx in _airQualityVFX)
                {
                    if (AirQualityManager.AQIElements.Contains(vfx.gameObject.name))
                        vfx.gameObject.SetActive(true);
                }
            }
        }

        private void DisableVFX()
        {
            _fogVFX.SetActive(false);

            foreach (var vfx in _airQualityVFX)
                vfx.gameObject.SetActive(false);
        }

    }
}
