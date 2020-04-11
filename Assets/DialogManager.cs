using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SA

{




    public class DialogManager : MonoBehaviour
    {
        public Text Dialogue;
       [TextArea(3,10)]
        public string[] Sentences;
        private int index;
        public float TypingSpeed;

        public GameObject StartCon;
        public GameObject Conv;
        public GameObject PlayerUI;


        public bool IsAboutToTalk;



        void Start()
        {


            Conv.SetActive(false);
            StartCon.SetActive(false);


        }



        public void Update()
        {

           
      
           


            if(StartCon.activeSelf)
            {

                if (Input.GetKeyDown(KeyCode.E))
                {
                       Conv.SetActive(true);
                      PlayerUI.SetActive(false);
                        NextSentences();
                    StartCon.transform.position =new Vector2(0,0);

                }



            }

            if(!StartCon.activeSelf)
            {

                Conv.SetActive(false);


            }

        }

        IEnumerator Type()
        {
            foreach (char Letter in Sentences[index].ToCharArray())
                Dialogue.text += Letter;
            yield return new WaitForSeconds(TypingSpeed);

        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {

                StartCon.SetActive(true);
                        
             }



        }

   

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {

                StartCon.SetActive(false);
            }

        }




        public void NextSentences()
        {
            if (index < Sentences.Length - 1)
            {
                index++;
                Dialogue.text = "";
                StartCoroutine(Type());
            } else
            {

                Dialogue.text = "";
                PlayerUI.SetActive(true);
                Conv.SetActive(false);
                StartCon.SetActive(false);



            }
        }
    }
}