using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerClock : MonoBehaviour {

    LightController lightController;
    Text clockText;
	// Use this for initialization
	void Start () {
        lightController = GameObject.FindWithTag("DirectionalLight").GetComponent<LightController>();
        clockText = transform.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        clockText.text = string.Format("{0,2:D2}:{1,2:D2}", (int)lightController.TimeOfDay, (int)(lightController.TimeOfDay % 1 * 60));
	}
}
