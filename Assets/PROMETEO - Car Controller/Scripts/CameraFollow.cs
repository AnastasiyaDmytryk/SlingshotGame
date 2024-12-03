using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform carTransform;  // The car to follow
    [Range(1, 10)] public float followSpeed = 5f;  // Speed of the camera following the car
    [Range(1, 10)] public float lookSpeed = 5f;    // Speed of the camera rotation to look at the car

    public Vector3 offset = new Vector3(0, 5, -10);  // Offset to place the camera behind the car

    void FixedUpdate()
    {
        // Smoothly move the camera to the target position
        Vector3 targetPosition = carTransform.position + carTransform.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to look at the car
        Quaternion targetRotation = Quaternion.LookRotation(carTransform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }
}
