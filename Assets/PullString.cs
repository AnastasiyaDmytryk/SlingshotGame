using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    public Rigidbody rb;
    public bool isLaunched;

    //referencing variables
    [SerializeField] public TextMeshProUGUI laps;
    [SerializeField] public TextMeshProUGUI pointText;
    [SerializeField] public GameObject Leaderboard, lapObject, startobj, finishobj;

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

        //get variables
        var prefabScript = Car.GetComponent<LapCounter>();
        if (prefabScript != null)
        {
            prefabScript.laps = laps;
            prefabScript.pointText = pointText;
            prefabScript.Leaderboard = Leaderboard;
            prefabScript.lapObject = lapObject;
            prefabScript.startobj = startobj;
            prefabScript.finishobj = finishobj;
        }
        
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
        Debug.Log("pointing");
        Vector3 temp = Mouse.current?.position.ReadValue() ?? Vector3.zero;
        if (temp == Vector3.zero)
        {
            Debug.LogError("Mouse position is unavailable. Check Input System configuration.");
            return;
        }

        Ray ray = slingshotCamera.ScreenPointToRay(temp);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            currentMouseWorldPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            Debug.Log($"Hit Point: {currentMouseWorldPosition}");
        }
        else
        {
            Debug.LogWarning("Raycast did not hit any object. Check colliders or camera setup.");
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 2f);
    }
}


    public void OnClick(InputAction.CallbackContext cl)
    {
        if (cl.started)
        {
            Debug.Log("clicked");
            launch();
        }
        
    }

    void launch()
    {
        
        Vector3 launchDirection = initialCenterPoint - CenterPoint.position;
        float launchForce = launchDirection.magnitude * 10f; // Adjust multiplier as needed

        isLaunched=true;
        Debug.Log("click");

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
