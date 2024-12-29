using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WhiteboardButtonManager : MonoBehaviour
{
    [SerializeField] private Whiteboard[] whiteboards; // Array of whiteboards
    [SerializeField] private Transform[] whiteboardPositions; // Positions for whiteboards
    [SerializeField] private Transform xrRig; // XR Rig for teleportation
    [SerializeField] private Transform xrRigFinalPosition; // Final position for XR Rig
    [SerializeField] private GameObject[] drawingObjects; // Array of drawing objects
    [SerializeField] private Vector3 drawingActivePosition = new Vector3(0, 0, 0); // Active position (e.g., y = 0)
    [SerializeField] private Vector3 drawingInactivePosition = new Vector3(0, -5, 0); // Inactive position (e.g., y = -5)
    [SerializeField] private float movementSpeed = 2f; // Speed of the drawing movement
    [SerializeField] private float cooldown = 5f; // Button cooldown time

    private int currentStep = 0;
    private bool isCooldown = false;

    private void Start()
    {
        // Validate array lengths
        if (whiteboards.Length != whiteboardPositions.Length)
        {
            Debug.LogError("Whiteboards and positions arrays must have the same length!");
            return;
        }

        // Initialize whiteboards
        for (int i = 0; i < whiteboards.Length; i++)
        {
            whiteboards[i].gameObject.SetActive(false); // Set all whiteboards inactive
        }
        whiteboards[0].gameObject.SetActive(true); // Activate only the first whiteboard

        // Initialize drawing objects
        foreach (var drawingObject in drawingObjects)
        {
            drawingObject.transform.position = drawingInactivePosition;
            drawingObject.GetComponent<MeshRenderer>().enabled = false;
        }

        // Activate the first drawing object
        if (drawingObjects.Length > 0)
        {
            drawingObjects[0].transform.position = drawingActivePosition;
            drawingObjects[0].GetComponent<MeshRenderer>().enabled = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VRHand"))
        {
            OnButtonPressed();
        }
    }

    private void OnButtonPressed()
    {
        Debug.Log("OnButtonPressed was called!");

        if (isCooldown)
        {
            Debug.Log("Button is on cooldown!");
            return;
        }

        StartCoroutine(ButtonCooldown());

        if (currentStep < whiteboards.Length - 1)
        {
            // Move the current whiteboard to its designated position
            whiteboards[currentStep].transform.position = whiteboardPositions[currentStep].position;

            // Handle the current drawing object
            if (currentStep < drawingObjects.Length)
            {
                var currentDrawingObject = drawingObjects[currentStep];
                StartCoroutine(MoveAndDeactivate(currentDrawingObject, drawingInactivePosition));
            }

            // Increment to the next step
            currentStep++;

            // Activate the next whiteboard
            whiteboards[currentStep].gameObject.SetActive(true);

            var nextMeshRenderer = whiteboards[currentStep].GetComponent<MeshRenderer>();
            if (nextMeshRenderer != null) nextMeshRenderer.enabled = true;

            // Activate the next drawing object
            if (currentStep < drawingObjects.Length)
            {
                var nextDrawingObject = drawingObjects[currentStep];
                nextDrawingObject.GetComponent<MeshRenderer>().enabled = true;
                StartCoroutine(MoveObject(nextDrawingObject, drawingActivePosition));
            }
        }
        else if (currentStep == whiteboards.Length - 1)
        {
            // Teleport XR Rig to the final position
            xrRig.position = xrRigFinalPosition.position;
            xrRig.rotation = xrRigFinalPosition.rotation;
            Debug.Log("Teleported XR Rig to final position!");
        }
    }

    private IEnumerator MoveAndDeactivate(GameObject obj, Vector3 targetPosition)
    {
        yield return MoveObject(obj, targetPosition);
        obj.GetComponent<MeshRenderer>().enabled = false; // Disable renderer after moving
    }

    private IEnumerator MoveObject(GameObject obj, Vector3 targetPosition)
    {
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }
        obj.transform.position = targetPosition; // Snap to exact position
    }

    private IEnumerator ButtonCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
