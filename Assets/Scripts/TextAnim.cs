using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TextAnim : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;

    [TextArea(1, 10)] public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;

    public void AnimateText(string text)
    {
        StartCoroutine(CoAnimateText(text));
    }

    private IEnumerator CoAnimateText(string text)
    {
        //for(int i=0; i <= stringArray.Length - 1; i++)
        //{
        textMeshPro.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textMeshPro.rectTransform.parent.GetComponent<RectTransform>());

        yield return StartCoroutine(TextVisible());
        //}
    }

    private int visibleCount = 0;
    private int counter = 0;
    private IEnumerator TextVisible()
    {
        textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = textMeshPro.textInfo.characterCount;

        while (visibleCount < totalVisibleCharacters)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);
            textMeshPro.maxVisibleCharacters = visibleCount;

            /*
            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
                Invoke("EndCheck", timeBtwnWords);
                break;
            }*/

            counter ++;
            yield return new WaitForSeconds(timeBtwnChars);
        }
    }
}
