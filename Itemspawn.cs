using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemspawn : MonoBehaviour {

    //making into prefabs and making sure everything works

    #region variables
    #region Hierarchy Adjustable Stats
    [Tooltip("These are your items which will be spawned")]
    public GameObject[] itemchoice;//left adjustable
    [Tooltip("These are the chances that they will spawn. Make sure that they match the items above according to order.")]
    public int[] spawnchance;//left adjustable
    [Tooltip("This is how long it will take the item to spawn. Make sure these match the items too")]
    public int[] spawnback;
    [Tooltip("Use this to adjust the height of the spawn point")]
    public float[] spawnheight;
    public float[] spawnrotation;
    #endregion
    private CharControl2 daytracker;//Getting our script to reference for days passing
    public GameObject spawneditem;//currentspawneditem
    private Transform itemmarker;//item location
    private int select;//this is which element is picked (order)
    private int randValue;//this is for random generation
    public int daysleft = 0;
    private Vector3 spawnpoint;
    public bool doublespawncheck = true;
    private bool randomroll = false;//prevents repeat of dice rolling
    #endregion

    void Start () {
        daytracker = GameObject.Find("Avatar").GetComponent<CharControl2>();
        #region setting chances
        for (int x = 1; x < spawnchance.Length; x++)//setting the spawn chance values for the script
        {
            spawnchance[x] = spawnchance[x] + spawnchance[x - 1];//taking the values put in by others and stacking the values together per element
        }
        #endregion
        #region setting spawning location
        itemmarker = this.gameObject.transform.GetChild(0);//getting the location for spawned item
        //spawnpoint = itemmarker.transform.position;//setting the location for spawned item
        //spawnpoint.y += spawnheight;//adjusting the height
        #endregion
        #region setting up prefabs
        for (int x = 0; x < itemchoice.Length; x++)//putting the prefabs in
        {
            spawnpoint = itemmarker.transform.position;
            spawnpoint.y += spawnheight[x];
            itemchoice[x] = Instantiate(itemchoice[x], spawnpoint, Quaternion.Euler(-90 + spawnrotation[x], 0, 0)) as GameObject;//this sets our variables to the instances(copies of prefabs)
            itemchoice[x].SetActive(false);
            itemchoice[x].name = itemchoice[x].name.Replace("(Clone)", "").Trim();//cuts the names of the instances(itemchoice objects). Gets rid of the clone part
        }
        #endregion
        #region initial item
        random();//diceroller
        spawneditem = itemchoice[select];//just setting the starting items
        spawneditem.SetActive(true);
        #endregion
    }

    public void random()//randomises and selects our current item to be spawned while setting it to active
    {
        randValue = Random.Range(0, 100);
        if (randValue < spawnchance[0])//whatever chance its sets to
        {
            select = 0;
        }
        else if (randValue < spawnchance[1])//whatever chance its sets to minus previous number
        {
            select = 1;
        }
        else if (randValue < spawnchance[2])//whatever chance its sets to minus previous number
        {
            select = 2;
        }
        else if (randValue < spawnchance[3])//whatever chance its sets to minus previous number
        {
            select = 3;
        }
        else if (randValue < spawnchance[4])//whatever chance its sets to minus previous number
        {
            select = 4;
        }
        else if (randValue < spawnchance[5])//whatever chance its sets to minus previous number
        {
            select = 5;
        }
        else if (randValue < spawnchance[6])//whatever chance its sets to minus previous number
        {
            select = 6;
        }
        else if (randValue < spawnchance[7])//whatever chance its sets to minus previous number
        {
            select = 7;
        }
        else if (randValue < spawnchance[8])//whatever chance its sets to minus previous number
        {
            select = 8;
        }
        else if (randValue < spawnchance[9])//whatever chance its sets to minus previous number
        {
            select = 9;
        }
        else if (randValue < spawnchance[10])//whatever chance its sets to minus previous number
        {
            select = 10;
        }
        else//rest of percent, e.g 10% chance
        {
            select = 11;//currently our max is 11 items to be spawned
        }
        randomroll = false;//to prevent random selection from going on every night
    }
    
    public void spawnstatus()
    {
        if (spawneditem.activeSelf == false)//to check if there is no item currently available
        {
            if (daytracker.spawncheck == true)//turning to day, first gate from charcontrol2, we can't manipulate this as it would affect ALL items being spawned back
            {
                if (doublespawncheck == true)//second gate, this is the one we can manipulate
                {
                    daysleft++;//adds to our time to spawnback
                    if (randomroll == true)
                    {
                        random();//all we have to do is seperate this and make sure it only runs once
                    }
                    else if (daysleft >= spawnback[select])
                    {
                        spawneditem = itemchoice[select];
                        spawneditem.SetActive(true);
                        daysleft = 0;
                        randomroll = true;
                    }
                    doublespawncheck = false;
                }
            }
            else if(daytracker.spawncheck == false)//turning to night
            {
                if (doublespawncheck == false)//second gate, this is the one we can manipulate
                {
                    if (randomroll == true)
                    {
                        random();
                    }
                    if (daysleft >= spawnback[select])
                    {
                        spawneditem = itemchoice[select];
                        spawneditem.SetActive(true);
                        daysleft = 0;
                        randomroll = true;
                    }
                        doublespawncheck = true;
                }
            }
        }
    }
   
    void Update () {
        spawnstatus();
        }
}
