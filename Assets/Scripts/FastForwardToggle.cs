using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FastForwardToggle : MonoBehaviour {

    public ButtonCollision collision;

    public Sprite uncheckedSprite, checkedSprite;

    public Image image;

    private bool isChecked = false;

    public delegate void ToggleButtonEvent();
    public static event ToggleButtonEvent OnClick;

    void Star()
    {
        collision = (ButtonCollision)GetComponent(typeof(ButtonCollision));
    }

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

            if (isChecked)
            {
                image.sprite = uncheckedSprite;
                isChecked = !isChecked;
            }
            else
            {
                image.sprite = checkedSprite;
                isChecked = !isChecked;
            }
        }
    }
}
