using UnityEngine;
using System.Collections;
using VRTK;

public class SphereColliderDragYDir : MonoBehaviour {

    private Renderer rend;
    public Material defaultMat, highlightedMat;
    
    private Vector3 prevControllerPos = new Vector3(0, 0, 0);

    private bool isHighlighted = false;
    private bool isGrabbed = false;

    public Transform controllerTransform;

    void OnEnable()
    {
        EventHandlerRightController.OnTriggerClick += EventHandlerRightController_OnTriggerClick;
        EventHandlerRightController.OnTriggerRelease += EventHandlerRightController_OnTriggerRelease;
    }

    void OnDisable()
    {
        EventHandlerRightController.OnTriggerClick -= EventHandlerRightController_OnTriggerClick;
        EventHandlerRightController.OnTriggerRelease += EventHandlerRightController_OnTriggerRelease;
    }

    private void EventHandlerRightController_OnTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        controllerTransform = GameObject.Find("Controller (right)").transform;
        if (isHighlighted)
        {
            isGrabbed = true;
            prevControllerPos = controllerTransform.transform.position;
            Debug.Log("First Cords: " + prevControllerPos);
        }
    }

    private void EventHandlerRightController_OnTriggerRelease(object sender, ControllerInteractionEventArgs e)
    {
        if (isGrabbed)
        {
            isGrabbed = false;
        }
    }

    void Update()
    {
        if (!isGrabbed)
        {
            return;
        }

        Vector3 controllerCoords = controllerTransform.transform.position;

        if (controllerCoords == null)
        {
            return;
        }

        Vector3 controllerMovement = controllerCoords - prevControllerPos;
        prevControllerPos = controllerCoords;
        transform.position = new Vector3(transform.position.x, transform.position.y + controllerMovement.y, transform.position.z);
    }
    
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (!isHighlighted)
        {
            isHighlighted = true;
            rend.sharedMaterial = highlightedMat;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (isHighlighted)
        {
            isHighlighted = false;
            rend.sharedMaterial = defaultMat;
        }
    }

    public bool IsHighlighted()
    {
        return isHighlighted;
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }
}
