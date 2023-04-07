using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SetCollisionPosition : MonoBehaviour
{
    [SerializeField] VisualEffect vfxFogRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vfxFogRenderer.SetVector3("ColliderPosition", transform.position);
    }
}
