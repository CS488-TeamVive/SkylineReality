using UnityEngine;
using System.Collections;
using VRTK;

public class TooltipDisable : MonoBehaviour {

    private VRTK_ControllerTooltips tip;
    public int disableSeconds;
    private int count = 0;

    void OnEnable()
    {
        count = 0;
    }

    void Start()
    {
        tip = (VRTK_ControllerTooltips)GetComponent(typeof(VRTK_ControllerTooltips));
    }

    void FixedUpdate()
    {
        if((disableSeconds * 60) <= count)
        {
            transform.gameObject.SetActive(false);
        }
        count++;
    }
}
