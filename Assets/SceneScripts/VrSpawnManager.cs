using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRSpawnManager : MonoBehaviour
{
    public GameObject xrRig; // Reference to the XR Rig
    public TeleportationProvider teleportationProvider; // Teleportation Provider

    void Start()
    {
        if (xrRig != null && teleportationProvider != null)
        {
            // Create a teleport request
            TeleportRequest request = new TeleportRequest
            {
                destinationPosition = transform.position,
                destinationRotation = transform.rotation
            };

            // Perform teleportation
            teleportationProvider.QueueTeleportRequest(request);
        }
        else
        {
            Debug.LogError("XR Rig or Teleportation Provider is not assigned!");
        }
    }
}
