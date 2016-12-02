using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour {

    private UnityEngine.UI.Image rend;
    //public Material defaultMat, highlightedMat;
    public Color defaultColor, HighlightedColor;
    private bool isHighlighted = false;

    void OnEnable()
    {
        EventHandlerRightController.OnTriggerClick += EventHandlerRightController_OnTriggerClick;
    }

    private void EventHandlerRightController_OnTriggerClick(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if(isHighlighted)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<UnityEngine.UI.Image>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger Enter");
        rend.color = HighlightedColor;
        isHighlighted = true;
        //rend.sharedMaterial = highlightedMat;

    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Trigger Exit");
        rend.color = defaultColor;
        isHighlighted = false;
        //rend.sharedMaterial = defaultMat;
    }
}
