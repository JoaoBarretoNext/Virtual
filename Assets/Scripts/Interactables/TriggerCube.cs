using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InstanceMaterial))]
public class TriggerCube : MonoBehaviour
{
    InstanceMaterial instanceMat_SCRIPT;
    Animator animator;
   

    // Start is called before the first frame update
    void Start()
    {
        instanceMat_SCRIPT = GetComponent<InstanceMaterial>();
        animator = GetComponent<Animator>();
        instanceMat_SCRIPT.CreateMatInstance();
    }

    //to ba called after cube touches this
    public void InteractAction()
    {
        animator.SetTrigger("CubeTouched");
    }
}
