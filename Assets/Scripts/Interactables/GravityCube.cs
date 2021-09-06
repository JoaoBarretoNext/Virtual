using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InstanceMaterial))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class GravityCube : MonoBehaviour, IGrabble, IDroppable
{
    InstanceMaterial cubeInstanceMat_SCRIPT;
    Rigidbody rb;
    Rigidbody rbToAttract;
    Animator animator;
    Collider cube_COLLIDER;

    [SerializeField]
    Transform gravityFieldTransform;

    [SerializeField]
    Transform positionToSnapOtherCube;

    int instanceID;

    //variables for attracting/repelling
    Vector3 direction;
    float distance;
    float forceMagnitude;
    Vector3 force;
    [SerializeField]
    bool isAttracting;
    [SerializeField]
    bool isRepelling;
    [SerializeField]
    bool isDefault;

    public Transform PositionToSnapOtherCube { get => positionToSnapOtherCube; set => positionToSnapOtherCube = value; }
    public int InstanceID { get => instanceID; set => instanceID = value; }

    void Start()
    {
        isAttracting = false;
        isRepelling = false;
        direction = Vector3.zero;
        distance = 0;
        forceMagnitude = 0;
        force = Vector3.zero;
        gravityFieldTransform.GetComponent<InstanceMaterial>().CreateMatInstance();
        cubeInstanceMat_SCRIPT = GetComponent<InstanceMaterial>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cube_COLLIDER = GetComponent<Collider>();
        cubeInstanceMat_SCRIPT.CreateMatInstance();
        InstanceID = GetInstanceID();
    }

    private void FixedUpdate()
    {
        if((isAttracting || isRepelling) && rbToAttract != null)
        {
            Attract_Repel();
        }
    }

    public void SetCubeDefaultBehaviour()
    {
        isAttracting = false;
        isRepelling = false;
        isDefault = false;
        animator.SetBool("isAttracting",false);
        animator.SetBool("isRepelling", false);
        animator.SetBool("isDefault", true);
    }

    public void SetCubeGravityAttractbehaviour()
    {
        //if isAttracting is already true, the cube goes to default mode
        if (isAttracting)
        {
            SetCubeDefaultBehaviour();
            return;
        }
        isAttracting = true;
        isRepelling = false;
        isDefault = false;
        animator.SetBool("isAttracting", true);
        animator.SetBool("isRepelling", false);
        animator.SetBool("isDefault", false);
    }

    public void SetCubeGravityRepelbehaviour()
    {
        if (isRepelling)
        {
            SetCubeDefaultBehaviour();
            return;
        }
        isRepelling = true;
        isAttracting = false;
        isDefault = true;
        animator.SetBool("isAttracting", false);
        animator.SetBool("isRepelling", true);
        animator.SetBool("isDefault", false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GRAVITY_CUBE"))
        {
            if (other.GetComponent<GravityCube>().isDefault) // and o player largou o cubo
            {
               transform.position = other.GetComponent<GravityCube>().PositionToSnapOtherCube.position;
                return;
            }
            rbToAttract = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rbToAttract = null;
    }


    private void Attract_Repel()
    {
        if (isAttracting)
        {
            direction = rb.position - rbToAttract.position;
        }
        else if(isRepelling)
        {
            direction = rbToAttract.position - rb.position;
        }
        
        distance = direction.magnitude;

        
        forceMagnitude = (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    public Transform Grab()
    {
        rb.isKinematic = true;
        cube_COLLIDER.enabled = false;
        return transform;
    }

    public Transform Drop()
    {
        rb.isKinematic = false;
        cube_COLLIDER.enabled = true;
        return transform;
    }
}
