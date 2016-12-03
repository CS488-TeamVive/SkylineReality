using UnityEngine;

public class LightController : MonoBehaviour {

    public float TimeExaggeration = 300;

    public float Latitude = 0;

    public enum Months { January, February, March, April, May, June, July, August, September, October, November, December };

    public Months Month;

    public float TimeOfDay;

    private bool isFastForward = false;

    void OnEnable()
    {
        TimeButton.OnClick += AdjustTime;
        FastForwardToggle.OnClick += ToggleFastFoward;
    }

    void OnDisable()
    {
        TimeButton.OnClick -= AdjustTime;
        FastForwardToggle.OnClick -= ToggleFastFoward;
    }

    // Use this for initialization
    void Start () {
        Month = Months.January;
        TimeOfDay = 12.0f;
	}


    private float SunAngle;
    private bool DayTime = true;
	// Update is called once per frame
	void Update () {

        if (!isFastForward)
        {
            return;
        }

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

    public void UpdateTimeOfDay(int hour, int minute)
    {
        TimeOfDay = hour + (minute / 60);
    }

    private void AdjustTime(TimeButton.TimeSetting timeSetting, TimeButton.EffectSetting effectSetting)
    {
        if(timeSetting == TimeButton.TimeSetting.Hour)
        {
            TimeOfDay = (TimeOfDay + (int)effectSetting) % 24.0f;
        }
        else
        {
            TimeOfDay = (TimeOfDay + ((int)effectSetting / 60.0f)) % 24.0f;
            Debug.Log(TimeOfDay);
        }

        if(TimeOfDay < 0)
        {
            TimeOfDay = (24 + (int)TimeOfDay) + (TimeOfDay % 1);
        }

        SunAngle = ((TimeOfDay + 6.0f) % 12.0f) * 15.0f;

        this.transform.rotation = Quaternion.Euler(SunAngle, 90.0f, 0.0f);
    }

    public void ToggleFastFoward()
    {
        this.isFastForward = !isFastForward;
    }
}
