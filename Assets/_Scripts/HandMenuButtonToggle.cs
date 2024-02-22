using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using System;

public class HandMenuButtonToggle : MonoBehaviour
{
    public static Action onInfoButtonClicked;

    private bool isButtonActive = false;
    private ButtonConfigHelper ButtonHelper;

    private void OnEnable()
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
        isButtonActive = !isButtonActive;
        this.gameObject.SetActive(isButtonActive);
    }
}
