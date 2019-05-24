using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//heart increase ui, hidden gems, heart point round ups
public class MenuHandler : MonoBehaviour
{

    public StorySystem STORY;
    public MilestoneList itemmilestonelist;
    public ItemRegistry itemstats;
    public BundleList bundles;
    public CharacterList Characters;
    private MilestoneRankManager ranker;
    public Transform pagehandler;
    public Transform leftarrow;
    public Transform rightarrow;
    public int currentpage = 0;
    public Transform[] pages;
    public Transform itemsprites;
    public Transform RankUpTextHolder;
    public Transform InventoryTextHolder;
    public Transform RankUpTextHolder2;
    public Transform InventoryTextHolder2;
    public Transform ItemButtonHolder;
    public Transform[] Covers;
    public Text[] ItemPerm;
    public Text[] ItemTemp;
    public Text[] ItemPerm2;
    public Text[] ItemTemp2;
    public Button[] ItemPermButtons;//this new for buttons
    public Button[] HandInButtons;
    public int itemselect = 0;
    private int itemselect2 = 0;
    private int selector = -3;
    private int selectorrankup = -1;
    public int[] bundlesoff = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // Use this for initialization
    void Start()
    {
        STORY = GameObject.Find("StorySystemObject").GetComponent<StorySystem>();
        ranker = GameObject.Find("Avatar").GetComponent<MilestoneRankManager>();
        pagehandler = this.gameObject.GetComponentInChildren<Transform>().GetChild(0);
        leftarrow = this.gameObject.GetComponentInChildren<Transform>().GetChild(1);
        rightarrow = this.gameObject.GetComponentInChildren<Transform>().GetChild(2);
        pages = pagehandler.transform.GetComponentsInChildren<Transform>();
        RankUpTextHolder = this.gameObject.GetComponentInChildren<Transform>().GetChild(3);
        InventoryTextHolder = this.gameObject.GetComponentInChildren<Transform>().GetChild(4);
        RankUpTextHolder2 = this.gameObject.GetComponentInChildren<Transform>().GetChild(5);
        InventoryTextHolder2 = this.gameObject.GetComponentInChildren<Transform>().GetChild(6);
        itemsprites = this.gameObject.GetComponentInChildren<Transform>().GetChild(7);
        ItemButtonHolder = this.gameObject.GetComponentInChildren<Transform>().GetChild(8);

        ItemPerm = RankUpTextHolder.GetComponentsInChildren<Text>();
        ItemTemp = InventoryTextHolder.GetComponentsInChildren<Text>();
        ItemPerm2 = RankUpTextHolder2.GetComponentsInChildren<Text>();
        ItemTemp2 = InventoryTextHolder2.GetComponentsInChildren<Text>();
        ItemPermButtons = ItemButtonHolder.gameObject.transform.GetComponentsInChildren<Button>();
        pageturner(true);
    }

    void Update()
    {
        UIsetting();
        //disablebundle();
    }

    private void disablebundle()
    {
        for(int x = 0; x < Covers.Length; x++)
        {
            if(bundlesoff[x] == 1)
            {
                Covers[x].gameObject.SetActive(true);
                HandInButtons[(x + 2) / 3].gameObject.SetActive(false);
            }
        }
    }

    public void pageturner(bool whichway)
    {
        if (currentpage != 0)//for the item sprite images
        {
            itemsprites.GetComponentInChildren<Transform>().GetChild(currentpage - 1).gameObject.SetActive(false);
        }
        for (int x = 1; x < pagehandler.transform.childCount + 1; x++)
        {
            pages[x].gameObject.SetActive(false);
        }
        if (whichway == true)//for turning the page with the right arrow
        {
            currentpage++;
        }
        else if (whichway == false)//for turning the page with the left arrow
        {
            currentpage--;
        }
        if (currentpage == 1)//for disabling the left arrow when on the first page
        {
            leftarrow.gameObject.SetActive(false);
        }
        else
        {
            leftarrow.gameObject.SetActive(true);
        }
        if (currentpage == pagehandler.transform.childCount)//for disabling the right arrow when on the end page
        {
            rightarrow.gameObject.SetActive(false);
        }
        else
        {
            rightarrow.gameObject.SetActive(true);
        }
        pages[currentpage].gameObject.SetActive(true);
        itemsprites.GetComponentInChildren<Transform>().GetChild(currentpage - 1).gameObject.SetActive(true);
        UItexttoggle(currentpage);
    }

    public void UIsetting()
    {
        for (int y = 0; y < ItemPerm.Length; y++)
        {
            if (itemmilestonelist.milestones[y].currentrank < 3)
            {
                ItemPerm[y].text = itemstats.items[y].currentamount + "/" + itemmilestonelist.milestones[y].MilestoneRequirements[itemmilestonelist.milestones[y].currentrank];
                if (itemstats.items[y].currentamount >= itemmilestonelist.milestones[y].MilestoneRequirements[itemmilestonelist.milestones[y].currentrank])
                {
                    ItemPerm[y].text = "<color=#800000ff> Rank Up </color>";
                    ItemPermButtons[y].gameObject.SetActive(true);
                    if (selectorrankup > -1)
                    {
                        itemstats.items[selectorrankup].currentamount -= itemmilestonelist.milestones[y].MilestoneRequirements[itemmilestonelist.milestones[y].currentrank];
                        ranker.TotalCP += itemmilestonelist.milestones[y].CP[itemmilestonelist.milestones[y].currentrank];//Increasing the CP
                        itemmilestonelist.milestones[y].currentrank += 1;
                        selectorrankup = -1;
                    }
                }
                else
                {
                    ItemPermButtons[y].gameObject.SetActive(false);
                }
            }
            else//max rank achieved
            {
                ItemPerm[y].text = "<color=#800000ff> MAX RANK </color>";
            }
            ItemTemp[y].text = itemstats.items[y].currentamount.ToString();
        }

        for (int y = 0; y < ItemPerm2.Length; y++)//to go through each ui text element. This is whats being assigned
        {
            bool uncheck = true;
            for (int x = 0; x < bundles.bundles.Count; x++)//to go through all the bundles
            {
                if (ItemPerm2[y].gameObject.name == bundles.bundles[x].CharacterName)//to assign bundle elements
                {
                    for (int z = 0; z < itemstats.items.Count; z++)//to go through all items
                    {
                        if (uncheck == true)
                        {
                            if (itemstats.items[z].ItemName == bundles.bundles[x].ItemName[itemselect])
                            {
                                ItemPerm2[y].text = itemstats.items[z].currentamount + "/" + bundles.bundles[x].ItemRequirements[itemselect];//no wonder it wasn't working, your item requirements area in the bundles asset is empty!
                                ItemTemp2[y].text = itemstats.items[z].currentamount.ToString();
                                if (itemstats.items[z].currentamount > bundles.bundles[x].ItemRequirements[itemselect] - 1)
                                {
                                    ItemPerm2[y].gameObject.GetComponent<Button>().enabled = true;
                                    if (y == selector)
                                    {
                                        itemstats.items[z].currentamount -= bundles.bundles[x].ItemRequirements[itemselect];
                                    }
                                    if (y == selector + 1)
                                    {
                                        itemstats.items[z].currentamount -= bundles.bundles[x].ItemRequirements[itemselect];
                                    }
                                    if (y == selector + 2)
                                    {
                                        itemstats.items[z].currentamount -= bundles.bundles[x].ItemRequirements[itemselect];
                                        ranker.TotalCP += 5;//CP increasing
                                        HandInButtons[y / 3].gameObject.SetActive(false);
                                        Covers[selector / 3].gameObject.SetActive(true);
                                        bundlesoff[selector / 3] = 1;
                                        characterheartincrease(bundles.bundles[x].CharacterName, x);
                                        //STORY.heartsystem();
                                        selector = -1;
                                    }
                                }
                                else
                                {
                                    ItemPerm2[y].gameObject.GetComponent<Button>().enabled = false;
                                }
                                itemselect++;
                                if (itemselect == 3)
                                {
                                    itemselect = 0;
                                    //Debug.LogError("RESET" + itemselect);
                                }
                                uncheck = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void characterheartincrease(string charactername, int bundlenumber)
    {
        for (int ccount = 0; ccount < Characters.characters.Count; ccount++)
        {
            if (bundles.bundles[bundlenumber].CharacterName == Characters.characters[ccount].CharacterName)
            {
                Characters.characters[ccount].HeartEXP += 70;
            }
        }
    }
    public void RankUp(int chosen)
    {
        selectorrankup = chosen;
        Debug.LogError("Text");
    }

    public void UItexttoggle(int page)
    {
        switch (page)
        {
            case 1:
                SetAllToFalse();
                for (int x = 0; x < 13; x++)
                {
                    ItemPerm[x].gameObject.SetActive(true);
                    ItemTemp[x].gameObject.SetActive(true);
                    //ItemPermButtons[x].gameObject.SetActive(true);
                }
                break;
            case 2:
                SetAllToFalse();
                for (int x = 13; x < 33; x++)
                {
                    ItemPerm[x].gameObject.SetActive(true);
                    ItemTemp[x].gameObject.SetActive(true);
                    //ItemPermButtons[x].gameObject.SetActive(true);
                }
                break;
            case 3:
                SetAllToFalse();
                for (int x = 33; x < ItemPerm.Length; x++)
                {
                    ItemPerm[x].gameObject.SetActive(true);
                    ItemTemp[x].gameObject.SetActive(true);
                    //ItemPermButtons[x].gameObject.SetActive(true);
                }
                break;
            case 4:
                SetAllToFalse();
                for (int x = 0; x < 18; x++)
                {
                    ItemPerm2[x].gameObject.SetActive(true);
                    ItemTemp2[x].gameObject.SetActive(true);
                }
                break;
            case 5:
                SetAllToFalse();
                for (int x = 18; x < ItemPerm2.Length; x++)
                {
                    ItemPerm2[x].gameObject.SetActive(true);
                    ItemTemp2[x].gameObject.SetActive(true);
                }
                break;
            default:
                SetAllToFalse();
                break;
        }
    }

    public void SetAllToFalse()
    {
        for (int x = 0; x < ItemPerm.Length; x++)
        {
            ItemPerm[x].gameObject.SetActive(false);
            ItemTemp[x].gameObject.SetActive(false);
            //ItemPermButtons[x].gameObject.SetActive(false);
        }
        for (int x = 0; x < ItemPerm2.Length; x++)
        {
            ItemPerm2[x].gameObject.SetActive(false);
            ItemTemp2[x].gameObject.SetActive(false);
        }
    }

    public void ButtonwOrk()
    {
        Debug.LogError("pls button ");
    }
    public void HandIn(int three)
    {
        Debug.LogError("buttonworks0");
        switch (three)
        {
            case 0:
                Repeater(three, 0);
                break;
            case 3:
                Repeater(three, 1);
                break;
            case 6:
                Repeater(three, 2);
                break;
            case 9:
                Repeater(three, 3);
                break;
            case 12:
                Repeater(three, 4);
                break;
            case 15:
                Repeater(three, 5);
                break;
            case 18:
                Repeater(three, 6);
                break;
            case 21:
                Repeater(three, 7);
                break;
            case 24:
                Repeater(three, 8);
                break;
            case 27:
                Repeater(three, 9);
                break;
            default:
                break;
        }
    }

    public void Repeater(int checks, int cover)
    {
        Debug.LogError("buttonworks");
        //ItemPerm2[27].GetComponent<Button>().enabled = false;
        if (currentpage != 0)//(ItemPerm2[checks].GetComponent<Button>().enabled == true)
        {
            Debug.LogError("buttonworks2");
            if (ItemPerm2[checks + 1].GetComponent<Button>().enabled == true)
            {
                Debug.LogError("buttonworks3");
                if (ItemPerm2[checks + 2].GetComponent<Button>().enabled == true)
                {
                    selector = checks;
                    Debug.LogError("buttonworks4");
                    //bundleComplete Occurs
                }
            }
        }
    }
}
