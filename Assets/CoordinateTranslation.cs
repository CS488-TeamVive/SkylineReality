using UnityEngine;
using System.Collections;

public class CoordinateTranslation : MonoBehaviour
{

    private Vector2 actualLowerLeftCorner;
    public Vector2 ActualLowerLeftCorner { get { return actualLowerLeftCorner; } set { actualLowerLeftCorner = value; } }
    /*
    public Vector2 ActualUpperLeftCorner { }
    public Vector2 ActualLowerRightCorner { }
    public Vector2 ActualUpperRightCorner { }
    */
}