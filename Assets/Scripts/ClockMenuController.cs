using UnityEngine;
using System.Collections;

public class ClockMenuController : MonoBehaviour {

    public Canvas clockCanvas;
    private bool isMenuOpen = false;

    void OnEnable()
    {
        LeftMenuController.OnMenuSelection += EnableClockMenu;
    }

    void OnDisable()
    {
        LeftMenuController.OnMenuSelection -= EnableClockMenu;
    }

    public void EnableClockMenu(LeftMenuController.MenuOption selection)
    {
        if(selection == LeftMenuController.MenuOption.Mag_Selected)
        {
        }

        else if (selection != LeftMenuController.MenuOption.Sun_Selected)
        {
            if(isMenuOpen)
            {
                clockCanvas.gameObject.SetActive(false);
                isMenuOpen = false;
            }          
        }

        else if (!isMenuOpen)
        {
            clockCanvas.gameObject.SetActive(true);
            isMenuOpen = !isMenuOpen;
        }
        else
        {
            clockCanvas.gameObject.SetActive(false);
            isMenuOpen = !isMenuOpen;
        }
    }
}
