using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform carTransform;  
    [Range(1, 10)] public float followSpeed = 5f;  
    [Range(1, 10)] public float lookSpeed = 5f;    

    public Vector3 offset = new Vector3(0, 5, -10); 

    void FixedUpdate()
    {
       
        Vector3 targetPosition = carTransform.position + carTransform.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        
        Quaternion targetRotation = Quaternion.LookRotation(carTransform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }
}
