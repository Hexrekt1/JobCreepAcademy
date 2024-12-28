using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System

public class WhiteboardButtonManager : MonoBehaviour
{
    [SerializeField] private Whiteboard[] whiteboards; // Array of whiteboards
    [SerializeField] private Transform[] whiteboardPositions; // Positions for whiteboards
    [SerializeField] private Transform xrRig; // XR Rig for teleportation
    [SerializeField] private Transform xrRigFinalPosition; // Final position for XR Rig
    [SerializeField] private float cooldown = 5f; // Button cooldown time

    private int currentStep = 0;
    private bool isCooldown = false;

    private void Start()
    {
        if (whiteboards.Length != whiteboardPositions.Length)
        {
            Debug.LogError("Whiteboards and positions arrays must have the same length!");
        }

        // Deactivate all whiteboards except the first
        for (int i = 1; i < whiteboards.Length; i++)
        {
            whiteboards[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for "K" key press using the New Input System
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            OnButtonPressed();
        }
    }

    private void OnButtonPressed()
    {
        if (isCooldown)
        {
            Debug.Log("Button is on cooldown!");
            return;
        }

        StartCoroutine(ButtonCooldown());

        if (currentStep < whiteboards.Length - 1)
        {
            // Move the current whiteboard to the desired position
            whiteboards[currentStep].transform.position = whiteboardPositions[currentStep].position;

            // Enable the Mesh Renderer for the next whiteboard
            var nextWhiteboard = whiteboards[currentStep + 1];
            nextWhiteboard.gameObject.SetActive(true);
            var meshRenderer = nextWhiteboard.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            // Increment to the next step
            currentStep++;
        }
        else if (currentStep == whiteboards.Length - 1)
        {
            // Teleport XR Rig to the final position
            xrRig.position = xrRigFinalPosition.position;
            xrRig.rotation = xrRigFinalPosition.rotation;
            Debug.Log("Teleported XR Rig to final position!");
        }
    }

    private IEnumerator ButtonCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
