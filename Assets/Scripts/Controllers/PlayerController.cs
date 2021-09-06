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
    bool playerNearCube;
    Transform objectToGrab;
    [SerializeField]
    Transform placeToPostitionGrabbedObject;

    [SerializeField]
    float totalMovementTime = 1f; //the amount of time you want the movement to take

    float currentMovementTime = 0f;//The amount of time that has passed

    Vector3 cubeOrigin;

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
        

        
        //if clicks left mouse, turns cube blueish(attract)
        if (Input.GetMouseButtonDown(0))
        {
            foundCube = false;
            ray = GetMouseRay();
            hits = Physics.RaycastAll(ray.origin, ray.direction, 1000f);

            closestsObjectDistance = hits[0].distance;
            for (int i = 0; i < hits.Length; i++)
            {
                
                if (hits[i].collider.CompareTag("GRAVITY_CUBE"))
                {
                   
                    if(hits[i].distance < closestsObjectDistance)
                    {
                        closestsObjectDistance = hits[i].distance;
                        hit = hits[i];
                        
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
            ray = GetMouseRay();
            hits = Physics.RaycastAll(ray.origin, ray.direction, 1000f);

            closestsObjectDistance = hits[0].distance;
            for (int i = 0; i < hits.Length; i++)
            {
              
                if (hits[i].collider.CompareTag("GRAVITY_CUBE"))
                {
                   
                    if (hits[i].distance < closestsObjectDistance)
                    {
                        closestsObjectDistance = hits[i].distance;
                        hit = hits[i];
                       
                        foundCube = true;
                    }
                }
                if (i == hits.Length - 1 && foundCube)
                {
                    hit.transform.GetComponent<GravityCube>().SetCubeGravityRepelbehaviour();
                }
            }
        }


        //Physics.Raycast(ray.origin, ray.direction, out hit, 1000f);
        //he has to be looking in same direction for grabbing to avoid object being grabbed if the player is looking elsewhere
        if (Input.GetKeyDown(KeyCode.E))
        {
            ray = GetMouseRay();
            hits = Physics.RaycastAll(ray.origin, ray.direction, 1000f);

            closestsObjectDistance = hits[0].distance;
            for (int i = 0; i < hits.Length; i++)
            {
                
                if (hits[i].collider.CompareTag("GRAVITY_CUBE"))
                {
            
                    if (hits[i].distance < closestsObjectDistance)
                    {
                        closestsObjectDistance = hits[i].distance;
                        hit = hits[i];
                        
                        foundCube = true;
                    }
                }
                if (i == hits.Length - 1 && foundCube)
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

            
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InterfaceManager.Instance.PauseGameEscape("Paused Game");
        }
           
            
           
            
            

        

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GRAVITY_CUBE") || other.CompareTag("PLATFORM"))
        {
            playerNearCube = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GRAVITY_CUBE") || other.CompareTag("PLATFORM"))
        {
            playerNearCube = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GRAVITY_CUBE") || other.CompareTag("PLATFORM"))
        {
            playerNearCube = false;
        }
        if (other.CompareTag("DEATH"))
        {
            
            APPManager.Instance.ResetGame();
        }
    }

    private void GrabObject(Transform trans)
    {
        objectToGrab = trans;
        cubeOrigin = objectToGrab.position;
        objectToGrab.SetParent(transform);
        StartCoroutine(MoveObject(cubeOrigin));
        //objectToGrab.position = placeToPostitionGrabbedObject.position;
        canGrab = false;
    }

    private void DropObject() //verify detrod o cube se tiver dentro e algum ele faz snap para o local
    {
        
        objectToGrab.SetParent(null);
        objectToGrab.GetComponent<IDroppable>().Drop();
        //trans.position = placeToPostitionGrabbedObject.position;
        canGrab = true;
    }

    public IEnumerator MoveObject(Vector3 origin)
    {
       
        while (Vector3.Distance(objectToGrab.position, placeToPostitionGrabbedObject.position) > 0.1f)
        {
            currentMovementTime += Time.deltaTime;
           
            objectToGrab.position = Vector3.Lerp(origin, placeToPostitionGrabbedObject.position, currentMovementTime / totalMovementTime);
            yield return null;
        }
        currentMovementTime = 0;
    }

    private Ray GetMouseRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }
}
