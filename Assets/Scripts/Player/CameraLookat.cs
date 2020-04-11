using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour
{


    public float SmoothSpeed = 1.5f;
    public Transform Player;
    public Vector3 RealCamerapos;
    public Vector3 Newposition;
    private float RealCameraMag;



    private void Awake()
    {

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        RealCamerapos = transform.position - Player.position;
        RealCameraMag = RealCamerapos.magnitude - 0.5f;
    }


    private void FixedUpdate()
    {

        Vector3 StandredCampos = Player.position + RealCamerapos;
        Vector3 AbovePos = Player.position + Vector3.up * RealCameraMag;
        Vector3[] Checkpoint = new Vector3[5];
        Checkpoint[0] = StandredCampos;
        Checkpoint[1] = Vector3.Lerp(StandredCampos, AbovePos, 0.25f);
        Checkpoint[2] = Vector3.Lerp(StandredCampos, AbovePos, 0.5f);
        Checkpoint[3] = Vector3.Lerp(StandredCampos, AbovePos, 0.75f);
        Checkpoint[4] = AbovePos;

        for(int i =0; i<Checkpoint.Length;i++)
        {

            if(ViewingPosCheck(Checkpoint[i]))
            {
                break;
            }

        }

        transform.position = Vector3.Lerp(transform.position, Newposition, SmoothSpeed * Time.deltaTime);
        SmoothLookAT();
        

    }



   bool ViewingPosCheck(Vector3 checkPos)
    {
        RaycastHit hit;

        if(Physics.Raycast(checkPos,Player.position - checkPos,out hit ,RealCameraMag))
        {
            if(hit.transform != Player)
            {
                return false;
            }
        }

        Newposition = checkPos;
        return true;
    }


    void SmoothLookAT()

    {

        Vector3 RelPlayerpos = Player.position - transform.position;
        Quaternion LookAtRotaion = Quaternion.LookRotation(RelPlayerpos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, LookAtRotaion, SmoothSpeed * Time.deltaTime);

    }

}
