using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeUpdater : MonoBehaviour {

    LightController lightController;
    Text text;

    void Start () {
        lightController = GameObject.FindWithTag("DirectionalLight").GetComponent<LightController>();
        text = GetComponent<Text>();
	}

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("{0,2:D2}:{1,2:D2}", (int)lightController.TimeOfDay, (int)(lightController.TimeOfDay % 1 * 60));
    }
}
