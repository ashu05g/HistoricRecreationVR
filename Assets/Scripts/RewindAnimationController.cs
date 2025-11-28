using UnityEngine;
using UnityEngine.UI;

public class RewindAnimationController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Slider to rewind the animation")]
    public Slider rewindSlider;

    [Header("Animation")]
    [Tooltip("Animator with your reconstruction animation clip")]
    public Animator reconstructionAnimator;
    [Tooltip("Name of the animation state in the Animator Controller")]
    public string animationStateName = "ReconstructAnim";

    public float MotionTimeValie =1.0f;
    private bool isAnimationCompleted = false; // Track if the animation is completed

    void Start()
    {
        // Hide the slider at the beginning
        if (rewindSlider != null)
        {
            rewindSlider.gameObject.SetActive(false);
            rewindSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Call this function when animation completes
    public void OnAnimationCompleted()
    {
        // Enable the slider once animation is completed
        isAnimationCompleted = true;

        if (rewindSlider != null)
        {
            rewindSlider.gameObject.SetActive(true); // Show the slider to allow rewinding
            rewindSlider.value = 1f; // Set the slider to the end (animation complete)
        }
    }

    // Function called when the slider value changes
    void OnSliderValueChanged(float value)
    {
        // Update the animation's normalized time based on slider value
        if (reconstructionAnimator != null && isAnimationCompleted)
        {
            MotionTimeValie=value;
            // Set the MotionTime parameter based on the slider value
            reconstructionAnimator.SetFloat("MotionTimevalue", value); // Control the animation’s motion time directly
            reconstructionAnimator.Update(0f);
        }

    }

    // Function to reset the slider and hide it
    public void ResetSlider()
    {
        if (rewindSlider != null)
        {
            rewindSlider.gameObject.SetActive(false); // Hide the slider again after reset
        }

        if (reconstructionAnimator != null)
        {
            // Reset animation to the beginning (pause the animation at start)
            reconstructionAnimator.Play(animationStateName, 0, 0f);
        }

        // Disable rewind functionality when reset
        isAnimationCompleted = false;
    }
}
