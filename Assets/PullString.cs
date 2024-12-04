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
    public bool move;
    public GameObject Carprefab;
    public GameObject Car;
    Rigidbody rb;
    public bool isLaunched;

    Camera carCamera; // Reference to the car's camera
    public Camera slingshotCamera; // Reference to the slingshot camera
    float distnaceZ;
    float distnaceX;
    private Vector3 initialCenterPoint;

    void Start()
    {
        move=true;
        SlingshotString = GetComponent<LineRenderer>();
        SlingshotString.SetPositions(new Vector3[3] { LeftPoint.position, CenterPoint.position, RightPoint.position });

        // Instantiate the car and find its camera
        Car = Instantiate(Carprefab, CenterPoint.position, Quaternion.identity);
        Car.transform.localScale = Vector3.one * 0.3f;
        Car.transform.rotation=Quaternion.Euler(0, -90, 0);
        
        rb = Car.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Find the car camera and disable it initially
        carCamera = Car.GetComponentInChildren<Camera>();
        if (carCamera != null)
        {
            carCamera.enabled = false; // Disable the car camera initially
        }

        initialCenterPoint = CenterPoint.position;
    }

   public void OnLook(InputAction.CallbackContext ol)
{
    if (ol.performed)
    {
        // Check if Camera.main is null
       
       
        Vector3 temp = Mouse.current?.position.ReadValue() ?? Vector3.zero; // Fallback if Mouse.current is null
        if (temp == Vector3.zero)
        {
            Debug.LogError("Mouse position is unavailable. Check Input System configuration.");
            return;
        }

        Ray ray = slingshotCamera.ScreenPointToRay(temp);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Ensure raycast hit
        {
            currentMouseWorldPosition = new Vector3(hit.point.x, 1, hit.point.z);
            Debug.Log(currentMouseWorldPosition);
        }
        else
        {
            Debug.LogWarning("Raycast did not hit any object.");
        }
    }
}


    public void OnClick(InputAction.CallbackContext cl)
    {
        if (cl.started)
        {
             launch();
        }
        
    }

    void launch()
    {
        
        Vector3 launchDirection = initialCenterPoint - CenterPoint.position;
        float launchForce = launchDirection.magnitude * 10f; // Adjust multiplier as needed

       isLaunched=true;

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
  
            
            Vector3 direction = (currentMouseWorldPosition);
            float step = 3f * Time.deltaTime;
            CenterPoint.transform.position = direction * step;
            Car.transform.position = direction;

            // Moving the rubber
            SlingshotString.SetPositions(new Vector3[3] { LeftPoint.position, currentMouseWorldPosition, RightPoint.position });
        
    }
}
