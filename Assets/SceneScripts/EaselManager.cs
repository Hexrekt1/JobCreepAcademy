using UnityEngine;

public class EaselManager : MonoBehaviour
{
    public GameObject[] easels; // Array to hold all easels
    public GameObject xrRig; // Reference to the XR Rig
    public Vector3[] easelPositions; // Positions where the XR Rig will be teleported to after 8 button presses

    private int currentEaselIndex = 0; // Tracks which easel is currently active
    private int pressCount = 0; // Counts the number of button presses

    private void Start()
    {
        // Initially hide all easels by deactivating them
        foreach (var easel in easels)
        {
            easel.SetActive(false);
        }
    }

    // This function will be called each time the button is pressed
    public void OnButtonPressed()
    {
        pressCount++; // Increment button press count

        if (pressCount < 8)
        {
            HandleEaselVisibility();
        }
        else
        {
            TeleportXR();
        }
    }

    // Handle the visibility and teleportation of easels based on button press count
    private void HandleEaselVisibility()
    {
        // Make the current easel visible and teleport it to its position
        if (currentEaselIndex < easels.Length)
        {
            // Hide all other easels
            foreach (var easel in easels)
            {
                easel.SetActive(false);
            }

            // Show current easel
            easels[currentEaselIndex].SetActive(true);

            // Move the current easel to its position (if needed, use a specific movement here)
            easels[currentEaselIndex].transform.position = easelPositions[currentEaselIndex];

            currentEaselIndex++; // Move to the next easel
        }
    }

    // Teleport the XR Rig to the last easel position after 8 presses
    private void TeleportXR()
    {
        // Teleport XR Rig to the final easel location (for example, the last easel)
        xrRig.transform.position = easelPositions[easelPositions.Length - 1];
    }
}
