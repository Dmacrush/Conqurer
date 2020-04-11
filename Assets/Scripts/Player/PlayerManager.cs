using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SA
{


    public class PlayerManager : MonoBehaviour
    {


        //   public Vector3 Position = new Vector3(1000f, 1000f, 1000f);
        // public Vector3 ResetPosition = new Vector3(1000f, 1000f, 1000f);

        public GameObject Info;




        private void Start()
        {
            Time.timeScale = 0f;
            Info.SetActive(true);

        }

        public void Exit()
        {
            Info.SetActive(false);
            Time.timeScale = 1f;

        }
    }
}
