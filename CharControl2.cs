using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CharControl2 : MonoBehaviour {
    #region Variables...
    #region For numerous procedures
    public CharControl Controller;
    public Flowchart flowchart;
    public DayNightCycle daynight;
    #endregion
    #region for simple inventory ui toggle
    public StoryChecker storycheck;
    public GameObject inventory;
    public GameObject inventory2;
    public GameObject tabs;
    public GameObject notice;
    #endregion
    #region For tutorial text
    public Text info;
    #endregion
    #region For respawn
    public Text respawnertext;
    public float floattimer = 10f;
    public int inttimer;
    #endregion
    #region For signs
    public Text signtext;
    #endregion
    #region For notes
    public Text notetext;
    public Text notetextexit;
    public Image notepic;
    #endregion
    #region For stamina bar
    public Image boostMetrered;
    #endregion
    #region For day/night changing
    public manageTime Timing;
    public bool changeok =  true;
    //new
    public float timeCount = 0f;
    public int timeCountrounded;
    public float DayLength = 10f;
    public float NightLength = 10f;
    //new
    public tabchanger TabManaging;
    #endregion
    #region For checking when items should spawn
    public bool spawncheck = false;
    #endregion
    #region For Uplift
    public Rigidbody rb;
    public AudioSource woosh;
    [Tooltip("How far will the player fall before uplift occurs")]
    public float upliftpoint = 40f;
    [Tooltip("How powerful the uplift will be")]
    public float upliftstrength = 0.5f;
    public bool Uplift = false;//uplift bool
    #endregion
    #region Cheats
    public GameObject Cheats;
    #endregion
    #endregion

    private void Awake () {
        #region Giving variable values
        floattimer = 10f;
        //TabManaging = GameObject.Find("TabManager").GetComponent<tabchanger>();
        storycheck = GameObject.Find("Avatar").GetComponent<StoryChecker>();
        Controller = transform.GetComponent<CharControl>();
        woosh = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        if (storycheck.DemoMode)
        {
            inventory = GameObject.FindGameObjectWithTag("itemui");
        } 
        #endregion
        #region setting variable status
        respawnertext.enabled = false;
        inventory.SetActive(false);

        Cursor.visible = false;
        #endregion
    }

    void Update ()
    {
        MenuTabToggle();
        StaminaBarChange();
        //UpliftPlayer();
        timeCount += Time.deltaTime;
        timeCountrounded = (int)timeCount;
    }

    private void MenuTabToggle()
    {
        if (Input.GetKeyDown(KeyCode.I)|| Input.GetKeyDown(KeyCode.Tab))//Inventory menu toggle via tab button
        {


            if (storycheck.DemoMode == true)
            {

                if (inventory.activeSelf == false)
                {
                    this.gameObject.GetComponent<CharControl>().sfxControlScript.InventoryOpen(); //play sound effect of opening the inventory
                    Controller.FreezeCam();
                    inventory.SetActive(true);
                    Cursor.visible = true;
                    info.enabled = true;

                }
                else
                {
                    this.gameObject.GetComponent<CharControl>().sfxControlScript.InventoryClose(); //play sound effect of opening the inventory
                    Controller.UnfreezeCam();
                    inventory.SetActive(false);
                    Cursor.visible = false;
                    info.enabled = false;

                }
            }
            else if(storycheck.DemoMode == false)
            {
                if (inventory2.activeSelf == false)
                {
                    this.gameObject.GetComponent<CharControl>().sfxControlScript.InventoryOpen(); //play sound effect of opening the inventory
                    Controller.FreezeCam();
                    inventory2.SetActive(true);
                    Cursor.visible = true;
                    tabs.SetActive(true);
                    notice.GetComponent<NoticeScript>().Close();
                    //info.enabled = true;

                }
                else
                {
                    this.gameObject.GetComponent<CharControl>().sfxControlScript.InventoryClose(); //play sound effect of opening the inventory
                    Controller.UnfreezeCam();
                    inventory2.SetActive(false);
                    Cursor.visible = false;
                    tabs.SetActive(false);
                    //info.enabled = false;

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.C)&& Input.GetKeyDown(KeyCode.T))
        {
            if (Cheats.activeSelf == false)
            {
                Cheats.SetActive(true);
                Cursor.visible = true;
                Controller.FreezeCam();
                Controller.sfxControlScript.ButtonIn();
            }
            else if (Cheats.activeSelf == true)
            {
                Cheats.SetActive(false);
                Cursor.visible = false;
                Controller.UnfreezeCam();
                Controller.sfxControlScript.ButtonOut();
            }
        }
    }

    private void pageturner()
    {
        if(inventory2.activeSelf == true)
        {

        }
    }

    void StaminaBarChange()
    {
        var tempColor = Controller.boostMetre.color;//right value is the colour of the boostmeter
        tempColor.a = Controller.boostMetre.fillAmount;//transparancy value = fillamount value (fillamount is 100 at highest)
        Controller.boostMetre.color = tempColor;
        boostMetrered.fillAmount = Controller.currentFuel / Controller.fuelCapacity;
    }

    void UpliftPlayer()
    {
        //uplift if player falls at this depth
        if (transform.position.y < -upliftpoint)
        {
            if (Uplift == false)
            {
                woosh.Play();
            }
            Uplift = true;
        }
        if (Uplift == true)
        {
            if (transform.position.y < 0)
            //Rigidbody only to be affected by impulse
            {
                Controller.isGliding = true;
                Controller.glider.SetActive(true);
                rb.AddForce(0, upliftstrength, 0, ForceMode.Impulse);
            }
            else
            {
                Uplift = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "sign")
        {
            signtext.enabled = true;
        }
        if (other.gameObject.tag == "note")
        {
            notetext.enabled = true;
        }
    }
    private void OnTriggerStay(Collider other)//this is where most of our work is done regarding our triggers
    {
        switch (other.gameObject.tag)
        {
            #region daynight mechanic
            case "bench":
                //Timing.alert.enabled = true;
                //if (changeok == true)
                //{
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        /*if (!Timing.isNight)
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
                        switch(daynight.currentTime)
                        {
                            case DayNightCycle.TimePeriod.Morning:
                                daynight.timeCount = 10;
                                //flowchart.ExecuteBlock("Daychange");
                                break;
                            case DayNightCycle.TimePeriod.Afternoon:
                                daynight.timeCount = 20;
                                //flowchart.ExecuteBlock("Daychange");
                                break;
                            case DayNightCycle.TimePeriod.Evening:
                                daynight.timeCount = 30;
                                //flowchart.ExecuteBlock("Daychange");
                                break;
                            case DayNightCycle.TimePeriod.Night:
                                daynight.timeCount = 40;
                                //flowchart.ExecuteBlock("Daychange");
                                break;
                        }*/
                    }
                //}
                switch (daynight.currentTime)
                {
                    case DayNightCycle.TimePeriod.Morning:
                        Timing.MorningTime();
                        break;
                    case DayNightCycle.TimePeriod.Afternoon:
                        Timing.DayTime();
                        break;
                    case DayNightCycle.TimePeriod.Evening:
                        Timing.EveningTime();
                        break;
                    case DayNightCycle.TimePeriod.Night:
                        Timing.NightTime();
                        break;
                }
                break;
            #endregion

            case "boundary":
                    respawnertext.enabled = true;
                    floattimer -= Time.deltaTime;//what counts down our time
                    inttimer = (Mathf.RoundToInt(floattimer));//round floats to ints for display purposes
                    respawnertext.text = (inttimer + " seconds to return to the demo area");
                    if (floattimer < 0.1)//disables timer text, resets timer and resets player's position to spawn
                    {
                        respawnertext.enabled = false;
                        floattimer = 10f;
                        transform.position = Controller.respawnPoint.position;
                    }
                break;

            case "sign":
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Controller.enabled = false;
                        signtext.enabled = false;
                        flowchart.ExecuteBlock(other.gameObject.name);//calls a fungus block with the same name as note and puts out at's description
                    }
                break;
            case "note":
                notetext.enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Controller.enabled = false;
                    notetext.enabled = false;
                    notepic.sprite = Resources.Load<Sprite>("notes/" + other.gameObject.name);
                    if (notepic.enabled == false)
                    {
                        notepic.enabled = true;
                        notetextexit.enabled = true;
                    }
                    else
                    {
                        notepic.enabled = false;
                        Controller.enabled = true;
                        notetextexit.enabled = false;
                    }
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
        case "bench":  
            Timing.alert.enabled = false;
                break;
            case "boundary":
            respawnertext.enabled = false;
            floattimer = 10f;
                break;
            case "sign":
            signtext.enabled = false;
                break;
            case "note":
            notetext.enabled = false;
                notepic.enabled = false;
                break;
    }
    }

    string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeText;
    }
}
