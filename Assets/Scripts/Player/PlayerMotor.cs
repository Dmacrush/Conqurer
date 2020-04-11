using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
   [Header("Player Movements")]
    public float JumpHeight = 1f;
    public float Gravity = -12f;
    public float WalkSpeed = 2;
    public float RunSpeed = 6;
    public float turnSmoothTIme = 0.1f;
    public float SpeedSmoothTime = 0.1f;
    public float RollSpeed = 1f;
    public string RollName;
    public bool PlayRoll = false;


    public Vector3 CamForward;
    public Vector3 Camright;

    [Header("Player Movements")]
    public float CrouchSPeed = 2f;
    public float CrouchRunspeed = 6f;
    public bool C_IsCrouh = false;

    [Range(0, 1)]
    public float AirFloatPercentage;

    float speedSmoothVelocity;
    float CurrentSpeed;
    Transform CameraT;
    float turnSmoothVelocity;



    Animator Anime;
    CharacterController Controller; // Referencing character controller.
    float VelocityY;


    void Start()
    {
        Anime = GetComponent<Animator>();
        CameraT = Camera.main.transform;
        Controller = GetComponent<CharacterController>();// Referencing character controller.
        

    }


    // Update is called once per frame
    void Update()
    {

        Roll();
        // Input Section
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool Running = Input.GetKey(KeyCode.LeftShift);
        bool Crouch = Input.GetKey(KeyCode.LeftControl);

        
            // if player press left control player will start at crouch idle pos and disable the walking pose else vice versa.
            if (Crouch == true)
            {
                Anime.SetBool("Crouch", true);

                // Crouch Section
                Running = false;
                float CroushAnimationSpeed = ((Running) ? CrouchSPeed / CrouchRunspeed : CurrentSpeed / CrouchSPeed * .5f);
                Anime.SetFloat("IsCrouching", CroushAnimationSpeed, SpeedSmoothTime, Time.deltaTime);
                Crouching(inputDir, Crouch);

            }
            else
            {


                //Animator Section
                float animationSpeed = ((Running) ? CurrentSpeed / RunSpeed : CurrentSpeed / WalkSpeed * .5f);
                Anime.SetFloat("Speed", animationSpeed, SpeedSmoothTime, Time.deltaTime);
                Move(inputDir, Running);
                Anime.SetBool("Crouch", false);


            }

        

      



    }

  
    // For walking
    public  void Move(Vector2 inputDir, bool Running)
    {
      

        if (inputDir != Vector2.zero)

            {
                float TargetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + CameraT.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTIme));

            }

            // For walking
            float speed = ((Running) ? RunSpeed : WalkSpeed) * inputDir.magnitude;
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, speed, ref speedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));


            VelocityY += Time.deltaTime * Gravity;
            Vector3 velocity = transform.forward * CurrentSpeed + Vector3.up * VelocityY;

            Controller.Move(velocity * Time.deltaTime);
            CurrentSpeed = new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude;

            // Checking if the player is grounded or not.
            if (Controller.isGrounded)
            {
                VelocityY = 0;
                Anime.SetBool("Jump", false);
             
            }

        






    }

    // For Crouching
    public void Crouching(Vector2 inputDir, bool Crouch)
    {


        if (inputDir != Vector2.zero)

        {
            float TargetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + CameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTIme));

        }


        // For walking
        float C_speed = ((Crouch) ? CrouchRunspeed : CrouchSPeed) * inputDir.magnitude;
        CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, C_speed, ref speedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));


        VelocityY += Time.deltaTime * Gravity;
        Vector3 velocity = transform.forward * CurrentSpeed + Vector3.up * VelocityY;

        Controller.Move(velocity * Time.deltaTime);
        CurrentSpeed = new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude;


    }



    public void Jump()
    {

        if (Controller.isGrounded)
        {
            Anime.SetBool("Jump", true);
            float jumpVelocity = Mathf.Sqrt(-2 * Gravity * JumpHeight);
            VelocityY = jumpVelocity;
        }
    }


    // For controlling player better in air or in ground
    float GetModifiedSmoothTime(float SmoothTime)
    {
        if(Controller.isGrounded)
        {
            return SmoothTime;

        }
        {

            return SmoothTime / AirFloatPercentage;
        }

    }





    public void Roll()
    {

        // Roll Left
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Q))
        {


            transform.Translate(Vector3.left * RollSpeed * Time.deltaTime);
            Anime.CrossFade(RollName, 0.01f);


        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Q)) // Roll Right.

        {

            transform.Translate(Vector3.right * RollSpeed * Time.deltaTime);
            Anime.CrossFade(RollName, 0.01f);

        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Q)) // Roll Back.

        {

            transform.Translate(Vector3.back * RollSpeed * Time.deltaTime);
            Anime.CrossFade(RollName, 0.01f);

        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Q)) // Roll Forward.
        {

            transform.Translate(Vector3.forward * RollSpeed * Time.deltaTime);
            Anime.CrossFade(RollName, 0.01f);

        }
    }




}
