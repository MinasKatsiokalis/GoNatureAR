using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR
{
    public class PanelSpawnOnCamera : MonoBehaviour
    {
        [SerializeField]
        float _distanceFromCamera = 1.5f;
        private void OnEnable()
        {
            var mainCamera = Camera.main;
            this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * _distanceFromCamera;
            this.transform.position = new Vector3(this.transform.position.x, mainCamera.transform.position.y, this.transform.position.z);
        }
    }
}
