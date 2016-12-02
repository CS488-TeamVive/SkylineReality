using UnityEngine;
using System.Collections;

public class ControllerClock : MonoBehaviour {

    LightController lightController;
    TextMesh clockTextMesh;
	// Use this for initialization
	void Start () {
        lightController = GameObject.FindWithTag("DirectionalLight").GetComponent<LightController>();
        clockTextMesh = this.GetComponentInChildren<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        clockTextMesh.text = string.Format("{0,2:D2}:{1,2:D2}", (int)lightController.TimeOfDay, (int)(lightController.TimeOfDay % 1 * 60));
	}
}
