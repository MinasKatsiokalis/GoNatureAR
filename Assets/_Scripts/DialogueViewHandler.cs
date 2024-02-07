using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GoNatureAR
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueViewHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textMeshPro;
        [SerializeField]
        private float characterDelay;
        
        private AudioSource audioSource;
        private Coroutine currentCoroutine;

        public bool palmUp = false;
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
            if ((dialogue.DialogueKey.Keyword == Keyword.Julie) && !palmUp)
                NarrationManager.Instance.GetPalmNotUpDialogue();
            else
            {
                TypeText(dialogue.Text);
                Debug.Log(dialogue.AudioClip.name);
                audioSource.PlayOneShot(dialogue.AudioClip);
            }
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
            currentCoroutine = StartCoroutine(TypeWriterEffect(textToType, characterDelay));
        }

        private IEnumerator TypeWriterEffect(string textToType, float delay)
        {
            textMeshPro.text = textToType;
            int maxVisibleCharacters = 0;

            for (int i = 0; i < textToType.Length; i++)
            {
                maxVisibleCharacters++;
                textMeshPro.maxVisibleCharacters = maxVisibleCharacters;
                yield return new WaitForSeconds(delay);
            }
        }
    }

}