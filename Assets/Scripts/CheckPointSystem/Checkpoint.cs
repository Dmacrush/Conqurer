using System;
using System.Collections.Generic;
using CheckPointSystem;
using SA;
using UnityEngine;

namespace CheckPointSystem
{
    public class Checkpoint : MonoBehaviour
    {
        public bool isTriggered;
        public Vector3 position;

        private PlayerStats player;
        public KeyCode OpenUIButton = KeyCode.I;

        void Awake()
        {
            position = transform.position;
            player = FindObjectOfType<PlayerStats>();
            player.deathEvent += OnPlayerDeath;
        }

        void OnPlayerDeath()
        {
            //put anything you want to the checkpoint when player dies here
        }

        // void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         
        //         // Debug.Log(name + "Was Triggered");
        //         isTriggered = true;
        //         CheckpointManager.instance.UpdateCheckpoint(this);
        //     }
        // }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Input.GetKeyDown(OpenUIButton))
                {
                    CheckpointManager.instance.UIOpen = true;
                    isTriggered = true;
                    CheckpointManager.instance.UpdateCheckpoint(this);

                    //add in what ever u want to make the checkpoint do when triggered
                    
                }
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CheckpointManager.instance.UIOpen = false;
            }
        }
    }
}