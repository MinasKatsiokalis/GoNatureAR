using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeManager : MonoBehaviour
{
    public static InitializeManager Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        HandMenuButtonToggle.onInfoButtonClicked += () => StartCoroutine(CoPressedButton());
    }

    private void OnDisable()
    {
        HandMenuButtonToggle.onInfoButtonClicked -= () => StartCoroutine(CoPressedButton());
    }

    [SerializeField] GameObject handMenu;
    [SerializeField] GameObject infoPanel;

    [SerializeField] GameObject fogParticles;
    [SerializeField] GameObject co2;
    [SerializeField] GameObject o3;
    [SerializeField] GameObject no2;
    [SerializeField] GameObject pm2_5;
    [SerializeField] GameObject pm10;

    [SerializeField] TextAnim textAnimator;

    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip musicAudioClip;

    [SerializeField] AudioSource companionAudioSource;
    [SerializeField] AudioClip companionAudioClip;
    [SerializeField] AudioClip companionAudioClip2;
    [SerializeField] AudioClip companionAudioClip3;
    [SerializeField] AudioClip companionAudioClip4;
    [SerializeField] AudioClip companionAudioClip5;



    // Start is called before the first frame update
    public void Init()
    {
        handMenu.SetActive(true);
        StartCoroutine(InitializeScene());
    }
    IEnumerator InitializeScene()
    {
        yield return new WaitForSeconds(3);
        companionAudioSource.Stop();
        companionAudioSource.PlayOneShot(companionAudioClip);
        textAnimator.AnimateText("Now that I introduced myself...\n" +
            "I should tell you that I have the ability to sense the air quality of the environment!\n"+
            "Do you wanna see the air through me eyes?");

        SpeechToMove.Instance.interactableButton.OnClick.RemoveAllListeners();
        SpeechToMove.Instance.interactableButton.OnClick.AddListener(SpeechToMove.Instance.ResponseToYes);
        SpeechToMove.Instance.interactableButtonText.text = "Yes";
        SpeechToMove.Instance.interactable.IsEnabled = true;

    }

    public void AiPollution()
    {
        StartCoroutine(CoAirPollution());
    }

    IEnumerator CoAirPollution()
    {
        musicAudioSource.Stop();
        musicAudioSource.PlayOneShot(musicAudioClip);

        companionAudioSource.Stop();
        companionAudioSource.PlayOneShot(companionAudioClip2);
        textAnimator.AnimateText("At first glance everyting seems quite normal...\n" +
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
        textAnimator.AnimateText("Put your left palm face up to view info panel!");
    }
    public void StopAiPollution()
    {
        handMenu.SetActive(false);
        infoPanel.SetActive(false);
        SetParticlesStrength.Instance.Disable();
    }

    IEnumerator CoPressedButton()
    {
        companionAudioSource.Stop();
        companionAudioSource.PlayOneShot(companionAudioClip4);
        textAnimator.AnimateText("These pollutants are related with:\n" +
            "1. Respiratory Conditions\n" +
            "2. Cardiovascular Diseases\n"+
            "3. Pregnancy Outcomes and\n" +
            "4. Premature Death.");

        yield return new WaitForSeconds(15);


        companionAudioSource.PlayOneShot(companionAudioClip5);
        textAnimator.AnimateText("I haven't show you yet all my abilities...\n" +
            "My hearing is quite sensitive too. I want to show you, how your environment feels like to me.\n" +
            "When you are ready to proceed, please say <b>\"Continue\"</b>");

        SpeechToMove.Instance.interactableButton.OnClick.RemoveAllListeners();
        SpeechToMove.Instance.interactableButton.OnClick.AddListener(SpeechToMove.Instance.ResponseToContinue);
        SpeechToMove.Instance.interactableButtonText.text = "Continue";
        SpeechToMove.Instance.interactable.IsEnabled = true;

    }
}
