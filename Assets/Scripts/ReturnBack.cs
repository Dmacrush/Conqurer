using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnBack : MonoBehaviour
{
    public int Timer;

    public void Update()
    {


        Invoke("Return", Timer);

    }



   public void Return()
    {

        SceneManager.LoadScene("Level 1");

    }

}
