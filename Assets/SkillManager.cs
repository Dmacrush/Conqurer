using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace SA

{

    public class SkillManager : MonoBehaviour
    {

        [Header("Chakra")]
        public Button Chakra1;
        public Button Chakra2;
        public Button Chakra3;
        public Button Chakra4;
        public Button Chakra5;
        public Button Chakra6;
        public Button Chakra7;
        public Button Chakra8;

        [Header ("Info")]
        public GameObject Exchnage1;
        public GameObject Exchnage2;
        public GameObject Exchnage3;
        public GameObject Exchnage4;
        public GameObject Exchnage5;
        public GameObject Exchnage6;
        public GameObject Exchnage7;
        public GameObject Exchnage8;

        [Header("EXIT")]
        public GameObject Exit;


        private void Start()
        {
            Chakra2.interactable = false;
            Chakra3.interactable = false;
            Chakra4.interactable = false;
            Chakra5.interactable = false;
            Chakra6.interactable = false;
            Chakra7.interactable = false;
            Chakra8.interactable = false;

            Exit.SetActive(false);

            Exchnage1.SetActive(false);
            Exchnage2.SetActive(false);
            Exchnage3.SetActive(false);
            Exchnage4.SetActive(false);
            Exchnage5.SetActive(false);
            Exchnage6.SetActive(false);
            Exchnage7.SetActive(false);
            Exchnage8.SetActive(false);

        }


        public void Update()
        {
            if(Chakra5.interactable == true && Chakra6.interactable == true)
            {

                Chakra7.interactable = true;
            }


        if(Input.GetKeyDown(KeyCode.Escape))
            {
                Exchnage1.SetActive(false);
                Exchnage2.SetActive(false);
                Exchnage3.SetActive(false);
                Exchnage4.SetActive(false);
                Exchnage5.SetActive(false);
                Exchnage6.SetActive(false);
                Exchnage7.SetActive(false);
                Exchnage8.SetActive(false);

            }

        }
        public void First()
        {
            if(FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 2)
            {

                Exchnage1.SetActive(true);
                
            }  else
            {
                Exit.SetActive(true);


            }




        }



     
        public void Second()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 3)
            {
                Exchnage2.SetActive(true);


            }
            else
            {
                Exit.SetActive(true);

            }

        }
        public void Third()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 3)
            {

                Exchnage3.SetActive(true);

            }
            else
            {
                Exit.SetActive(true);

            }
        }
        public void Fourth()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 2)
            {

                Exchnage4.SetActive(true);

            }
            else
            {
                Exit.SetActive(true);

            }
        }


        public void Fifth()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 4)
            {

                Exchnage5.SetActive(true);

            }
            else
            {
                Exit.SetActive(true);

            }
        }



        public void Sixth()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 4 )
            {

                Exchnage6.SetActive(true);

            }
            else
            {
                Exit.SetActive(true);

            }
        }
        public void Seventh()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 5)
            {

                Exchnage7.SetActive(true);

            }
            else
            {
                Exit.SetActive(true);

            }
        }

        public void Eighth()
        {
            if (FindObjectOfType<PlayerStats>().PlayerSkillPoints >= 8)
            {

                Exchnage8.SetActive(true);

            }
            else
            {
                Exit.SetActive(true);

            }
        }






        public void HitOkay1()
        {
            Exchnage1.SetActive(false);
            Chakra2.interactable = true;
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 2;
        }

        public void HitOkay2()
        {
            Exchnage2.SetActive(false);

            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 3;
            Chakra3.interactable = true;
            Chakra4.interactable = true;


        }
        public void HitOkay3()
        {
            Exchnage3.SetActive(false);
            Chakra6.interactable = true;
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 3;
        }


        public void HitOkay4()
        {
            Exchnage4.SetActive(false);
            Chakra5.interactable = true;
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 2;
        }



        public void HitOkay5()
        {
            Exchnage5.SetActive(false);
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 4;
        }


        public void HitOkay6()
        {
            Exchnage6.SetActive(false);
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 4;
        }
        public void HitOkay7()
        {
            Exchnage7.SetActive(false);
            Chakra8.interactable = true;
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 5;
        }


        public void HitOkay8()
        {
            Exchnage8.SetActive(false);
            FindObjectOfType<PlayerStats>().PlayerSkillPoints -= 8;
        }


        public void Return()
        {

            Exit.SetActive(false);
        }

    }








}


