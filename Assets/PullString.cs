using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullString : MonoBehaviour
{
    public Transform LeftPoint;
    public Transform RightPoint;
    public Transform CenterPoint;
    LineRenderer SlingshotString;
    public Vector3 currentMouseWorldPosition;
    public bool move=true;
    public GameObject Carprefab;
    public GameObject Car;
    Rigidbody rb;
    Camera carCamera; // Reference to the car's camera
    public Camera slingshotCamera; // Reference to the slingshot camera
    float distnaceZ;
    float distnaceX;
    private Vector3 initialCenterPoint;

    void Start()
    {
        
        SlingshotString = GetComponent<LineRenderer>();
        SlingshotString.SetPositions(new Vector3[3] { LeftPoint.position, CenterPoint.position, RightPoint.position });

        // Instantiate the car and find its camera
        Car = Instantiate(Carprefab, CenterPoint.position, Quaternion.identity);
        Car.transform.localScale = Vector3.one * 0.3f;
        rb = Car.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;

        // Find the car camera and disable it initially
        carCamera = Car.GetComponentInChildren<Camera>();
        if (carCamera != null)
        {
            carCamera.enabled = false; // Disable the car camera initially
        }

        initialCenterPoint = CenterPoint.position;
    }

    public void OnPointerPosition(InputAction.CallbackContext pr)
{
    Debug.Log("started");
    if (pr.started)
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        
        // Define the plane of interaction at Y = 1 (or whatever height your ground is)
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, 1, 0));
        
        // Create a ray from the camera through the mouse position
        Ray ray = slingshotCamera.ScreenPointToRay(mousePosition);
        
        // Calculate the point of intersection with the plane
        float enter;
        if (groundPlane.Raycast(ray, out enter))
        {
            currentMouseWorldPosition = ray.GetPoint(enter);
            Debug.Log($"Calculated World Position: {currentMouseWorldPosition}");
        }
        else
        {
            Debug.Log("Mouse is not over the ground plane.");
        }
    }
}


    public void OnClick(InputAction.CallbackContext cl)
    {
        if (cl.started)
        {
            move = false;
            launch();
        }
    }

    void launch()
    {
        Vector3 launchDirection = initialCenterPoint - CenterPoint.position;
        float launchForce = launchDirection.magnitude * 10f; // Adjust multiplier as needed

        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = launchDirection.normalized * launchForce * -1;

        // Reset CenterPoint position
        CenterPoint.position = initialCenterPoint;
        SlingshotString.SetPositions(new Vector3[3] { LeftPoint.position, CenterPoint.position, RightPoint.position });

        // Switch cameras
        if (slingshotCamera != null)
        {
            slingshotCamera.enabled = false; // Disable slingshot camera
        }
        if (carCamera != null)
        {
            carCamera.enabled = true; // Enable the car camera
        }
    }

    void Update()
    {
  
        //Debug.Log(currentMouseWorldPosition);

        if (move == true)
        {
            // Moving the rubber
            Vector3 direction = (currentMouseWorldPosition);
            float step = 3f * Time.deltaTime;
            CenterPoint.transform.position = direction * step;

            // Making the car follow the pointer
            Car.transform.position = direction;

            // Moving the rubber
            SlingshotString.SetPositions(new Vector3[3] { LeftPoint.position, direction, RightPoint.position });
        }
    }
}
