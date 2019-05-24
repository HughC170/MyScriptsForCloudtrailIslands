using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manageTime : MonoBehaviour {

    public DayNightCycle daynight;
    //Kris' code - this manages all the world particles (not player) in the scene
    public ParticlesManager particleManagerScript;
    [Tooltip("Total time in seconds for an entire day. Refer to 5th case in 'DayNightCycle' script")]
    public float totalTime;

    //public int CurrentPatrolPoint = 0;

    public int daycount = 1;
    public int Totaldaycount = 1;
    public string[] Seasons = { "Spring", "Summer", "Autumn", "Winter" };// was tempted to do enumerators here but left it
    public string currentseason;
    public enum day {Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday};
    public day CurrentDay;
    int Seasoncount = 1;
    public int year = 1;
    public int nextmonthcount = 1;

    public Color morningColor;
    public Color morningColorDirLight;
    [Space(10)]
    public Color dayColor;
    public Color dayColorDirLight;
    [Space(10)]
    public Color eveningColor;
    public Color eveningColorDirLight;
    [Space(10)]
    public Color nightcolor;
    public Color nightcolorDirLight;
    [Space(10)]
    public float delaySpeed = 10.0f;

    public Material morningbox;
    public Material daybox;
    public Material eveningbox;
    public Material nightbox;
    public Text alert;
    public Text currentDayText; //the text under the clock showing what day it is currently

    public Color32 morningfog;
    public Color32 dayfog;
    public Color32 eveningfog;
    public Color32 nightfog;

    public bool isNight = false;//This doesn't influence anything in this script

    private Quaternion RotNight;
    private Quaternion RotDay;
    private Quaternion RotMorning;
    private Quaternion RotEvening;

    Light lightSource;

    private void Awake()
    {
        RotNight = Quaternion.Euler(200, 0, 0);//get value for directional light rotation//originally 200
        RotDay = Quaternion.Euler(20, 0, 0);//get value for directional light rotation//originally 20
        RotMorning = Quaternion.Euler(20, 0, 0);
        RotMorning = Quaternion.Euler(200, 0, 0);

    }
    void Start ()//start at day time
    {
        lightSource = GetComponent<Light>();//get light component inside of this object
        lightSource.color = dayColor;//set starting color
    }
	
	void Update () {/*
        if (Input.GetKeyDown(KeyCode.L)
            && !isNight)
        {
            isNight = true;
        } else if (Input.GetKeyDown(KeyCode.L) 
            && isNight)
        {
            isNight = false;
        }*/

        DirectionalLightRotation();

        switch (daynight.currentTime)
        {
            case DayNightCycle.TimePeriod.Morning:
                MorningTime();
                //dayRegister();
                break;
            case DayNightCycle.TimePeriod.Afternoon:
                DayTime();
                break;
            case DayNightCycle.TimePeriod.Evening:
                EveningTime();
                break;
            case DayNightCycle.TimePeriod.Night:
                NightTime();
                break;
        }

        currentDayText.text = CurrentDay.ToString(); //sets up the current day text to match current day
	}

    public void DirectionalLightRotation()
    {
         float rotationSpeed = 360 / totalTime * Time.deltaTime;
        //float rotationSpeed_X = 180 / totalTime * Time.deltaTime;

        
        transform.Rotate(rotationSpeed, 0, 0);

        if (transform.localEulerAngles.x >= 180.0f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //print("ROTATION: " + transform.localEulerAngles);

        /*
        transform.RotateAround(Vector3.zero, Vector3.right, rotationSpeed);
        transform.LookAt(Vector3.zero); */
    }
    
    public void NightTime()
    {
        lightSource.color = Color.Lerp(lightSource.color, nightcolorDirLight, Time.deltaTime * delaySpeed);//changes the light component's color
        //transform.rotation = Quaternion.Lerp(transform.rotation, RotNight, Time.deltaTime * delaySpeed);//rotates the directional light to make it dark
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, nightcolor, Time.deltaTime * delaySpeed * 2.5f);//changes the ambient light to make objects dark
        //Debug.Log("time changed to night");
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, nightfog, Time.deltaTime * delaySpeed);//changes the fog color;

        //Kris - tell the particle manager script that its night time
        particleManagerScript.nightTime = true;
    }

    public void EveningTime()
    {
        lightSource.color = Color.Lerp(lightSource.color, eveningColorDirLight, Time.deltaTime * delaySpeed);//changes the light component's color
        //transform.rotation = Quaternion.Lerp(transform.rotation, RotEvening, Time.deltaTime * delaySpeed);//rotates the directional light to make it dark
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, eveningColor, Time.deltaTime * delaySpeed * 2.5f);//changes the ambient light to make objects dark
        //Debug.Log("time changed to Evening");
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, eveningfog, Time.deltaTime * delaySpeed);//changes the fog color;


        //Kris - tell the particle manager script that its not time
        particleManagerScript.nightTime = false;
    }

    public void MorningTime()
    {
        lightSource.color = Color.Lerp(lightSource.color, morningColorDirLight, Time.deltaTime * delaySpeed);//changes the light component's color
        //transform.rotation = Quaternion.Lerp(transform.rotation, RotMorning, Time.deltaTime * delaySpeed);//rotates the directional light to make it dark
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, morningColor, Time.deltaTime * delaySpeed * 2.5f);//changes the ambient light to make objects dark
        //Debug.Log("time changed to morning");
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, morningfog, Time.deltaTime * delaySpeed);//changes the fog color;

        //Kris - tell the particle manager script that its not time
        particleManagerScript.nightTime = false;
    }

    public void DayTime()
    {
        lightSource.color = Color.Lerp(lightSource.color, dayColor, Time.deltaTime * delaySpeed);//changes the light component's color
        //transform.rotation = Quaternion.Lerp(transform.rotation, RotDay, Time.deltaTime * delaySpeed);//rotates the directional light to make it dark
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, dayColor, Time.deltaTime * delaySpeed);//changes the ambient light to make objects dark
        //Debug.Log("time changed to day");
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, dayfog, Time.deltaTime * delaySpeed);//changes the fog color;

        //Kris - tell the particle manager script that its not time
        particleManagerScript.nightTime = false;
    }

    public void dayRegister()
    {
            daycount += 1;
        Totaldaycount += 1;
        switch(daycount)
        {
            case 1:
                CurrentDay = day.Monday;
                break;
            case 2:
                CurrentDay = day.Tuesday;
                break;
            case 3:
                CurrentDay = day.Wednesday;
                break;
            case 4:
                CurrentDay = day.Thursday;
                break;
            case 5:
                CurrentDay = day.Friday;
                break;
            case 6:
                CurrentDay = day.Saturday;
                break;
            case 7:
                CurrentDay = day.Sunday;
                nextmonthcount += 1;
                daycount = 0;
                break;
        }
        monthRegister();
    }

    private void monthRegister()
    {
        if (nextmonthcount == 4)
        {
            nextmonthcount = 0;
            Seasoncount += 1;
            switch (Seasoncount)
            {
                case 1:
                    currentseason = Seasons[1];
                    break;
                case 2:
                    currentseason = Seasons[2];
                    break;
                case 3:
                    currentseason = Seasons[3];
                    break;
                case 4:
                    currentseason = Seasons[0];
                    Seasoncount = 0;
                    year += 1;
                    break;
            }
        }
    }
}
