using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SA

{
     


    public class SkillInventoryHud : MonoBehaviour
    {

        public Image SkillPoint;
        public Text SkillTetx;

        public void Update()
        {

            SkillTetx.text = FindObjectOfType<PlayerStats>().PlayerSkillPoints.ToString();
            SkillPoint.fillAmount = FindObjectOfType<PlayerStats>().SKillLengths;


        }

    }
}