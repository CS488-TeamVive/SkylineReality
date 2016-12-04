using UnityEngine;
using System.Collections;

public class ClockButtonListener : MonoBehaviour {

    public delegate void TriggerActionOn(Transform sender);
    public event TriggerActionOn OnTriggerOn;

    public delegate void TriggerActionOff(Transform sender);
    public event TriggerActionOff OnTriggerOff;

    // Use this for initialization
    void Start () {
	
	}

    void OnTriggerEnter()
    {
        OnTriggerOn(transform);
    }
    void OnTriggerExit()
    {
        OnTriggerOff(transform);
    }
}
