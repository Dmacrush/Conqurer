using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SA

{



    public class EquimetsManager : MonoBehaviour
    {

        public bool[] IsFull;
        public GameObject[] slots;
        public GameObject ItemImage1;
        public GameObject ItemImage2;




        public void ItemSelection()
        {

            for (int i = 0; i < slots.Length; i++)
            {
                if (IsFull[i] == false)
                {

                    Instantiate(ItemImage1, slots[i].transform, false);
                    IsFull[i] = true;
                    break;
                }

            }

        }



        public void ItemSelection2()
        {

            for (int i = 0; i < slots.Length; i++)
            {
                if (IsFull[i] == false)
                {

                    Instantiate(ItemImage2, slots[i].transform, false);
                    IsFull[i] = true;
                    break;
                }

            }

        }


    }


    }
