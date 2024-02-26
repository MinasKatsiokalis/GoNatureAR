using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace GoNatureAR
{
    public class Credits : MonoBehaviour
    {   
        
        [SerializeField] GameObject _sphere;
        [SerializeField] GameObject _content;
        [SerializeField] GameObject _creditsPanel;
        [SerializeField] float _speed;

        private Camera mainCamera;
        private GameObject companionContainer;
        private AudioSource narrationAudioSource;

        private void OnEnable()
        {
            OutroManager.OnCreditsEnabled += EnableCredits;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            companionContainer = GameObject.Find("CompanionContainer");
            narrationAudioSource = companionContainer.transform.GetChild(0).GetComponent<AudioSource>();

            AttachToCamera();
        }

        private void Update()
        {
            AttachToCamera();
        }

        public void EnableCredits()
        {
            StartCoroutine(EnableCreditsCo());
        }

        IEnumerator EnableCreditsCo()
        {
            while (narrationAudioSource.isPlaying)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(3);

            _sphere.SetActive(true);
            _sphere.GetComponent<Renderer>().material.DOFade(1, 1);
            _content.SetActive(false);
            _creditsPanel.SetActive(true);
            companionContainer.SetActive(false);

            yield return new WaitForSeconds(3);

            while (_creditsPanel.transform.position.y <= 2.0f)
            {
                _creditsPanel.transform.Translate(Vector3.up * Time.deltaTime * _speed);
                yield return null;
            }

            OutroManager.OnOutroEnded?.Invoke();
        }

        private void AttachToCamera()
        {
            if (mainCamera != null)
            {
                _sphere.transform.position = mainCamera.transform.position;
                _sphere.transform.rotation = mainCamera.transform.rotation;
            }
        }
    }
}