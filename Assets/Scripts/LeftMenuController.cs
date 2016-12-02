using UnityEngine;
using VRTK;

public class LeftMenuController : MonoBehaviour {

    public enum MenuOption { Calendar_Selected = 0, Sun_Selected = 1, Building_Selected = 2, Mag_Selected = 3 };

    public delegate void MenuSelect(MenuOption selectedOption);
    public static event MenuSelect OnMenuSelection;

    void OnEnable()
    {
        EventHandlerLeftController.OnTouchpadPress += TouchPadPress;
    }

    void OnDisable()
    {
        EventHandlerLeftController.OnTouchpadPress -= TouchPadPress;
    }

    private void TouchPadPress(object sender, ControllerInteractionEventArgs e)
    {
        MenuOption menuOptionSelected = GetMenuOptionSelected(e.touchpadAngle);
        OnMenuSelection(menuOptionSelected);
    }

    private MenuOption GetMenuOptionSelected(float angle)
    {
        if (angle >= 45 && angle < 135)
        {
            return MenuOption.Building_Selected;
        }
        else if (angle >= 135 && angle < 225)
        {
            return MenuOption.Mag_Selected;
        }
        else if (angle >= 225 && angle < 315)
        {
            return MenuOption.Sun_Selected;
        }
        else
        {
            return MenuOption.Calendar_Selected;
        }
    }
}
