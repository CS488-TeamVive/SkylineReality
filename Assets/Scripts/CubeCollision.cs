using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class CubeCollision : MonoBehaviour {
    
    private Renderer rend;
    public static Material defaultMat, highlightedMat, strongHighlightedMat;


    private bool isHighlighted = false;
    private bool isStrongHighlighted = false;

    private bool rightClick = false;
    private bool leftClick = false;
    
    public static bool globalIsStrongHighlighted = false;

    void OnEnable()
    {
        EventHandlerRightController.OnTriggerClick += EventHandlerRightController_OnTriggerClick;
        EventHandlerRightController.OnTriggerRelease += EventHandlerRightController_OnTriggerRelease;

        EventHandlerLeftController.OnTriggerClick += EventHandleLeftController_OnTriggerClick;
        EventHandlerLeftController.OnTriggerRelease += EventHandlerLeftController_OnTriggerRelease;
    }

    void OnDisable()
    {
        EventHandlerRightController.OnTriggerClick -= EventHandlerRightController_OnTriggerClick;
        EventHandlerRightController.OnTriggerRelease -= EventHandlerLeftController_OnTriggerRelease;

        EventHandlerLeftController.OnTriggerClick -= EventHandleLeftController_OnTriggerClick;
        EventHandlerLeftController.OnTriggerRelease -= EventHandlerLeftController_OnTriggerRelease;
    }

    private void EventHandlerRightController_OnTriggerClick(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        rightClick = true;

        if (leftClick && rightClick && isStrongHighlighted)
        {
            globalIsStrongHighlighted = false;
            transform.parent.gameObject.SetActive(false);
            GameObject[] list = new GameObject[] { transform.parent.gameObject, null };
            Debug.Log("OnTriggerAdd1");
            RedoUndo.AddUndo(list);
        }
    }

    private void EventHandlerRightController_OnTriggerRelease(object sender, ControllerInteractionEventArgs e)
    {
        rightClick = false;
    }

    private void EventHandleLeftController_OnTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        leftClick = true;

        if(leftClick && rightClick && isStrongHighlighted)
        {
            globalIsStrongHighlighted = false;
            transform.parent.gameObject.SetActive(false);
            GameObject[] list = new GameObject[] { transform.parent.gameObject, null };
            Debug.Log("OnTriggerAdd2");
            RedoUndo.AddUndo(list);
        }
    }

    private void EventHandlerLeftController_OnTriggerRelease(object sender, ControllerInteractionEventArgs e)
    {
        leftClick = false;
    } 

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider col)
    {
        defaultMat = Resources.Load("Default", typeof(Material)) as Material;
        highlightedMat = Resources.Load("Red", typeof(Material)) as Material;
        strongHighlightedMat = Resources.Load("Blue", typeof(Material)) as Material;
       
        if (!isHighlighted)
        {
            isHighlighted = true;
            rend.sharedMaterial = highlightedMat;
        }
        else if (isHighlighted)
        {
            Debug.Log("StrongHighlighting");
            isStrongHighlighted = true;
            globalIsStrongHighlighted = true;
            rend.sharedMaterial = strongHighlightedMat;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (isStrongHighlighted)
        {
            isStrongHighlighted = false;
            globalIsStrongHighlighted = false;
            rend.sharedMaterial = highlightedMat;
        }
        
        else if (isHighlighted)
        {
            isHighlighted = false;
            rend.sharedMaterial = defaultMat;
        }
    }

    public bool IsHighlighted()
    {
        return isHighlighted;
    }

    public bool IsStrongHighlighted()
    {
        return isStrongHighlighted;
    }
}
