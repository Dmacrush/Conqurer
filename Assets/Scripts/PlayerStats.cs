using System.Collections;
using System.Collections.Generic;
using CheckPointSystem;
using SaveSystem;
using SaveSystem.Serialization;
using UnityEngine;
using UnityEngine.UI;


namespace SA

{
    public class PlayerStats : MonoBehaviour
    {
        public bool Penalty;
        public float ItemHealingTimerStart;
        public bool IsplayerDead;
        public float T_imer = 1f;

        public Transform HealingPoint;
        public GameObject HealingEffect;

        public GameObject Healing; // Healing Items...
        public GameObject HealingItems; // PalletGameObject;
        public GameObject HealingShader;
        public Text HealingText;
        public Text HealingInvText;
        public int HealingValue;


        public float Timer;
        public bool IsHealing = true;
        public bool IsItemHealing = false;

        public bool PlayinhHealingAnime;
        public bool IteamHealingAnime;
        public float HealingAmount;

        public GameObject ParriedEffects;
        public GameObject ParriedSpark;
        public Transform EffectsPoint;
        public int DivideValue = 2;

        public Image SkillPoints;
        public float SKillLengths;
        public Text SkillNumber;
        public int PlayerSkillPoints;


        public Image PlayerHealth;
        public Image PlayerDamageRate;
        public GameObject ParryAudio;
        StateManager states;
        public Vector3 PLAYERPOS;
        public Vector3 CURRENTpLAYERPOS;
        public bool Getting_Parried;

        public delegate void DeathDelegate();

        public event DeathDelegate deathEvent;

        public void Start()
        {
            states = GetComponent<StateManager>();
            PLAYERPOS = CURRENTpLAYERPOS;
            Getting_Parried = false;
            IsplayerDead = true;
        }

        public void Die()
        {
            deathEvent?.Invoke();
            SKillLengths = SaveData.current.playerSkillLengths;
            PlayerSkillPoints = SaveData.current.playerSkillPoints;
            PlayerHealth.fillAmount = 1f;
            IsplayerDead = false;
            StartCoroutine(MoveToNewPosition());
        }

        public IEnumerator MoveToNewPosition()
        {
            //Debug.Log("Resetting");
            yield return new WaitForSeconds(0.5f);
            transform.position = CheckpointManager.instance.currentCheckpoint.position;
        }

        public void Update()
        {
            //for debugging
            // if (Input.GetKeyDown(KeyCode.Alpha9))
            // {
            //     Die();
            // }


            //   HealingText.text = HealingInvText.text;
            HealingText.text = HealingValue.ToString();
            HealingInvText.text = HealingValue.ToString();
            SkillNumber.text = PlayerSkillPoints.ToString();
            SkillPoints.fillAmount = SKillLengths;

            if (SaveData.current != null)
            {
                SaveData.current.playerSkillLengths = SKillLengths;
                SaveData.current.playerSkillPoints = PlayerSkillPoints;
            }

            if (HealingValue == 0)
            {
                HealingValue = 0;
                PlayinhHealingAnime = false;
            }

            if (HealingValue > 0)
            {
                PlayinhHealingAnime = true;
            }

            if (FindObjectOfType<ScoreManager>().ItemNUmber > 0)
            {
                IteamHealingAnime = true;
            }

            if (FindObjectOfType<ScoreManager>().ItemNUmber == 0)
            {
                FindObjectOfType<ScoreManager>().ItemNUmber = 0;
                IteamHealingAnime = false;
            }


            if (Input.GetKeyDown(KeyCode.Tab) && FindObjectOfType<ItemSwtiching>().SelectedItems == 0 &&
                ItemHealingTimerStart >= 10f)
            {
                if (PlayinhHealingAnime == true)
                {
                    states.Anim.Play("Healing");
                    IsHealing = true;
                    GameObject H = Instantiate(HealingEffect, HealingPoint.transform.position,
                        HealingPoint.transform.rotation);
                    Destroy(H, 2f);
                }
            }


            if (Input.GetKeyDown(KeyCode.Tab) && FindObjectOfType<ItemSwtiching>().SelectedItems == 1)
            {
                if (IteamHealingAnime == true)
                {
                    states.Anim.Play("PalletHealing");
                    IsItemHealing = true;
                    HealingItems.SetActive(true);
                    GameObject H = Instantiate(HealingEffect, HealingPoint.transform.position,
                        HealingPoint.transform.rotation);
                    Destroy(H, 2f);
                }
            }


            if (Timer > 0.9f)
            {
                Healing.SetActive(false);
            }

            if (Timer <= 0f)
            {
                HealingShader.SetActive(false);
                states.Anim.SetLayerWeight(states.Anim.GetLayerIndex("OverRide"), 1);
                Timer = 2f;
                IsHealing = false;
                states.IsnotMoving = false;
            }

            if (IsHealing == true)
            {
                states.IsnotMoving = true;
                states.CanMove = false;
                states.Anim.SetLayerWeight(states.Anim.GetLayerIndex("OverRide"), 0);
                Healing.SetActive(true);
                Timer -= Time.deltaTime;
            }


            if (IsItemHealing == true)
            {
                ItemHealingTimerStart -= Time.deltaTime;
            }

            if (ItemHealingTimerStart < 10f)
            {
                HealingShader.SetActive(true);
                PlayerHealth.fillAmount += 0.001f;
            }


            if (ItemHealingTimerStart <= 0f)
            {
                IsItemHealing = false;
                ItemHealingTimerStart = 10f;
                HealingShader.SetActive(false);
                HealingItems.SetActive(true);
            }


            if (ItemHealingTimerStart >= 10f)
            {
                HealingItems.SetActive(false);
            }

            //handle player getting parried by.
            if (Getting_Parried == true)
            {
                states.Anim.SetLayerWeight(states.Anim.GetLayerIndex("OverRide"), 0);
                Invoke("PlayerParried", 1f);
            }


            if (PlayerSkillPoints <= 0)
            {
                PlayerSkillPoints = 0;
            }


            if (SKillLengths >= 1f)
            {
                PlayerSkillPoints += 1;
                SKillLengths = 0;
            }

            if (PlayerHealth.fillAmount < PlayerDamageRate.fillAmount)
            {
                Invoke("HealthBarDamage", 1f);
            }


            if (PlayerHealth.fillAmount >= PlayerDamageRate.fillAmount)
            {
                PlayerDamageRate.fillAmount = PlayerHealth.fillAmount;
            }


            // handle player death.
            if (PlayerHealth.fillAmount <= 0)
            {
                //states.IsnotMoving = true;
                //states.Anim.SetBool("New Bool", true);
                //states.Anim.SetLayerWeight(states.Anim.GetLayerIndex("OverRide"), 0);
                T_imer -= Time.deltaTime;
                // FindObjectOfType<InputHandler>().B_Input = false;
                Die();
            }
            else
            {
                // states.IsnotMoving = false;
                states.Anim.SetBool("New Bool", false);
                T_imer = 1f;
                IsplayerDead = true;
            }

            if (SKillLengths < 0f)
            {
                PlayerSkillPoints -= 1;
                SKillLengths = 0.99f;
            }

            if (T_imer <= 0.0f)
            {
                if (IsplayerDead == true)
                {
                    SKillLengths -= 0.5f;
                    IsplayerDead = false;
                }
            }
        }


        public void GettingParried()
        {
            Getting_Parried = true;
            PlayerHealth.fillAmount -= 0.008f;
            states.Anim.Play("Parry_Received");
            states.Anim.SetLayerWeight(states.Anim.GetLayerIndex("OverRide"), 0);

            Invoke("PlayerParried", 1f);

            GameObject p = Instantiate(ParryAudio, transform.position, transform.rotation);
            Destroy(p, 1f);


            GameObject s = Instantiate(ParriedEffects, EffectsPoint.transform.position,
                EffectsPoint.transform.rotation);
            Destroy(s, 1f);
            GameObject E = Instantiate(ParriedSpark, EffectsPoint.transform.position, EffectsPoint.transform.rotation);
            Destroy(E, 1f);
        }


        public void DoDamage(float v)
        {
            PlayerHealth.fillAmount -= v;
            //  states.Anim.Play("Hurt");
        }


        public void PlayerParried()
        {
            Getting_Parried = false;
            states.Anim.SetLayerWeight(states.Anim.GetLayerIndex("OverRide"), 1);
        }


        public void HealthBarDamage()
        {
            PlayerDamageRate.fillAmount = PlayerHealth.fillAmount;
        }


        public void PlayerSkillPoint(float v)
        {
            SKillLengths += v;
        }

        public void PlayerHeal(float h)
        {
            PlayerHealth.fillAmount += h;
        }


        public void PlayerHealing()
        {
            PlayerHeal(HealingAmount);
            HealingValue -= 1;
            HealingShader.SetActive(true);
        }


        public void PlayerItemHealing()
        {
            FindObjectOfType<ScoreManager>().ItemNUmber -= 1;
        }
    }
}