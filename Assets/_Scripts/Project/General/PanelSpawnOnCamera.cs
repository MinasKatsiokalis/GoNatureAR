using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR
{
    public class PanelSpawnOnCamera : MonoBehaviour
    {
        private void OnEnable()
        {
            var mainCamera = Camera.main;
            this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2f;
        }
    }
}
