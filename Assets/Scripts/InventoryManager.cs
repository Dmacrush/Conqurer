using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SA
{


    public class InventoryManager : MonoBehaviour
    {



        public bool IsBack = false;

        public GameObject Inventory;
        public GameObject SkillsInv;
        public GameObject EQUIMENTSINV;
        public GameObject OptionInv;
        public GameObject ItemsInv;
        public GameObject ConboInv;




        public void Start()
        {



            Inventory.SetActive(false);

        }

        public void Update()
        {
            
         if(Input.GetKeyUp(KeyCode.Escape))
            {
                if(IsBack == true)
                {
                    IsBack = false;
                } else
                {
                    IsBack = true;
                }
            }
         

         if(IsBack == true)
            {
                Inventory.SetActive(true);
                Time.timeScale = 0f;

            } else
            {

                Inventory.SetActive(false);
                Time.timeScale = 1f;
            }

        }

        public void Skills()
        {
            Inventory.SetActive(false);
            SkillsInv.SetActive(true);

        }


        public void Equiments()
        {


            Inventory.SetActive(false);
            EQUIMENTSINV.SetActive(true);
        }


        public void Options()
        {

            Inventory.SetActive(false);
            OptionInv.SetActive(true);


        }


        public void GoBack()
        {

            Inventory.SetActive(true);
            OptionInv.SetActive(false);
            EQUIMENTSINV.SetActive(false);
            SkillsInv.SetActive(false);
            ItemsInv.SetActive(false);
            ConboInv.SetActive(false);

        }


        public void Items()
        {

            Inventory.SetActive(false);
            OptionInv.SetActive(false);
            EQUIMENTSINV.SetActive(false);
            SkillsInv.SetActive(false);
            ItemsInv.SetActive(true);
            ConboInv.SetActive(false);
        }



        public void Combo()
        {

            Inventory.SetActive(false);
            OptionInv.SetActive(false);
            EQUIMENTSINV.SetActive(false);
            SkillsInv.SetActive(false);
            ItemsInv.SetActive(false);
            ConboInv.SetActive(true);
        }

    }
}