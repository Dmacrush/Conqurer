using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SA
{
    public class BossTarget : MonoBehaviour
    {
        public GameObject BossHealth;
        public Image Boss_Health;
        public GameObject UnlockPanel;

        public void Update()
        {
            if (Vector3.Distance(transform.position, FindObjectOfType<PlayerStats>().transform.position) < 20f)
            {
                BossHealth.SetActive(true);
            }
            else
            {
                BossHealth.SetActive(false);
            }


            if (Boss_Health.fillAmount <= 0f)
            {
                FindObjectOfType<PlayerStats>().SKillLengths += 0.5f;
                UnlockPanel.SetActive(false);
                Destroy(FindObjectOfType<BossTarget>());
            }
        }
    }
}