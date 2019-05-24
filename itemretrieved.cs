using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GEAR;

public class itemretrieved : MonoBehaviour {

    public Text Pickuptext;
    public Image PickupImage;
    public Itemslots dagear;//to reference for our inventory
    public AchievementHunter itemchecker;//to check on our achievements
    public StoryChecker storychecker;
    string pickedname;
    public bool itemok = false;
    public MilestoneRankManager ranker;
    [Space(10)]
    public SfxControl sfxControlScript;
    public Text pickedUpNameText;
    public CharControl charControlScript;
    //public ItemRegistry registeritem;

    public void Awake()
    {
        itemok = false;
        ranker = GameObject.Find("Avatar").GetComponent<MilestoneRankManager>();
        storychecker = GameObject.Find("Avatar").GetComponent<StoryChecker>();
        Pickuptext = GameObject.Find("Item Pickup").GetComponent<Text>();
        dagear = GameObject.Find("Avatar").GetComponent<Itemslots>();
        itemchecker = GameObject.Find("Avatar").GetComponent<AchievementHunter>();
        sfxControlScript = GameObject.Find("SFXControl").GetComponent<SfxControl>();
        pickedUpNameText = GameObject.Find("PickedUpText").GetComponent<Text>();
        PickupImage = GameObject.Find("PickUp").GetComponent<Image>();//update
        charControlScript = GameObject.Find("Avatar").GetComponent<CharControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            itemok = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            itemok = true;
            itemok = false;//first measure to turn off detection (for when the player leaves the detection radius of the item)
            //Pickuptext.enabled = false;
            PickupImage.enabled = false;
        }
    }

    public void Update()
    {
        if (itemok == true)
        {
            pickedname = this.name;
            //Pickuptext.text = "Press E to pickup " + pickedname;//.text accesses the actual text of the text, pretty rad
            //Pickuptext.enabled = true;
            PickupImage.enabled = true;
            if (Input.GetButtonDown("talk"))
            {
                if (charControlScript.storedItem == null)
                {
                    //(Kris)
                    charControlScript.storedItem = this.gameObject;
                    #region pick up SFX
                    sfxControlScript.PickUpPlantSFX();
                    pickedUpNameText.text = "Picked Up [" + "<b>" + pickedname + "</b>" + "]";

                    pickedUpNameText.GetComponent<Animator>().Play("PickedUpTextAnim", -1, 0f);

                    /*
                    //order: plant, gem, ore
                    if (!pickedname.Contains("Diamond")
                        || !pickedname.Contains("Amethyst")
                        || !pickedname.Contains("Ruby")
                        || !pickedname.Contains("Sapphire")
                        || !pickedname.Contains("Bronze")
                        || !pickedname.Contains("Iron")
                        || !pickedname.Contains("Steel")
                        || !pickedname.Contains("Gold"))
                    {
                        sfxControlScript.PickUpPlantSFX();
                        print("Plant");
                    }

                    if (pickedname.Contains("Diamond")
                        || pickedname.Contains("Amethyst")
                        || pickedname.Contains("Ruby")
                        || pickedname.Contains("Sapphire"))
                    {
                        sfxControlScript.PickUpGemSFX();
                        print("gem");
                    }

                    if (pickedname.Contains("Bronze")
                        || !pickedname.Contains("Iron")
                        || !pickedname.Contains("Steel")
                        || !pickedname.Contains("Gold"))
                    {
                        sfxControlScript.PickUpOreSFX();
                        print("ore");
                    }*/

                    #endregion
                    print("item: " + pickedname);
                    Debug.Log("item taken");
                    if (storychecker.DemoMode == false)
                    {
                        ranker.itemadded(pickedname);
                    }
                    else
                    {
                        dagear.ItemAdded(pickedname);
                    }
                    ranker.itemscollected++;
                    itemchecker.Itemcheck();//this is for checking if achievements have been reached
                    gameObject.SetActive(false);
                    //Pickuptext.enabled = false;
                    PickupImage.enabled = false;
                    itemok = false;//second measure to turn off detection
                }
            }
        }
    }

    /*public void itemadded(string itemname)
    {
        for(int x = 0; x < registeritem.items.Count; x ++)
        {
            if(itemname == registeritem.items[x].ItemName)
            {
                registeritem.items[x].currentamount++;
            }
        }
    }*/
}
