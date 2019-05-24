using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DayNightCycle : MonoBehaviour {

    public Flowchart flowchart;
    StoryChecker storycheck;

    public float CurrentPatrolPointf = 0f;
    public int CurrentPatrolPoint = 0;

    public CharControl2 character;
    public manageTime Timing;
    public PatrolManager patroller;
    public bool changeok = true;
    public float timeCount = 0f;
    public int timeCountrounded;
    public bool spawncheck = false;

    public int []dayperiodlength;
    public enum TimePeriod { Morning, Afternoon, Evening, Night };
    public TimePeriod currentTime;

    [HideInInspector]
    public bool isTalking;
    private void Awake () {
        storycheck = GetComponent<StoryChecker>();
        character = GetComponent<CharControl2>();
    }
	
	void Update () {
        TimePassing();
	}

    #region ignore for the moment
    /*private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "bench")
        {
            Timing.alert.enabled = true;
            if (changeok == true)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    if (!Timing.isNight)
                    {
                        timeCount = 10;
                        changeok = false;
                        Timing.isNight = true;
                        flowchart.ExecuteBlock("Daychange");
                        spawncheck = false;//this is for registering how long it'll take for something to spawn back
                    }
                    else if (Timing.isNight)
                    {
                        timeCount = 0;
                        changeok = false;
                        Timing.isNight = false;
                        flowchart.ExecuteBlock("Daychange");
                        spawncheck = true;//this is for registering how long it'll take for something to spawn back
                    }
                }
            }
        }
        switch (Timing.isNight)
        {
            case false:
                Timing.DayTime();
                break;
            case true:
                Timing.NightTime();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "bench")
        {
            Timing.alert.enabled = false;
        }
    }*/
    #endregion

    public void switcher()
    {
        /*switch (Timing.isNight)//Just setting the text for bench time skip and skybox change
        {
            case false:
                Timing.alert.color = Color.black;
                Timing.alert.text = "Press L to change to night";
                RenderSettings.skybox = Timing.daybox;
                break;
            case true:
                Timing.alert.color = Color.white;
                Timing.alert.text = "Press L to change to day";
                RenderSettings.skybox = Timing.nightbox;
                break;
        }*/
        switch (currentTime)
        {
            case TimePeriod.Morning:
                //Timing.alert.color = Color.black;
                //Timing.alert.text = "Press L to change to afternoon";
                RenderSettings.skybox = Timing.morningbox;
                break;
            case TimePeriod.Afternoon:
                //Timing.alert.color = Color.black;
                //Timing.alert.text = "Press L to change to evening";
                RenderSettings.skybox = Timing.daybox;
                break;
            case TimePeriod.Evening:
                //Timing.alert.color = Color.black;
                //Timing.alert.text = "Press L to change to night";
                RenderSettings.skybox = Timing.eveningbox;
                break;
            case TimePeriod.Night:
                //Timing.alert.color = Color.white;
                //Timing.alert.text = "Press L to change to morning";
                RenderSettings.skybox = Timing.nightbox;
                break;
        }
    }

    public void Enabled()
    {
        changeok = true;
    }

    public void NextSpotReady()
    {
        CurrentPatrolPoint += 1;
    }

    void TimePassing()
    {
        float passingtime;

        //CurrentPatrolPoint = (Mathf.RoundToInt(CurrentPatrolPointf));
        if (!isTalking)
        {
            passingtime = Time.deltaTime; // if not  talking
            timeCount += passingtime;
            //elapsedTime = timeCount;
        } else
        {
            //if talking
            passingtime = 0;
        }

        timeCountrounded = (int)timeCount;
        switch (timeCountrounded)
        {
            case 0:
                //changeok = false;
                storycheck.AllCharactersReady();
                currentTime = TimePeriod.Morning;
                Timing.isNight = false;
                character.spawncheck = true;
                flowchart.ExecuteBlock("Daychange");
                patroller.patrol();
                break;
            case 180:// 60://this is in seconds. Default is 600 //180
                currentTime = TimePeriod.Afternoon;
                flowchart.ExecuteBlock("Daychange");
                patroller.patrol();
                break;
            case 360:// 120://this is in seconds. Default is 1200 //360
                currentTime = TimePeriod.Evening;
                Timing.isNight = true;
                flowchart.ExecuteBlock("Daychange");
                patroller.patrol();
                break;
            case 540:// 180://this is in seconds. Default is 1800 //540
                currentTime = TimePeriod.Night;
                character.spawncheck = false;//this is for registering how long it'll take for something to spawn back
                flowchart.ExecuteBlock("Daychange");
                patroller.patrol();
                break;
            case 720:// 240://this is in seconds. Default is 2400 //720
                if (Timing.CurrentDay == manageTime.day.Sunday)
                {
                    CurrentPatrolPoint = 0;
                }
                Timing.dayRegister();
                timeCount = 0;
                break;
        }
    }
}
