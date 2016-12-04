using UnityEngine;
using System.Collections;
using VRTK;

public class ClockMenuController : MonoBehaviour {

    public Canvas clockCanvas;
    public Canvas applicationMenu;
    private bool isClockOpen = false;
    private bool isAppMenuOpen = false;

    void OnEnable()
    {
        LeftMenuController.OnMenuSelection += EnableClockMenu;
        EventHandlerLeftController.OnApplicationMenuPress += ToggleMenu;
    }

    void OnDisable()
    {
        LeftMenuController.OnMenuSelection -= EnableClockMenu;
        EventHandlerLeftController.OnApplicationMenuPress -= ToggleMenu;
    }

    void Start()
    {
        //GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(ToggleMenu);
    }

    public void EnableClockMenu(LeftMenuController.MenuOption selection)
    {
        if(selection == LeftMenuController.MenuOption.Mag_Selected)
        {
        }

        else if (selection != LeftMenuController.MenuOption.Sun_Selected)
        {
            if(isClockOpen)
            {
                clockCanvas.gameObject.SetActive(false);
                isClockOpen = false;
            }          
        }

        else if (!isClockOpen)
        {
            clockCanvas.gameObject.SetActive(true);
            isClockOpen = !isClockOpen;
        }
        else
        {
            clockCanvas.gameObject.SetActive(false);
            isClockOpen = !isClockOpen;
        }
    }

    public void ToggleMenu(object sender, ControllerInteractionEventArgs e)
    {
        if (!isAppMenuOpen)
        {
            applicationMenu.gameObject.SetActive(true);
            isAppMenuOpen = true;
        }
        else
        {
            applicationMenu.gameObject.SetActive(false);
            isAppMenuOpen = false;
        }
    }
}
