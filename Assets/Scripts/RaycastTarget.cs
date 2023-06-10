using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTarget : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 5f; // Maximum distance for the raycast
    [SerializeField] private GameObject buttonText;
    [SerializeField] private GameObject buttonBack;
    [SerializeField] private GameObject buttonFront;

    void Update()
    {
        buttonText.SetActive(false);
        buttonBack.SetActive(false);
        buttonFront.SetActive(false);

        // Get the center of the camera's viewport
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

        // Cast a ray from the center of the camera
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, raycastDistance))
        {
            // Check if the hit object has the tag "Julie"
            if (hit.collider.CompareTag("Companion"))
            {
                // Object with tag "Julie" was hit
                Debug.Log("Julie was hit!");

                buttonText.SetActive(true);
                buttonBack.SetActive(true);
                buttonFront.SetActive(true);
            }
        }
    }
}
