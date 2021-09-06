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
    GravityCube gravityCubeTobeAttracted;
    Animator animator;
    Collider cube_COLLIDER;
    Collider cube_AreaToFindOtherCubes;
    bool cubeFrozen;
    [SerializeField]
    Transform cubeParent;

    [SerializeField]
    Transform gravityFieldTransform;

    [SerializeField]
    Transform positionToSnapOtherCube;

   

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
    public bool IsDefault { get => isDefault; set => isDefault = value; }
    public bool IsRepelling { get => isRepelling; set => isRepelling = value; }
    public bool IsAttracting { get => isAttracting; set => isAttracting = value; }
    public Rigidbody RbToAttract { get => rbToAttract; set => rbToAttract = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }
    public bool CubeFrozen { get => cubeFrozen; set => cubeFrozen = value; }
    public GravityCube GravityCubeTobeAttracted { get => gravityCubeTobeAttracted; set => gravityCubeTobeAttracted = value; }

    void Start()
    {
        IsAttracting = false;
        IsRepelling = false;
        IsDefault = true;
        direction = Vector3.zero;
        distance = 0;
        forceMagnitude = 0;
        force = Vector3.zero;
        gravityFieldTransform.GetComponent<InstanceMaterial>().CreateMatInstance();
        cubeInstanceMat_SCRIPT = GetComponent<InstanceMaterial>();
        Rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cube_COLLIDER = GetComponent<Collider>();
        cubeInstanceMat_SCRIPT.CreateMatInstance();
        
    }

    private void FixedUpdate()
    {
        if((IsAttracting || IsRepelling) && RbToAttract != null)
        {
            Attract_Repel();
        }
    }

    public void SetCubeDefaultBehaviour()
    {
        IsAttracting = false;
        IsRepelling = false;
        IsDefault = false;
        animator.SetBool("isAttracting",false);
        animator.SetBool("isRepelling", false);
        animator.SetBool("isDefault", true);
    }

    public void SetCubeGravityAttractbehaviour()
    {
        //if isAttracting is already true, the cube goes to default mode
        if (IsAttracting)
        {
            SetCubeDefaultBehaviour();
            return;
        }
        IsAttracting = true;
        IsRepelling = false;
        IsDefault = false;
        animator.SetBool("isAttracting", true);
        animator.SetBool("isRepelling", false);
        animator.SetBool("isDefault", false);
    }

    public void SetCubeGravityRepelbehaviour()
    {
        if (IsRepelling)
        {
            SetCubeDefaultBehaviour();
            return;
        }
        IsRepelling = true;
        IsAttracting = false;
        IsDefault = true;
        animator.SetBool("isAttracting", false);
        animator.SetBool("isRepelling", true);
        animator.SetBool("isDefault", false);
    }


 


    private void Attract_Repel()
    {
        if (gravityCubeTobeAttracted != null && IsAttracting && gravityCubeTobeAttracted.isAttracting)
        {
            direction = Rb.position - RbToAttract.position;
            
        }
        else if(gravityCubeTobeAttracted != null && IsRepelling && (gravityCubeTobeAttracted.isRepelling || gravityCubeTobeAttracted.isAttracting))
        {
            direction = RbToAttract.position - Rb.position;
            
        }
        else 
        { 
            return; 
        }
        
        distance = direction.magnitude;
        


        forceMagnitude = (Rb.mass * RbToAttract.mass) / Mathf.Pow(distance, 2);
        force = direction.normalized * forceMagnitude;
        
        RbToAttract.AddForce(force);
    }


    public void SetVariablesForOptimalPosition(Vector3 newPos)
    {
        
        //rb.isKinematic = true;
        cube_AreaToFindOtherCubes.enabled = false;
        CubeFrozen = true;
        transform.position = newPos;
}

    public Transform Grab()
    {
        SetCubeDefaultBehaviour();
        CubeFrozen = false;
        Rb.isKinematic = true;
        cube_COLLIDER.enabled = false;
        transform.rotation = Quaternion.identity;
        return transform;
    }

    public void Drop()
    {
        Rb.isKinematic = false;
        cube_COLLIDER.enabled = true;
        CubeFrozen = false;
        //return cubeParent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LEVER"))
        {
            other.GetComponent<TriggerCube>().InteractAction();
        }
    }
}
