using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA

{



    public class ScoreManager : MonoBehaviour
    {
        [Header("Pallet Score Count")]
        public Text Itemtext;
        public int ItemNUmber;
        public Text ItemHudtext;
        public bool Iscounting;




        private void Update()
        {


            Itemtext.text = ItemNUmber.ToString();
            ItemHudtext.text = ItemNUmber.ToString();

        }

        public void IncreaseScore()
        {

                ItemNUmber += 1;
            Itemtext.text = "" + ItemNUmber;



        }


    }
}