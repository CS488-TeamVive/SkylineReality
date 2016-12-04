using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCollision : MonoBehaviour {

    private UnityEngine.UI.Image rend;
    public Color defaultColor, HighlightedColor;
    private bool isHighlighted = false;

    void OnEnable()
    {
        rend = GetComponent<Image>();
        rend.color = defaultColor;
    }

    void OnTriggerEnter(Collider col)
    {
        rend.color = HighlightedColor;
        isHighlighted = true;
    }

    void OnTriggerExit(Collider col)
    {
        rend.color = defaultColor;
        isHighlighted = false;
    }

    public bool IsHighlighted()
    {
        return isHighlighted;
    }
}
