using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonCollisionGreyed : MonoBehaviour {

    private Image rend;
    public Color defaultColor, HighlightedColor, greyedOut;
    private bool isHighlighted = false;
    public bool isGreyed = true;
    public enum ButtonType { Undo = 0, Redo = 1 };

    public ButtonType buttonType;

    void OnEnable()
    {
        rend = GetComponent<Image>();
        if (isGreyed)
        {
            rend.color = greyedOut;
        }
        else
        {
            rend.color = defaultColor;
        }
    }

    void FixedUpdate()
    {
        switch (buttonType)
        {
            case ButtonType.Undo:
                //Debug.Log(RedoUndo.HasUndo());
                if (RedoUndo.HasUndo())
                {
                    isGreyed = false;
                    if (!isHighlighted)
                        rend.color = defaultColor;
                    else
                        rend.color = HighlightedColor;
                }
                else
                {
                    rend.color = greyedOut;
                    isGreyed = true;
                }
                break;
            case ButtonType.Redo:
                if (RedoUndo.HasRedo())
                {
                    isGreyed = false;
                    if (!isHighlighted)
                        rend.color = defaultColor;
                    else
                        rend.color = HighlightedColor;
                }
                else
                {
                    rend.color = greyedOut;
                    isGreyed = true;
                }
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!isGreyed)
        {
            rend.color = HighlightedColor;
            isHighlighted = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(!isGreyed)
        {
            rend.color = defaultColor;
        }
        isHighlighted = false;
    }

    public bool IsHighlighted()
    {
        return isHighlighted && !isGreyed;
    }
}
