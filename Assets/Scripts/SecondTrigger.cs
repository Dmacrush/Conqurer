using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace SA

{



    public class SecondTrigger : MonoBehaviour
    {

        public string SceneName;
 



        private void Start()
        {

        }




        public void OnTriggerEnter(Collider other)
        {

            PlayerStats player = other.transform.GetComponent<PlayerStats>();

            if (player == null)
                return;
            else
                

            SceneManager.LoadScene(SceneName);

      
    }



    }
}