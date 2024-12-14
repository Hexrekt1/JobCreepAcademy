using UnityEngine;
using UnityEngine.UI; // Required for the default Text component

public class DigitalTimer : MonoBehaviour
{
    [SerializeField] private Text timerText; // Reference to the Text component for timer display
    [SerializeField] private WhiteboardMarker whiteboardMarker; // Reference to the WhiteboardMarker script

    private float timeRemaining = 360f; // 6 minutes in seconds (360 seconds)
    private bool isCountingDown = true; // Control whether the timer is counting down or not

    void Update()
    {
        if (isCountingDown && timeRemaining > 0f)
        {
            // Decrease the timer by the time elapsed since last frame
            timeRemaining -= Time.deltaTime;

            // Convert timeRemaining to minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            // Update the timer text to display the countdown
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);

            // Check if the timer reaches zero
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                timerText.text = "00:00"; // Ensure the text shows 00:00 when it ends

                // Disable drawing functionality (disable the WhiteboardMarker)
                if (whiteboardMarker != null)
                {
                    whiteboardMarker.DisableDrawing(); // Call a method to stop drawing
                }

                isCountingDown = false; // Stop the timer from continuing to run
            }
        }
    }

    // Optional: Method to reset the timer to 6 minutes again
    public void ResetTimer()
    {
        timeRemaining = 360f; // Reset to 6 minutes
        isCountingDown = true;
        if (whiteboardMarker != null)
        {
            whiteboardMarker.EnableDrawing(); // Enable drawing again
        }
    }
}
