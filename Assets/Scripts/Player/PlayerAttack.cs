using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA

{


    public class PlayerAttack : MonoBehaviour
    {


        [Header("AXE THROW")]
        public Rigidbody Axe;
        public Transform TAxe;
        public BoxCollider Axecol;
        public float ThrowForce = 50f;
        public float RotationSped = 500f;
        public bool IsAxeBack = true;
        public bool IsReturning = false;
        public Transform Hand, Curve_Point;
        private Vector3 old_Pos;
        private Vector3 old_Rot;
        private Vector3 pullPosition;
        private AxeSpinner Axe_Spinner;
        private bool AllowAxeThrow = true;


        private float time;

        [Header("Normal ATTACKS")]
        public string[] Attacks;
        public bool PlayAnim;
        Animator Anime;
        public bool EnableRootMotion;


    




        private void Start()
        {
            Anime = GetComponent<Animator>();
            old_Pos = TAxe.localPosition;
            old_Rot = TAxe.localEulerAngles;
            Axe = Axe.GetComponent<Rigidbody>();
            Axe_Spinner = TAxe.GetComponent<AxeSpinner>();
        }




        private void Update()
        {

            // Animation States

           Anime.SetBool("AxeBack", IsReturning);


          if (IsAxeBack == true)
            {

                if (Input.GetKey(KeyCode.Z))
                {
                    Anime.SetBool("Throw", true);
                    EnableRootMotion = false;


                }

            }
            else
            {

                Anime.SetBool("Throw", false);

            }


            if (IsAxeBack == false)

            {
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    ReturnAxe();

                }

            }

            // Axe return
            if (IsReturning == true)
            {
                if (time < 1.0f)
                {

                    TAxe.position = GetAxeBackWithCurve(time, pullPosition, Curve_Point.position, Hand.position);
                    time += Time.deltaTime * 1.5f;
                }
                else
                {

                    ResetAxe();
                }
            }





         //   EnableRootMotion = !Anime.GetBool("CanMove");
          //  Anime.applyRootMotion = EnableRootMotion;

          //  Block();

         /*   if (EnableRootMotion)
            {
                return;
            }
            // Player attacks.
            if (Input.GetMouseButton(0))
            {
                if (PlayAnim)
                {
                    string TargetANim;

                    int R = Random.Range(0, Attacks.Length);
                    TargetANim = Attacks[R];

                    Anime.CrossFade(TargetANim, 0.01f);
                    Anime.SetBool("CanMove", false);
                    EnableRootMotion = true;

                    //  PlayAnim = false;

                }


            }*/






        }

        // Player Block
      /*  public void Block()
        {

            if (Anime.GetFloat("Speed") > 0f && Input.GetKey(KeyCode.Mouse1))

            {
                if (Input.GetKey(KeyCode.Z))
                {

                    Anime.SetBool("Throw", false);

                }

                Anime.SetLayerWeight(Anime.GetLayerIndex("BlockMovements"), 1);
                PlayAnim = false;

            }
            else
            {
                Anime.SetLayerWeight(Anime.GetLayerIndex("BlockMovements"), 0);

                // Checking if axe is back is true.
                if (IsAxeBack == true)
                {
                    PlayAnim = true;

                }


            }




            if (Anime.GetFloat("Speed") < 0f && Input.GetKey(KeyCode.Mouse1))

            {
                if (Input.GetKey(KeyCode.Z))
                {

                    Anime.SetBool("Throw", false);

                }

                PlayAnim = false;

            }
            else
            {
                // Checking if axe is back is true.
                if (IsAxeBack == true)
                {
                    PlayAnim = true;

                }


            }


        }*/

        public void ThrowAxe()

        {

            PlayAnim = false;
            IsAxeBack = false;
            TAxe.parent = null;
            Axe.isKinematic = false;
            Axe_Spinner.isActivated = true;
            TAxe.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
            TAxe.transform.position += transform.right / 5;
            Axe.AddForce(Camera.main.transform.forward * ThrowForce + transform.up * 2, ForceMode.Impulse);
            Axecol.enabled = true;





        }

        // Return Axe
        public void ReturnAxe()

        {

            pullPosition = TAxe.position;
            Axe.isKinematic = true;
            Axe.Sleep();
            IsReturning = true;
            Axe_Spinner.isActivated = true;
            Axecol.enabled = false;

            // rotate axe

        }

        // reset Axe 
        public void ResetAxe()
        {
            time = 0f;
            IsReturning = false;
            IsAxeBack = true;
            TAxe.parent = Hand;
            TAxe.localPosition = old_Pos;
            TAxe.localEulerAngles = old_Rot;
            Axe_Spinner.isActivated = false;
            PlayAnim = true;





        }


        // Return Axe with curve Point;

        Vector3 GetAxeBackWithCurve(float t, Vector3 pos0, Vector3 pos1, Vector3 pos2)
        {

            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = (uu * pos0) + (2 * u * t * pos1) + (tt * pos2);
            return p;



        }

    }

}


