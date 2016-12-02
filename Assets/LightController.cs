using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

    public float TimeExaggeration = 300;

    public float Latitude = 0;

    public enum Months { January, February, March, April, May, June, July, August, September, October, November, December };

    public Months Month;

    public float TimeOfDay;

	// Use this for initialization
	void Start () {
        Month = Months.January;
        TimeOfDay = 12.0f;
	}


    private float SunAngle;
    private bool DayTime = true;
	// Update is called once per frame
	void FixedUpdate () {
        TimeOfDay = (TimeOfDay + (Time.deltaTime / 3600.0f) * TimeExaggeration) % 24.0f;

        SunAngle = ((TimeOfDay + 6.0f) % 12.0f) * 15.0f;

        this.transform.rotation = Quaternion.Euler(SunAngle, 90.0f, 0.0f);

        if (DayTime && (TimeOfDay < 6.0f || TimeOfDay > 18.0f))
        { // night time
            DayTime = false;
            this.GetComponent<Light>().intensity = 0.5f;
        }
        else if (!DayTime && (TimeOfDay >= 6.0f && TimeOfDay <= 18.0f))
        { // day time
            DayTime = true;
            this.GetComponent<Light>().intensity = 1.0f;
        }
	}
}
