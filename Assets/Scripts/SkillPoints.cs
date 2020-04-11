using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA

{



    public class SkillPoints : MonoBehaviour
    {
        GameObject player;
        PlayerStats stats;
        public float Points;

        private void Start()
        {

            player = GameObject.FindGameObjectWithTag("Player");
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        private void Update()
        {


            transform.position = Vector3.MoveTowards(transform.position ,player.transform.position, 1f);


            if(transform.position == player.transform.position)
            {
                Destroy(this.gameObject);

                stats.PlayerSkillPoint(Points);


            }

        }





    }
}