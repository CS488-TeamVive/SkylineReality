using UnityEngine;
using System.Collections;

public class RedoButton : MonoBehaviour {

    public ButtonCollisionGreyed collision;

    public delegate void RedoButtonEvent();
    public static RedoButtonEvent OnClick;

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
        {
            OnClick();
        }
    }
}
