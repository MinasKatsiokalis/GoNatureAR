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

    [SerializeField] TextAnim textAnimator;

    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource companionAudioSource;
    [SerializeField] AudioClip companionAudioClip;
    [SerializeField] AudioClip companionAudioClip2;
    [SerializeField] AudioClip companionAudioClip3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeScene());
    }
    IEnumerator InitializeScene()
    {
        yield return new WaitForSeconds(3);
        companionAudioSource.PlayOneShot(companionAudioClip);
        textAnimator.AnimateText("Everything seems to be fine, don't yopu agree?");
    }

    public void AiPollution()
    {
        StartCoroutine(CoAirPollution());
    }

    IEnumerator CoAirPollution()
    {
        musicAudioSource.Play();

        companionAudioSource.PlayOneShot(companionAudioClip2);
        textAnimator.AnimateText("At first glance everytinh seems quite normal...\n" +
            "but if we look deeper, we shall witness the true nature of the situation.\n" +
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

        while(companionAudioSource.isPlaying)
            yield return null;

        yield return new WaitForSeconds(2);
        companionAudioSource.PlayOneShot(companionAudioClip3);
        textAnimator.AnimateText("Put your palm face up to view info panel!");
    }


}
