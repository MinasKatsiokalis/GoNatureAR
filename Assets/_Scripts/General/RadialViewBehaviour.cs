using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

namespace GoNatureAR
{
    public class RadialViewBehaviour : MonoBehaviour
    {
        private RadialView radialView;
        private Camera mainCamera;

        IEnumerator coroutine;
        private bool isNear = false;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
            radialView = GetComponent<RadialView>();

            coroutine = AdjustRadialView();
            StartCoroutine(coroutine);
        }

        private void OnDisable()
        {
            StopCoroutine(coroutine);
        }

        private IEnumerator AdjustRadialView()
        {   
            while (true)
            {
                var distance = Mathf.Abs(Vector3.Distance(mainCamera.transform.position, this.transform.position));
                if (distance <= 1f && !isNear)
                {
                    radialView.UpdateLinkedTransform = true;
                    isNear = true;
                }
                else if(distance > 1f && isNear)
                {
                    radialView.UpdateLinkedTransform = false;
                    isNear = false;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

    }
}
