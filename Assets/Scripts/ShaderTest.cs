using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderTest : MonoBehaviour
{
    //[SerializeField] Material[] materials;
    [SerializeField] GameObject insidePortal;
    [SerializeField] GameObject behindPortal;

    void Start()
    {
        //SetMaterialsToStencil(true);
    }

    /*
    private void SetMaterialsToStencil(bool isStencil)
    {
        if (isStencil)
        {
            foreach (Material material in materials)
            {
                material.SetInt("_Stencil", 1);
                material.SetInt("_StencilReference", 1);
                material.SetInt("_StencilComparison", 3);
                material.SetInt("_StencilOperation", 0);
            }
        }
        else
        {
            foreach (Material material in materials)
            {
                material.SetInt("_Stencil", 0);
                material.SetInt("_StencilReference", 1);
                material.SetInt("_StencilComparison", 3);
                material.SetInt("_StencilOperation", 0);
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.gameObject.tag == "MainCamera")
        {
            Debug.Log("It's Camera");
            
            if (!behindPortal.activeSelf)
            {
                insidePortal.SetActive(false);
                behindPortal.SetActive(true);
            }
            else
            {
                insidePortal.SetActive(true);
                behindPortal.SetActive(false);
            }
            //SetMaterialsToStencil(false);
        }
    }

}
