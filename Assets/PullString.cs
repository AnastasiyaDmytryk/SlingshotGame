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
    Camera carCamera; // Reference to the car's camera
    public Camera slingshotCamera; // Reference to the slingshot camera
    float distnaceZ;
    float distnaceX;
    private Vector3 initialCenterPoint;

    void Start()
    {
        move = true;
        SlingshotString = GetComponent<LineRenderer>();
        SlingshotString.SetPositions(new Vector3[3] { LeftPoint.position, CenterPoint.position, RightPoint.position });

        // Instantiate the car and find its camera
        Car = Instantiate(Carprefab, CenterPoint.position, Quaternion.identity);
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
        // Getting mouse
        if (pr.performed)
        {
            Vector3 temp = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(temp);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentMouseWorldPosition = new Vector3(hit.point.x, 1, hit.point.z);
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
