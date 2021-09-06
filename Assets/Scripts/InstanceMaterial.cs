using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    private Material instanceMat;

    public void CreateMatInstance()
    {
        instanceMat = GetComponent<MeshRenderer>().material;
    }
}
