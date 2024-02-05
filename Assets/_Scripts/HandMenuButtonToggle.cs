using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using System;

public class HandMenuButtonToggle : MonoBehaviour
{
    public static Action onInfoButtonClicked;
    
    [SerializeField]
    private GameObject targetObject;

    private bool isButtonActive = false;

    public ButtonConfigHelper ButtonHelper { get; set; }

    /// <summary>
    /// Toggle all other buttons except currently one
    /// </summary>
    private void ToggleOthersOff()
    {
        var allToggles = FindObjectsOfType<HandMenuButtonToggle>();
        foreach (HandMenuButtonToggle handMenuToggle in allToggles)
        {
            if (handMenuToggle.ButtonHelper != ButtonHelper)
            {
                handMenuToggle.targetObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        GetBindingsForButton();
    }

    private void GetBindingsForButton()
    {
        ButtonHelper = GetComponent<ButtonConfigHelper>();
        ButtonHelper.OnClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        ButtonHelper.OnClick.RemoveListener(Toggle);
    }

    public void Toggle()
    {
        if (targetObject != null)
        {
            isButtonActive = !isButtonActive;
            targetObject.SetActive(isButtonActive);

            if (isButtonActive && targetObject.name == "SceneDescriptionPanelRev")
                onInfoButtonClicked?.Invoke();
        }
    }
}
