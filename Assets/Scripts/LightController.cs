using UnityEngine;

public class LightController : MonoBehaviour {

    public float TimeExaggeration = 300;

    public float Latitude = 0;

    public enum Months { January, February, March, April, May, June, July, August, September, October, November, December };

    public Months Month;

    private float timeOfDay;
    [SerializeField]
    public float TimeOfDay {
        get
        {
            return timeOfDay;
        }
        set
        {
            if (value >= 24 || value < 0)
                timeOfDay = 0;
            else
                timeOfDay = value;
            UpdateSun();
        }
    }

    private bool timeProgression = false;

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

        if (!timeProgression)
        {
            return;
        }

        TimeOfDay = (TimeOfDay + (Time.deltaTime / 3600.0f) * TimeExaggeration) % 24.0f;

        UpdateSun();
	}

    void UpdateSun()
    {
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
        }

        if (TimeOfDay < 0)
            TimeOfDay = (TimeOfDay + 24f) % 24f;

        UpdateSun();
    }

    public void ToggleFastFoward()
    {
        this.timeProgression = !timeProgression;
    }
}
