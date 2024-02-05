using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TextAnim : MonoBehaviour
{   
    [SerializeField]
    private TMP_Text textMeshPro;
    [SerializeField]
    private float characterDelay;
    private Coroutine currentCoroutine;

    public void TypeText(string textToType)
    {
        // Stop any ongoing typewriter effect
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
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
