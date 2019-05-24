using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ITEMS;

//the only important thing about naming items right now is that they match the name of a sprite in the resources folder.

namespace GEAR
{
    public class Itemslots : MonoBehaviour
    {
        #region Variables

        #region Lists and arrays
        public List<Itemslot> Slots = new List<Itemslot>();//this stores our item name and order values, also used as our checker
        public List<Image> UIimages = new List<Image>();//holds our ui images which we are going to affect
        public Image[] UIbackup = new Image[18];//this rematches the order of elements with the correct order of ui images. 
        public GameObject[] erasers = new GameObject[18];//just for x button appearing and reappearing
        public List<int> removeditems = new List<int>();//to keep track of our order of removed items
        #endregion

        #region other variables
        int slotnumber = 0;//to keep track of which slot to fill next(unless items have been removed, those must be filled first)
        string currentitemA;//to temporarily hold onto our picked up item name
        public inventory ouritems;//i might use this in future for item name selection in the hierarchy
        #endregion

        #region Switcher Variables
        //this is for item switching and these values will be holding variables which will be switched
        int changer1;
        int changer2;
        string changername1;
        string changername2;
        Sprite changerpic1;
        Sprite changerpic2;

        bool switching = false;
        #endregion

        #endregion

        public void ItemAdded(string currentitem)//might tweak so that it pulls from the list of items instead.
        {
            if (removeditems.Count > 0)//this is checking if there's any refill spots left by removed items first
            {
                UIimageSet(currentitem);
                Slots[removeditems[0]] = new Itemslot(currentitem, removeditems[0]);//makes the removed item area the item
                removeditems.RemoveAt(0);//got to remove that place now since its filled and to move onto the next place
            }
            else
            {
                if (slotnumber < 9)//fully putting item into registry unless full
                {
                    UIimageSet(currentitem);
                    Slots.Add(new Itemslot(currentitem, slotnumber));
                    slotnumber += 1;
                }
                else
                {
                    Debug.Log("Your inventory is full");
                }
            }
        }

        void UIimageSet(string currentitem)
        {
            currentitemA = currentitem;
            if (removeditems.Count > 0)//for removed items
            {
                imagesetting(removeditems[0]);
            }
            else//normal imagesetting
                {
                imagesetting(slotnumber);
            }
        }
        private void imagesetting(int placement)
        {
            UIimages[placement] = UIbackup[placement];//need this to rematch the ui array elements back to correct order
            UIimages[placement].enabled = true;
            UIimages[placement].sprite = Resources.Load<Sprite>("items/" + currentitemA);//Resources is honestly class, can just load directly from your assets as long as the name matches
            erasers[placement].SetActive(true);
        }

        public void itemremoved(int binitem)
        {
            removeditems.Add(binitem);
            UIimages[binitem].enabled = false;//is this necessary?
            UIimages[binitem] = null;
            Slots[binitem] = null;
            erasers[binitem].SetActive(false);
        }

        #region this is to be edited in the future
        /*public void itemreorganised()
        {

        }*/
        #endregion

        public void itemswitched(int switcher)
        {
            if(switching == false)//this is for first item selected
            {
                #region getting all of first items attributes

            changer1 = switcher;
                changername1 = Slots[switcher].itemname;
            changerpic1 = UIimages[changer1].sprite;

#endregion
                switching = true;
            }
            else//this is for second item selected
            {
                #region getting all of second items attributes

                changer2 = switcher;
                changername2 = Slots[switcher].itemname;
            changerpic2 = UIimages[changer2].sprite;

                #endregion
                switching = false;
                #region Switching all values

                Slots[changer1].order = changer2;
          Slots[changer2].order = changer1;
                Slots[changer1].itemname = changername2;
                Slots[changer2].itemname = changername1;
          UIimages[changer1].sprite = changerpic2;
          UIimages[changer2].sprite = changerpic1;
                #endregion
            }
        }
         
    }
}
