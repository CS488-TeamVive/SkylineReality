using UnityEngine;
using System.Collections;
using VRTK;
using System;

public class DiableTest : MonoBehaviour {

    public GameObject leftDefault, rightMain, buildingMenu;
    bool isEnabled = true;

    void OnEnable()
    {
        EventHandlerRightController.OnApplicationMenuPress += EventHandlerRightController_OnTriggerClick;
    }

    void OnDisable()
    {
        EventHandlerRightController.OnApplicationMenuPress -= EventHandlerRightController_OnTriggerClick;
    }

    private void EventHandlerRightController_OnTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        switch (OverrideControllerTexture.currentMenu)
        {
            case OverrideControllerTexture.MenuDisplayOption.Default:
                leftDefault.SetActive(true);
                break;
            case OverrideControllerTexture.MenuDisplayOption.Building_Selected:
                buildingMenu.SetActive(true);
                break;
        }

        rightMain.SetActive(true);
            
    }
}
