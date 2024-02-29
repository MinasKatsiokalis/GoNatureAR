using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoNatureAR
{
    public class MenuBehaviour : MonoBehaviour
    {
        [SerializeField]
        Interactable _startButton;

        [SerializeField]
        GameObject _tutorialVideo;
        [SerializeField]
        GameObject _musicSource;

        private SpeechInputHandler speechInputHandler;
        // Start is called before the first frame update
        void Start()
        {
            _startButton.OnClick.AddListener(() => SceneManager.LoadScene("MainScene"));

            speechInputHandler = GetComponent<SpeechInputHandler>();
            speechInputHandler.AddResponse("Start", () => SceneManager.LoadScene("MainScene"));
            speechInputHandler.AddResponse("Tutorial", OpenTutorialVideo);
        }

        private void OpenTutorialVideo()
        {
            this.gameObject.SetActive(false);
            _musicSource.SetActive(false);
            _tutorialVideo.SetActive(true);
        }
    }
}
