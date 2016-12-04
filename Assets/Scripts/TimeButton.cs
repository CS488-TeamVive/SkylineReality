using UnityEngine;
using System.Collections;

public class TimeButton : MonoBehaviour {

    public ButtonCollision collision;

    public enum TimeSetting { Hour = 0, Minute = 1 };
    public enum EffectSetting { Increase = 1, Decrease = -1 };

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

    private void EventHandlerRightController_OnTriggerClick(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if (collision.IsHighlighted())
            OnClick(timeSetting, effectSetting);
    }
}
