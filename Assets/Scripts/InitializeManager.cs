using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeManager : MonoBehaviour
{
    [SerializeField] GameObject fogParticles;
    [SerializeField] GameObject co2;
    [SerializeField] GameObject o3;
    [SerializeField] GameObject no2;
    [SerializeField] GameObject pm2_5;
    [SerializeField] GameObject pm10;

    [SerializeField] AudioSource audioSource;
    [SerializeField] TextAnim textAnimator;
    [SerializeField] AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeScene());
    }

    IEnumerator InitializeScene()
    {
        yield return new WaitForSeconds(1);
        audioSource.Play();
        textAnimator.AnimateText("At first glance everytinh seems quite normal...\n" +
            "but if we look deeper, we shall witness the truw nature of the situation.\n" +
            "Hundreds of thousands of pollutant particles and gases surround us on each step of our lives,\n" +
            " affecting our health and well-being");

        yield return new WaitForSeconds(2);
        fogParticles.SetActive(true);

        yield return new WaitForSeconds(2);
        co2.SetActive(true);
        yield return new WaitForSeconds(2);
        o3.SetActive(true);
        yield return new WaitForSeconds(2);
        no2.SetActive(true);
        yield return new WaitForSeconds(2);
        pm2_5.SetActive(true);
        yield return new WaitForSeconds(2);
        pm10.SetActive(true);

        while(audioSource.isPlaying)
            yield return null;

        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(audioClip);
        textAnimator.AnimateText("Put your palm face up to view info panel!");
    }


}
