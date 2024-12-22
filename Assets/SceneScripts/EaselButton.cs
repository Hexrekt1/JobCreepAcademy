using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EaselButton : MonoBehaviour
{
    public EaselManager easelManager;  // Reference to the EaselManager script

    private void OnEnable()
    {
        // Get the XR Interactable component
        var interactable = GetComponent<XRGrabInteractable>();  // Or XRSimpleInteractable for select-only
        if (interactable)
        {
            // Register the OnSelectEntered event to trigger when the button is pressed
            interactable.onSelectEntered.AddListener(OnButtonPressed);
        }
    }

    private void OnDisable()
    {
        // Unregister to avoid memory leaks
        var interactable = GetComponent<XRGrabInteractable>();
        if (interactable)
        {
            interactable.onSelectEntered.RemoveListener(OnButtonPressed);
        }
    }

    // This method will be called when the button is selected (pressed) by the XR controller
    private void OnButtonPressed(XRBaseInteractor interactor)
    {
        // Call the OnButtonInteracted method in EaselManager
        easelManager.OnButtonPressed();
    }
}
