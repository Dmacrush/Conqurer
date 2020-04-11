using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA

{



    public class ItemSwtiching : MonoBehaviour
    {
        public int SelectedItems = 0;



        public void Start()
        {

            SelectItems();


        }


        private void Update()
        {


            int PreviousSelectedItems = SelectedItems;
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (SelectedItems >= transform.childCount - 1)
                SelectedItems = 0;
                else
                SelectedItems++;

            }




            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (SelectedItems <= 0)
                    SelectedItems = transform.childCount - 1;

                else
                    SelectedItems--;

            }


            if(PreviousSelectedItems != SelectedItems)
            {
                SelectItems();
            }

        }


        public void SelectItems()
        {
            int i = 0;

            foreach (Transform item in transform)
            {
                if (i == SelectedItems)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
                
                i++;

            }
        }



    }
}