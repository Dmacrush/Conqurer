using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    public float MaxDistnace = 1.0f;
    public float MinDistnace = 4.0f;

    public float Smooth = 10.0f;

    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;


   public void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;



    }

    // Update is called once per frame
   public void Update()
    {


        Vector3 desiredCampos = transform.parent.TransformPoint(dollyDir * MaxDistnace);

        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position,desiredCampos,out hit))
        {
            distance = Mathf.Clamp((hit.distance *0.8f), MinDistnace, MaxDistnace);
        } else
        {
            distance = MaxDistnace;

        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * Smooth);
        
    }
}
