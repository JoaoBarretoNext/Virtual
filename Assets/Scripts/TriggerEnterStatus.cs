using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterStatus : MonoBehaviour
{
    [SerializeField]
    GravityCube gravityCube_SCRIPT;
    private void OnTriggerEnter(Collider other)
    {
        if (!gravityCube_SCRIPT.CubeFrozen)
        {
            if (other.CompareTag("GRAVITY_CUBE"))
            {
                gravityCube_SCRIPT.RbToAttract = other.GetComponent<Rigidbody>();
                gravityCube_SCRIPT.GravityCubeTobeAttracted = other.GetComponent<GravityCube>();
                
                if (other.GetComponent<GravityCube>().IsDefault) // and o player largou o cubo
                {
                    
                    gravityCube_SCRIPT.SetVariablesForOptimalPosition(other.GetComponent<GravityCube>().PositionToSnapOtherCube.position);


                    //return;
                }
                //gravityCube_SCRIPT.RbToAttract = other.GetComponent<Rigidbody>();
            }
            if (other.CompareTag("PLATFORM"))
            {

                if (other.GetComponent<GravityCube>() != null && other.GetComponent<GravityCube>().IsDefault) // and o player largou o cubo
                {

                    gravityCube_SCRIPT.transform.position = other.transform.GetChild(0).position;
                    //return;
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
       
    }

}
