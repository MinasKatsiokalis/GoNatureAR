using System.Collections;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

namespace GoNatureAR
{
    [RequireComponent(typeof(SolverHandler))]
    [RequireComponent(typeof(HandConstraintPalmUp))]
    public class CompanionBehaviour : MonoBehaviour
    {
        public static Action<bool> OnPalmUpDetected;

        private SolverHandler handTracker;
        private HandConstraintPalmUp constraintPalmUp;
        private bool isPalmUp = false;
        private Vector3 handPosition;

        [SerializeField] 
        private float travelSpeed = 0.5f;
        [SerializeField] 
        private float distanceThreshhold = 0.1f;

        private void OnEnable()
        {
            NarrationManager.OnDialogueTrigger += OnDialogueTriggerHandler;
        }

        private void OnDisable()
        {
            NarrationManager.OnDialogueTrigger -= OnDialogueTriggerHandler;
        }

        // Start is called before the first frame update
        void Start()
        {
            handTracker = GetComponent<SolverHandler>();
            SetHandTrackerProperties();

            constraintPalmUp = GetComponent<HandConstraintPalmUp>();
            constraintPalmUp.OnHandActivate.AddListener(() => SetPalmUp(true));
            constraintPalmUp.OnHandDeactivate.AddListener(() => SetPalmUp(false));
            constraintPalmUp.UpdateLinkedTransform = true;
        }

        // Update is called once per frame
        void Update()
        {
            handPosition = handTracker.TransformTarget.position;
        }

        void OnDialogueTriggerHandler(DialogueScriptableObject dialogue)
        {
            if (dialogue.DialogueKey.Keyword != Keyword.Julie)
                return;
            else if (isPalmUp)
                StartToMove();
        }

        public void StartToMove()
        {
            constraintPalmUp.UpdateLinkedTransform = false;
            StartCoroutine(CoStartToMove());
        }

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

        }

        private void SetPalmUp(bool palmUp)
        {
            isPalmUp = palmUp;
            OnPalmUpDetected?.Invoke(palmUp);
        }

        private void SetHandTrackerProperties()
        {
            handTracker.TrackedTargetType = TrackedObjectType.HandJoint;
            handTracker.TrackedHandedness = Handedness.Both;
            handTracker.TrackedHandJoint = TrackedHandJoint.Palm;
            handTracker.UpdateSolvers = true;
        }
    }
}