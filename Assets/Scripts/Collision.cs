using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour {

    private UnityEngine.UI.Image rend;
    //public Material defaultMat, highlightedMat;
    public Color defaultColor, HighlightedColor;
    private bool isHighlighted = false;

    public enum TimeSetting { Hour = 0, Minute = 1 };
    public enum EffectSetting { Increase = 1, Decrease = -1 } ;

    public TimeSetting timeSetting;
    public EffectSetting effectSetting;

    public delegate void TimeButtonEvent(TimeSetting timeSetting, EffectSetting effectSetting);
    public static event TimeButtonEvent OnClick;

    void OnEnable()
    {
        EventHandlerRightController.OnTriggerClick += EventHandlerRightController_OnTriggerClick;
    }

    void OnDisable()
    {
        EventHandlerRightController.OnTriggerClick -= EventHandlerRightController_OnTriggerClick;
    }

    void Start ()
    {
        rend = GetComponent<UnityEngine.UI.Image>();
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

    private void EventHandlerRightController_OnTriggerClick(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if (isHighlighted)
            OnClick(timeSetting, effectSetting);
    }
}
