using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using GoNatureAR.Requests;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using System;

namespace GoNatureAR
{
    public class PilotCanvasViewHandler : MonoBehaviour
    {   
        // Buttons for the pilot selection
        [Header("Pilot Buttons")]
        [SerializeField]
        private Interactable _chaniaButton;
        [SerializeField]
        private Interactable _leuvenButton;
        [SerializeField]
        private Interactable _castelfrancoButton;
        [SerializeField]
        private Interactable _dundalkButton;
        [SerializeField]
        private Interactable _novomestoButton;
        [SerializeField]
        private Interactable _gziraButton;
        [SerializeField]
        private Interactable _skellefteaButton;

        [Header("Error Panel Object")]
        [SerializeField]
        private GameObject _errorPanel;

        private Task runningTask;
        private Timer timer;
        private bool isRequestable;

        private void OnEnable()
        {
            var mainCamera = Camera.main;
            this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 1.5f;

            PilotDataRequestManager.OnError += EnableErrorPanel;
        }

        private void OnDisable()
        {
            PilotDataRequestManager.OnError -= EnableErrorPanel;
        }

        // Start is called before the first frame update
        void Start()
        {
            isRequestable = true;
            timer = new Timer(5000f);
            timer.Elapsed += OnThresholdPassed;

            _chaniaButton.OnClick.AddListener(() => PilotDataRequest(Pilot.chania));
            _leuvenButton.OnClick.AddListener(() => PilotDataRequest(Pilot.leuven));
            _castelfrancoButton.OnClick.AddListener(() => PilotDataRequest(Pilot.castelfranco_veneto));
            _dundalkButton.OnClick.AddListener(() => PilotDataRequest(Pilot.dundalk));
            _novomestoButton.OnClick.AddListener(() => PilotDataRequest(Pilot.novo_mesto));
            _gziraButton.OnClick.AddListener(() => PilotDataRequest(Pilot.gzira));
            _skellefteaButton.OnClick.AddListener(() => PilotDataRequest(Pilot.skelleftea));
        }

        /// <summary>
        /// This method is called when the pilot is selected.
        /// It sends requests for the data.
        /// If an existing task is running or no more than 5 seconds have passed from previous request, the request is not sent.
        /// </summary>
        /// <param name="pilot"></param>
        private async void PilotDataRequest(Pilot pilot)
        {
            if (runningTask?.Status != TaskStatus.Running && isRequestable)
            {   
                isRequestable = false;
                timer.Enabled = true;

                runningTask = PilotDataRequestManager.Instance.SendRequestsForData(pilot);
                await runningTask;
            }
        }

        /// <summary>
        /// This method is called when the threshold is passed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnThresholdPassed(object sender, ElapsedEventArgs e)
        {
            isRequestable = true;
            timer.Enabled = false;
        }

        /// <summary>
        /// This method is called when an error occurs.
        /// </summary>
        /// <param name="msg"></param>
        private void EnableErrorPanel(string msg)
        {
            _errorPanel.SetActive(true);
        }
    }
}
