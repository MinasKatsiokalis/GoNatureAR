using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GoNatureAR
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueViewHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _textMeshPro;
        [SerializeField]
        private float _characterDelay;

        private AudioSource audioSource;
        private Coroutine currentCoroutine;

        private bool isJulieCalled = false;
        private bool palmUp = false;

        private void OnEnable()
        {
            NarrationManager.OnDialogueTrigger += OnDialogueTriggerHandler;
            CompanionBehaviour.OnPalmUpDetected += OnPalmUpDetectedHandler;
        }

        private void OnDisable()
        {
            NarrationManager.OnDialogueTrigger -= OnDialogueTriggerHandler;
            CompanionBehaviour.OnPalmUpDetected -= OnPalmUpDetectedHandler;
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnDialogueTriggerHandler(DialogueScriptableObject dialogue)
        {   
            if(audioSource.isPlaying)
                audioSource.Stop();

            if ((dialogue.DialogueKey.Keyword == Keyword.Julie) && !palmUp)
            {
                NarrationManager.Instance.GetPalmNotUpDialogue();
                return;
            }
            else if((dialogue.DialogueKey.Keyword == Keyword.Julie) && palmUp)
            {
                if (isJulieCalled && GameManager.Instance.CurrentState != State.Introduction)
                    return;
                else
                    isJulieCalled = true;
            }

            TypeText(dialogue.Text);
            Debug.Log(dialogue.AudioClip.name);
            audioSource.PlayOneShot(dialogue.AudioClip);
        }

        private void OnPalmUpDetectedHandler(bool isPalmUp)
        {
            palmUp = isPalmUp;
        }

        public void TypeText(string textToType)
        {
            // Stop any ongoing typewriter effect
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            // Start the typewriter effect for the new text
            currentCoroutine = StartCoroutine(TypeWriterEffect(textToType, _characterDelay));
        }

        private IEnumerator TypeWriterEffect(string textToType, float delay)
        {
            _textMeshPro.text = textToType;
            int maxVisibleCharacters = 0;

            for (int i = 0; i < textToType.Length; i++)
            {
                maxVisibleCharacters++;
                _textMeshPro.maxVisibleCharacters = maxVisibleCharacters;
                yield return new WaitForSeconds(delay);
            }
        }
    }

}