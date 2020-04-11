using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;


namespace SA
{

    public class AttackInVIllage : MonoBehaviour
    {
        [Header("Change Lighting")]
        public Light lights;
        public BoxCollider col;
        public float Distnace;

        [Header("Blue Flames")]

        public GameObject Fires;
        public GameObject Enemies;
        public GameObject PressImage;

        public GameObject PrayDialogue;
        bool inside = true;
        bool IsanimationPlaying;

        public void Start()
        {
            //  lights = GetComponent<Light>();
            Enemies.SetActive(false);
            Fires.SetActive(false);
            col.enabled = false;
            PressImage.SetActive(false);
            PrayDialogue.SetActive(false);
            IsanimationPlaying = false;


        }




        private void Update()
        {

            if (Vector3.Distance(transform.position, FindObjectOfType<StateManager>().transform.position) < Distnace)
            {
                if(inside == true)
                {

                     PressImage.SetActive(true);
                    IsanimationPlaying = true;

                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inside = false;
                    PrayDialogue.SetActive(true);
                 
                }

                if (IsanimationPlaying == true && Input.GetKeyDown(KeyCode.E))
                {

                    FindObjectOfType<StateManager>().Anim.Play("KneelDown");
                    IsanimationPlaying = false;
                } 

            }
            else
            {

                PressImage.SetActive(false);

            }


           

            if (PrayDialogue.activeSelf)
            {

                if (Input.GetKeyDown(KeyCode.E))
                {

                    NextSentences();

                }

            }

        }



        //   Invoke("Attack_Onvillage", 5f);
        //   col.enabled = true;







     

        void Attack_Onvillage()
        {
            Debug.Log("Calling");
            Enemies.SetActive(true);
            Fires.SetActive(true);
            lights.intensity = 1000f;
            lights.colorTemperature = 20000f;





        }

        public Text Dialogue;

        [TextArea(3,10)]
        public string[] Sentences;
        private int index;
        public float TypingSpeed;

        // public GameObject AnchorPoint;


        public bool IsAboutToTalk;



    

        IEnumerator Type()
        {
            foreach (char Letter in Sentences[index].ToCharArray())
                Dialogue.text += Letter;
            yield return new WaitForSeconds(TypingSpeed);

        }


       




        public void NextSentences()
        {
            if (index < Sentences.Length - 1)
            {
                index++;
                Dialogue.text = "";
                StartCoroutine(Type());
                PressImage.SetActive(false);
                PressImage.SetActive(false);

            }
            else
            {

                Dialogue.text = "";
                PrayDialogue.SetActive(false);
                IsanimationPlaying = false;
                FindObjectOfType<StateManager>().Anim.SetLayerWeight(FindObjectOfType<StateManager>().Anim.GetLayerIndex("Pray"), 0);



            }
        }
    }
}