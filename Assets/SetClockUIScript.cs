using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetClockUIScript : MonoBehaviour {

    private Button[] HourButtons;
    private string[] HourNames = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve" };

    LightController light;

    // Use this for initialization
    void Start() {
        light = GameObject.Find("Directional Light").GetComponent<LightController>();

        HourButtons = new Button[12];
        for (int hour = 0; hour < HourNames.Length; hour++)
        {
            HourButtons[hour] = transform.FindChild(HourNames[hour] + "Button").GetComponent<Button>();
            HourButtons[hour].GetComponent<ClockButtonListener>().OnTriggerOn += HourButtonTriggeredOn;
            HourButtons[hour].GetComponent<ClockButtonListener>().OnTriggerOff += HourButtonTriggeredOff;
        }


    }

    void HourButtonTriggeredOn(Transform sender)
    {


        StartCoroutine("FadeImage", sender.GetComponent<Image>());
    }

    void HourButtonTriggeredOff(Transform sender)
    {
        
    }

    void SunButtonTriggered(Transform sender)
    {

    }

    void MoonButtonTriggered(Transform sender)
    {

    }

    IEnumerator FadeImage(Image im)
    {
        im.CrossFadeColor(Color.cyan, 0.3f, false, false);
        yield return new WaitForSeconds(0.3f);
        im.CrossFadeColor(Color.white, 1.0f, false, false);
    }
    void OnEnable()
    {

    }

    void OnDisable()
    {

    }




}
