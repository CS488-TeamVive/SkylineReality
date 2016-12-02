using UnityEngine;
using System.Collections;
using VRTK;

public class EventHandlerRightController : MonoBehaviour {

    public delegate void ControllerEvent(object sender, ControllerInteractionEventArgs e);

    public static event ControllerEvent OnTriggerPress;
    public static event ControllerEvent OnTriggerRelease;
    public static event ControllerEvent OnTriggerTouchStart;
    public static event ControllerEvent OnTriggerTouchEnd;
    public static event ControllerEvent OnTriggerHairlineStart;
    public static event ControllerEvent OnTriggerHairlineEnd;
    public static event ControllerEvent OnTriggerClick;
    public static event ControllerEvent OnTriggerUnclick;
    public static event ControllerEvent OnTriggerAxisChange;
    public static event ControllerEvent OnApplicationMenuPress;
    public static event ControllerEvent OnApplicationMenuRelease;
    public static event ControllerEvent OnGripPress;
    public static event ControllerEvent OnGripRelease;
    public static event ControllerEvent OnTouchpadPress;
    public static event ControllerEvent OnTouchpadRelease;
    public static event ControllerEvent OnTouchpadTouchStart;
    public static event ControllerEvent OnTouchpadTouchEnd;
    public static event ControllerEvent OnTouchpadAxisChange;
    public static event ControllerEvent OnControllerEnable;
    public static event ControllerEvent OnControllerDisable;

    private void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        GetComponent<VRTK_ControllerEvents>().TriggerTouchStart += new ControllerInteractionEventHandler(DoTriggerTouchStart);
        GetComponent<VRTK_ControllerEvents>().TriggerTouchEnd += new ControllerInteractionEventHandler(DoTriggerTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TriggerHairlineStart += new ControllerInteractionEventHandler(DoTriggerHairlineStart);
        GetComponent<VRTK_ControllerEvents>().TriggerHairlineEnd += new ControllerInteractionEventHandler(DoTriggerHairlineEnd);

        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

        GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);

        GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
        GetComponent<VRTK_ControllerEvents>().ApplicationMenuReleased += new ControllerInteractionEventHandler(DoApplicationMenuReleased);

        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        GetComponent<VRTK_ControllerEvents>().ControllerEnabled += new ControllerInteractionEventHandler(DoControllerEnabled);
        GetComponent<VRTK_ControllerEvents>().ControllerDisabled += new ControllerInteractionEventHandler(DoControllerDisabled);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(e.controllerIndex, "TRIGGER", "pressed", e);
        if (OnTriggerPress != null)
        {
            OnTriggerPress(sender, e);
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerRelease != null)
        {
            OnTriggerRelease(sender, e);
        }
    }

    private void DoTriggerTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerTouchStart != null)
        {
            OnTriggerTouchStart(sender, e);
        }
    }

    private void DoTriggerTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerTouchEnd != null)
        {
            OnTriggerTouchEnd(sender, e);
        }
    }

    private void DoTriggerHairlineStart(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerHairlineStart != null)
        {
            OnTriggerHairlineStart(sender, e);
        }
    }

    private void DoTriggerHairlineEnd(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerHairlineEnd != null)
        {
            OnTriggerHairlineEnd(sender, e);
        }
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerClick != null)
        {
            OnTriggerClick(sender, e);
        }
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerUnclick != null)
        {
            OnTriggerUnclick(sender, e);
        }
    }

    private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerAxisChange != null)
        {
            OnTriggerAxisChange(sender, e);
        }
    }

    private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTriggerAxisChange != null)
        {
            OnApplicationMenuPress(sender, e);
        }
    }

    private void DoApplicationMenuReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (OnApplicationMenuRelease != null)
        {
            OnApplicationMenuRelease(sender, e);
        }
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (OnGripPress != null)
        {
            OnGripPress(sender, e);
        }
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (OnGripRelease != null)
        {
            OnGripRelease(sender, e);
        }
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTouchpadPress != null)
        {
            OnTouchpadPress(sender, e);
        }
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTouchpadRelease != null)
        {
            OnTouchpadRelease(sender, e);
        }
    }

    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTouchpadTouchStart != null)
        {
            OnTouchpadTouchStart(sender, e);
        }
    }

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTouchpadTouchEnd != null)
        {
            OnTouchpadTouchEnd(sender, e);
        }
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        if (OnTouchpadAxisChange != null)
        {
            OnTouchpadAxisChange(sender, e);
        }
    }

    private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
    {
        if (OnControllerEnable != null)
        {
            OnControllerEnable(sender, e);
        }
    }

    private void DoControllerDisabled(object sender, ControllerInteractionEventArgs e)
    {
        if (OnControllerDisable != null)
        {
            OnControllerDisable(sender, e);
        }
    }
}
