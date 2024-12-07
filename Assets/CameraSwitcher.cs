using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera slingshotCamera;  // Reference to the slingshot camera
    public Camera carCamera;       // Reference to the car's camera

    private void Start()
    {
        // Enable the slingshot camera and disable the car camera at the start
        if (slingshotCamera != null)
            slingshotCamera.enabled = true;

        if (carCamera != null)
            carCamera.enabled = false;
    }

    public void SwitchToCarCamera()
    {
        // Switch to the car camera
        if (slingshotCamera != null)
            slingshotCamera.enabled = false;

        if (carCamera != null)
            carCamera.enabled = true;
    }
}
