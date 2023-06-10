using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

[RequireComponent(typeof(SolverHandler))]
[RequireComponent(typeof(HandConstraintPalmUp))]
[RequireComponent(typeof(SpeechInputHandler))]
public class SpeechToMove : MonoBehaviour
{
    public static SpeechToMove Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    SolverHandler handTracker;
    HandConstraintPalmUp constraintPalmUp;
    SpeechInputHandler speechInputHandler;


    public ButtonConfigHelper interactableButton;
    public TMP_Text interactableButtonText;
    public Interactable interactable;

    [SerializeField] TextAnim textAnimator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    [SerializeField] float travelSpeed = 0.5f;
    [SerializeField] float distanceThreshhold =0.1f;

    private bool isPalmUp = false;
    private Vector3 handPosition;

    private State currentState;

    // Start is called before the first frame update
    void Start()
    {
        interactable.IsEnabled = false;
        handTracker = GetComponent<SolverHandler>();
        SetHandTrackerProperties();

        constraintPalmUp = GetComponent<HandConstraintPalmUp>();
        constraintPalmUp.OnHandActivate.AddListener(() => SetPalmUp(true));
        constraintPalmUp.OnHandDeactivate.AddListener(() => SetPalmUp(false));
        constraintPalmUp.UpdateLinkedTransform = true;

        speechInputHandler = GetComponent<SpeechInputHandler>();
        speechInputHandler.AddResponse("Julie", () => StartToMove());
        speechInputHandler.AddResponse("Let's go", () => StartVisualization());
        speechInputHandler.AddResponse("Yes", () => ResponseToYes());
        speechInputHandler.AddResponse("Continue", () => ResponseToContinue());

        StartCoroutine(InitializeScene());
    }

    public void ResponseToYes()
    {
        interactable.IsEnabled = false;

        if (currentState == State.AirQuality)
            InitializeManager.Instance.AiPollution();
        else if (currentState == State.Noise)
            SetNoiseEffects.Instance.NoiseTransition();
    }

    public void ResponseToContinue()
    {
        interactable.IsEnabled = false;

        InitializeManager.Instance.StopAiPollution();

        currentState = State.Noise;
        SetNoiseEffects.Instance.InitNoiseScene();
    }

    IEnumerator InitializeScene()
    {
        currentState = State.Introduction;
        yield return new WaitForSeconds(3);

        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[0]);
        textAnimator.AnimateText("Hey there!\n" +
            "I am Julie the Pigeon, and I am your companion! Together we will have a great experience!\n" +
            "Turn your palm facing up and call my name, and I will come to you as fast as I can!\n" +
            "Let's try it out!");

        interactableButton.OnClick.RemoveAllListeners();
        interactableButton.OnClick.AddListener(StartToMove);
        interactableButtonText.text = "Julie!";
        interactable.IsEnabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        handPosition = handTracker.TransformTarget.position;
    }

    public void StartToMove()
    {
        if (!isPalmUp)
        {
            Debug.Log("Your palm is not placed correctly!");
            audioSource.Stop();
            audioSource.PlayOneShot(audioClips[2]);
            textAnimator.AnimateText("Please, keep your palm facing up!");
            julieCalled = false;
            return;
        }

        constraintPalmUp.UpdateLinkedTransform = false;
        StartCoroutine(CoStartToMove());
    }

    private bool julieCalled = false;
    private IEnumerator CoStartToMove()
    {
        Debug.Log("Start Moving...");
        while (!(Vector3.Distance(transform.position, handPosition) <= distanceThreshhold))
        {
            transform.position = Vector3.Lerp(transform.position, handPosition, travelSpeed * Time.deltaTime);
            yield return null;
        }
        constraintPalmUp.UpdateLinkedTransform = true;
        Debug.Log("Stop Moving");
        if(!julieCalled && (currentState == State.Introduction))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audioClips[1]);
            textAnimator.AnimateText("Nice!\nTogether we shall take a journey into the world of GoNature AR!\n" +
                "When you are ready say: <b>\"Let's Go\"</b>");

            interactableButton.OnClick.RemoveAllListeners();
            interactableButton.OnClick.AddListener(StartVisualization);
            interactableButtonText.text = "Let's Go!";
            interactable.IsEnabled = true;

        }
        julieCalled = true;

    }

    private void StartVisualization()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[3]);
        textAnimator.AnimateText("Alright!");
        interactable.IsEnabled = false;

        currentState = State.AirQuality;
        InitializeManager.Instance.Init();
    }

    private void SetPalmUp(bool palmUp)
    {
        isPalmUp = palmUp;
    }

    private void SetHandTrackerProperties()
    {
        handTracker.TrackedTargetType = TrackedObjectType.HandJoint;
        handTracker.TrackedHandedness = Handedness.Both;
        handTracker.TrackedHandJoint = TrackedHandJoint.Palm;
        handTracker.UpdateSolvers = true;
    }
}

public enum State
{
    Introduction,
    AirQuality,
    Noise
}
