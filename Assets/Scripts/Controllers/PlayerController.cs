using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Camera _camera;

    //raycasting
    Ray ray;
    RaycastHit hit;
    RaycastHit[] hits;
    float closestsObjectDistance;


    bool foundCube;
    Transform objectToGrab;
    [SerializeField]
    Transform placeToPostitionGrabbedObject;


    //booleans
    bool canGrab;
    void Start()
    {
        canGrab = true;
        foundCube = false; ;
        closestsObjectDistance = 0.0f;
        hit = new RaycastHit();
    }

    // Update is called once per frame
    void Update()
    {
        InteractWithWorld();
    }

    private void InteractWithWorld()
    {
        ray = GetMouseRay();

        
        //if clicks left mouse, turns cube blueish(attract)
        if (Input.GetMouseButtonDown(0))
        {
            foundCube = false;
            hits = Physics.RaycastAll(ray.origin, ray.direction, 1000f);

            closestsObjectDistance = hits[0].distance;
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(hits[i].collider.name);
                if (hits[i].collider.CompareTag("GRAVITY_CUBE"))
                {
                    Debug.Log("Entrei aqui no collider");
                    if(hits[i].distance < closestsObjectDistance)
                    {
                        closestsObjectDistance = hits[i].distance;
                        hit = hits[i];
                        Debug.Log("A distancia é: " + hits[i].distance);
                        foundCube = true;
                    }
                }
                if (i == hits.Length - 1 && foundCube)
                {
                    hit.transform.GetComponent<GravityCube>().SetCubeGravityAttractbehaviour();
                }
            }
            
            


        }
        //if clicks right mouse, turns cube reddish(repels)
        else if (Input.GetMouseButtonDown(1))
        {
            foundCube = false;
            hits = Physics.RaycastAll(ray.origin, ray.direction, 1000f);

            closestsObjectDistance = hits[0].distance;
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(hits[i].collider.name);
                if (hits[i].collider.CompareTag("GRAVITY_CUBE"))
                {
                    Debug.Log("Entrei aqui no collider");
                    if (hits[i].distance < closestsObjectDistance)
                    {
                        closestsObjectDistance = hits[i].distance;
                        hit = hits[i];
                        Debug.Log("A distancia é: " + hits[i].distance);
                        foundCube = true;
                    }
                }
                if (i == hits.Length - 1 && foundCube)
                {
                    hit.transform.GetComponent<GravityCube>().SetCubeGravityRepelbehaviour();
                }
            }
        }


        Physics.Raycast(ray.origin, ray.direction, out hit, 1000f);
        //he has to be looking in same direction for grabbing to avoid object being grabbed if the player is looking elsewhere
        if (Input.GetKeyDown(KeyCode.E))
        {
            var grabbale = hit.transform.GetComponent<IGrabble>();
            if (grabbale != null && canGrab)
            {
                GrabObject(grabbale.Grab());
                return;

            }
                
            if (!canGrab)
            {
                DropObject();
                return;
            }

            var interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.InteractAction();
            }
        }
           
            
           
            
            

        

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GRAVITY_CUBE"))
        {

        }
    }

    private void GrabObject(Transform trans)
    {
        objectToGrab = trans;
        trans.SetParent(transform);
        trans.position = placeToPostitionGrabbedObject.position;
        canGrab = false;
    }

    private void DropObject() //verify detrod o cube se tiver dentro e algum ele faz snap para o local
    {
        objectToGrab.SetParent(null);
        objectToGrab.GetComponent<IDroppable>().Drop();
        //trans.position = placeToPostitionGrabbedObject.position;
        canGrab = true;
    }

    private Ray GetMouseRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }
}
