using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneRankManager : MonoBehaviour {
    //last thing to do, saving cp and data maybe
    [Tooltip("Tick this to reset the ranks to 0. Then click play and then click again to exit game mode. Then untick this box and the ranks should be reset")]
    public bool TickThisToResetRanks;
    [Space(10)]

    [Tooltip("Just for testing purposes")]
    //public Text rankui;
    public CharControl Controller;
    public MilestoneList milestonelist;
    public MilestoneList itemmilestonelist;
    public ItemRegistry itemstats;
    public MenuHandler menuhandler;
    public UI_Manager ui_managerScript;
    //we'll just make another milestonelist for items, below code should work. Will keep track of our item count and rank requirements.
    [Space(5)]
    public int TotalCP;//This is the value you should edit
    [Space (5)]
    public string flytime;
    public int secondscount;
    private float timecounter = 0f;
    [Space(5)]
    public string time;
    public int secondscount2;
    private float timecounter2 = 0f;
    [Space(5)]
    public int itemscollected = 0;

    public Text[] ItemPerm;
    public Text[] ItemTemp;
    public GameObject[] itemsboi;

    public void Start()
    {
        //menuhandler = GameObject.Find("Menu2").GetComponent<MenuHandler>();
        for (int x = 0; x < itemmilestonelist.milestones.Count; x++)//NOTE : x is only for counting through our individual milestones, 
        {
            //itemmilestonelist.milestones[x].MilestoneRequirements = new int[3];
            itemmilestonelist.milestones[x].MilestoneName = itemstats.items[x].ItemName;
            itemmilestonelist.milestones[x].MilestoneRequirements = new int[3];
            itemmilestonelist.milestones[x].CP = new int[3];
            switch (itemstats.items[x].stars)//sets item stats
                {
                case Item.rarity.one:
                    for (int y = 0; y < 3; y++)
                    {
                        int[] onestar = { 20, 25, 30 };//sets requirements
                        itemmilestonelist.milestones[x].MilestoneRequirements[y] = onestar[y];
                        int[] onestarcp = { 1, 2, 3 };
                        itemmilestonelist.milestones[x].CP[y] = onestarcp[y];
                    }
                    break;
                case Item.rarity.two:
                    for (int y = 0; y < 3; y++)
                    {
                        int[] twostar = { 10, 15, 20 };
                        itemmilestonelist.milestones[x].MilestoneRequirements[y] = twostar[y];
                        int[] twostarcp = { 1, 2, 3 };
                        itemmilestonelist.milestones[x].CP[y] = twostarcp[y];
                    }
                    break;
                case Item.rarity.three:
                    for (int y = 0; y < 3; y++)
                    {
                        int[] threestar = { 3, 6, 9 };
                        itemmilestonelist.milestones[x].MilestoneRequirements[y] = threestar[y];
                        int[] threestarcp = { 1, 2, 3 };
                        itemmilestonelist.milestones[x].CP[y] = threestarcp[y];
                    }
                    break;
                }
            
        }
    }

    public void itemadded(string itemname)
    {
        for (int x = 0; x < itemstats.items.Count; x++)
        {
            if (itemname == itemstats.items[x].ItemName)
            {
                itemstats.items[x].statamount++;
                itemstats.items[x].currentamount++;
                //ranktracker2(itemname, itemstats.items[x].currentamount);
            }
        }
    }

    public void UIGathering()
    {
        for (int x = 0; x < 14; x++)
        {
            ItemPerm[x] = GameObject.FindGameObjectWithTag("ItemPermanent").GetComponent<Text>();
            ItemTemp[x] = GameObject.FindGameObjectWithTag("ItemTemporary").GetComponent<Text>();
            //itemsboi[x] = GameObject.FindGameObjectWithTag("ItemPermanent");
        }
        //itemsboi = GameObject.FindGameObjectsWithTag("ItemPermanent");
        Debug.LogError("tis working yeah man");
        //ItemPerm = GameObject.FindGameObjectsWithTag("ItemPermanent");
    }

    public void ItemUIstats()
    {
        UIGathering();
        //Debug.LogError("tis working yeah man");
        for (int x = 0; x < itemmilestonelist.milestones.Count; x ++)
        {
            for (int y = 0; y < ItemPerm.Length; y ++)
            {
                if(ItemPerm[y].GetComponentInParent<GameObject>().name == itemmilestonelist.milestones[x].MilestoneName)
                {
                    ItemPerm[y].text = itemstats.items[x].currentamount + "/" + itemmilestonelist.milestones[x].MilestoneRequirements;
                    ItemTemp[y].text = itemstats.items[x].statamount.ToString();
                    //Text tempchanger = ItemPerm[y].GetComponent<Text>();
                    //tempchanger.text = itemstats.items[x].ItemName + "/" + itemmilestonelist.milestones[x].MilestoneRequirements;
                }
            }
        }
    }

    private void Update()
    {
        //ItemPerm = GameObject.FindGameObjectsWithTag("ItemPermanent");
        //rankui.text = "Gliding Time Rank : " + milestonelist.milestones[0].currentrank + "\n" + "Items Collection Rank : " + milestonelist.milestones[1].currentrank + "\n" + "Time Played Rank : " + milestonelist.milestones[2].currentrank;
        ItemsCollectedRank();
        GlidingTimeRank();
        TimePlayedRank();
        RankReset();
    }

    //to avoid duplicating code we can just have a milestonelist variable that holds the current milestone being called? What if two are called at the same time tho?
    public void ranktracker(string milestonename,  int tracker)
    {
        for(int x = 0; x < milestonelist.milestones.Count; x++)//NOTE : x is only for counting through our individual milestones, 
        {
            if (milestonelist.milestones[x].MilestoneName == milestonename)//checks our milestone for it's name
            {
                for (int y = 0; y < milestonelist.milestones[x].MilestoneRequirements.Length; y++)//goes through all our milestone rankings
                {
                    if (tracker == milestonelist.milestones[x].MilestoneRequirements[y])//checks which milestone ranking our current value is equal to by going through them all
                    {
                        if (y == milestonelist.milestones[x].currentrank)
                        {
                            TotalCP += milestonelist.milestones[x].CP[y];//Adds to our CP from our CP selection
                            ui_managerScript.Achievementdetermined(milestonelist.milestones[x].MilestoneName, milestonelist.milestones[x].CP[y], milestonelist.milestones[x].MilestoneRequirements[milestonelist.milestones[x].currentrank]); //activate and set notifications
                            milestonelist.milestones[x].currentrank += 1;//increases our rank
                        }
                    }
                }
            }
        }
    }

    /*public void ranktracker2(string milestonename, int tracker)
    {
        for (int x = 0; x < itemmilestonelist.milestones.Count; x++)//NOTE : x is only for counting through our individual milestones, 
        {
            if (itemmilestonelist.milestones[x].MilestoneName == milestonename)//checks our milestone for it's name
            {
                for (int y = 0; y < itemmilestonelist.milestones[x].MilestoneRequirements.Length; y++)//goes through all our milestone rankings
                {
                    if (tracker == itemmilestonelist.milestones[x].MilestoneRequirements[y])//checks which milestone ranking our current value is equal to by going through them all
                    {
                        if (y == itemmilestonelist.milestones[x].currentrank)
                        {
                            TotalCP += itemmilestonelist.milestones[x].CP[y];//Adds to our CP from our CP selection
                            itemmilestonelist.milestones[x].currentrank += 1;//increases our rank
                        }
                    }
                }
            }
        }
    }*/

    private void GlidingTimeRank()
    {
        if (Controller.isGliding == true)
        {
            timecounter += Time.deltaTime;
            flytime = FormatTime(timecounter);
            secondscount = (int)timecounter;
        }
        ranktracker("Gliding Time", secondscount);
    }

    string FormatTime (float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeText;
    }

    private void ItemsCollectedRank()
    {
        ranktracker("Items Collected", itemscollected);
    }

    private void TimePlayedRank()
    {
        timecounter2 += Time.deltaTime;
        time = FormatTime(timecounter2);
        secondscount2 = (int)timecounter2;
        ranktracker("Time Played", secondscount2);
    }

    public void RankReset()
    {
        if (TickThisToResetRanks == true)
        {
            for (int x = 0; x < milestonelist.milestones.Count; x++)
            {
                milestonelist.milestones[x].currentrank = 0;
            }
            for (int y = 0; y < itemmilestonelist.milestones.Count; y++)
            {
                itemstats.items[y].statamount = 0;
                itemstats.items[y].currentamount = 0;
                itemmilestonelist.milestones[y].currentrank = 0;
            }
        }
    }

    }
