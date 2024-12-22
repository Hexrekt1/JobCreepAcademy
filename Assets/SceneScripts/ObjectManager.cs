using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] drawnObjects; // Array to hold the 7 objects (e.g., cubes, spheres)
    private int currentObjectIndex = 0; // Tracks the current object being interacted with

    public EaselManager easelManager; // Reference to EaselManager for calling OnButtonPressed

    private void Start()
    {
        // Initially, only the first drawn object is active, and it starts at y = 5
        for (int i = 1; i < drawnObjects.Length; i++)
        {
            drawnObjects[i].SetActive(false); // Make all other objects inactive
        }

        // Start the first object at y = 5 and move it down to y = 1
        drawnObjects[0].SetActive(true); // Make the first object visible
        StartCoroutine(MoveObjectDown(drawnObjects[0])); // Move it down to y = 1
    }

    // This function will be called when the button is pressed to move objects
    public void OnButtonPressed()
    {
        if (currentObjectIndex < drawnObjects.Length)
        {
            StartCoroutine(HandleObjectTransition());
        }
        else
        {
            // Trigger EaselManager's OnButtonPressed if the objects are all done
            easelManager.OnButtonPressed();
        }
    }

    // This coroutine will handle the transition of objects (moving and visibility)
    private IEnumerator HandleObjectTransition()
    {
        GameObject currentObject = drawnObjects[currentObjectIndex];

        // Make the current object visible and move it up to y = 5
        currentObject.SetActive(true); // Make the object visible
        yield return StartCoroutine(MoveObjectUp(currentObject));

        // After moving it up, hide the current object
        currentObject.SetActive(false);

        // Move to the next object, and show it while moving it down to y = 1
        currentObjectIndex++;
        if (currentObjectIndex < drawnObjects.Length)
        {
            GameObject nextObject = drawnObjects[currentObjectIndex];
            nextObject.SetActive(true); // Make the next object visible
            yield return StartCoroutine(MoveObjectDown(nextObject)); // Move it down to y = 1
        }
    }

    // Coroutine to move the object up to y = 5
    private IEnumerator MoveObjectUp(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 5f, startPosition.z); // Move up to y = 5
        float duration = 2f; // Duration of the movement
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition; // Ensure it reaches exactly y = 5
    }

    // Coroutine to move the object down to y = 1
    private IEnumerator MoveObjectDown(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 1f, startPosition.z); // Move down to y = 1
        float duration = 2f; // Duration of the movement
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition; // Ensure it reaches exactly y = 1
    }
}
