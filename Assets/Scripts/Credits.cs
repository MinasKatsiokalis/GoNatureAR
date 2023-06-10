using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public static Credits Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    [SerializeField] GameObject _sphere;
    [SerializeField] GameObject _content;
    [SerializeField] GameObject _creditsPanel;
    [SerializeField] float _speed;

    float fadeStart = 0;
    float fadeTime = 1f;
    Color objectColor = new Color(0.5382634f, 0.3237806f, 0.6415094f, 0f);
    Color fadeColor = new Color(0.5382634f, 0.3237806f, 0.6415094f, 1f);

    public void EnableCredits()
    {
        _sphere.SetActive(true);
        _content.SetActive(false);
        _creditsPanel.SetActive(true);

        //StartCoroutine(FadeIn());
        StartCoroutine(MoveCredits());
    }
    IEnumerator FadeIn()
    {
        while (fadeStart < fadeTime)
        {
            fadeStart += Time.deltaTime * fadeTime;
            _sphere.GetComponent<Renderer>().material.color = Color.Lerp(objectColor, fadeColor, fadeStart);
            yield return null;
        }
    }
    IEnumerator MoveCredits()
    {
        yield return new WaitForSeconds(4);

        while (_creditsPanel.transform.position.y <= 5.0f)
        {
            _creditsPanel.transform.Translate(Vector3.up * Time.deltaTime * _speed);
            yield return null;
        }

        Application.Quit();
    }
}
