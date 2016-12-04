using UnityEngine;
using System.Collections;

public class UndoButton : MonoBehaviour {

    public ButtonCollisionGreyed collision;

    public delegate void UndoButtonEvent();
    public static UndoButtonEvent OnClick;

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
