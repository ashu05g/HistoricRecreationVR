using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using static UnityEngine.Rendering.DebugUI;

public class SphinxReconstructionController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The VR button that triggers reconstruction")]
    public Button reconstructButton;
    [Tooltip("The VR button that resets the scene")]
    public Button resetButton;
    [Tooltip("Slider to rewind the animation")]
    public Slider rewindSlider;

    [Header("Model References")]
    [Tooltip("Your original broken Sphinx model")]
    public GameObject oldSphinx;
    [Tooltip("Your animation object for the reconstruction")]
    public GameObject animationObject;
    [Tooltip("Your animation object for the reconstruction")]
    public GameObject animationObject2;
    [Tooltip("Your fully reconstructed Sphinx model (initially disabled)")]
    public GameObject newSphinx;

    [Header("Animation")]
    [Tooltip("Animator with your reconstruction animation clip")]
    public Animator reconstructionAnimator;
    [Tooltip("Animator with your reconstruction animation clip")]
    public Animator reconstructionAnimator2;
    [Tooltip("Name of the animation state in the Animator Controller")]
    public string animationStateName = "ReconstructAnim";

    [Range(0.1f, 3f)]
    [Tooltip("Playback speed: 1 = normal, <1 = slower, >1 = faster")]
    public float animationSpeed = 1f;

    [Header("Audio")]
    [Tooltip("AudioSource that plays along with the animation")]
    public AudioSource reconstructionAudio;

    RewindAnimationController rewindAnimationController;

    void Start()
    {
        if (resetButton != null)
            resetButton.gameObject.SetActive(false);
        if (rewindSlider != null)
            rewindSlider.gameObject.SetActive(false);

        if (oldSphinx != null)
            oldSphinx.SetActive(true);
        if (animationObject != null)
            animationObject.SetActive(false);
        if (animationObject2 != null)
            animationObject2.SetActive(false);
        if (newSphinx != null)
            newSphinx.SetActive(false);

        if (reconstructButton != null)
            reconstructButton.onClick.AddListener(OnReconstructClicked);

        if (reconstructionAnimator != null)
            reconstructionAnimator.speed = 0f;

        if (reconstructionAudio != null)
            reconstructionAudio.playOnAwake = false;

        rewindAnimationController = gameObject.GetComponent<RewindAnimationController>();
    }

    void OnReconstructClicked()
    {
        reconstructButton.interactable = false;

        if (oldSphinx != null)
            oldSphinx.SetActive(true);
        if (animationObject != null)
            animationObject.SetActive(true);

        float animClipLength = 1f;
        float finalAnimDuration = 1f;

        // 1. Get actual animation length
        if (reconstructionAnimator != null && reconstructionAnimator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in reconstructionAnimator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationStateName)
                {
                    animClipLength = clip.length;
                    break;
                }
            }

            // 2. Set animator speed
            reconstructionAnimator.speed = animationSpeed;

            // 3. Calculate actual animation duration (after speed adjustment)
            finalAnimDuration = animClipLength / animationSpeed;

            // 4. Play the animation
            reconstructionAnimator.Play(animationStateName, 0, 0f);
        }

        // 5. Adjust audio pitch to match animation duration
        if (reconstructionAudio != null && reconstructionAudio.clip != null)
        {
            float originalAudioLength = reconstructionAudio.clip.length;

            // We want: audio_length / pitch = finalAnimDuration => pitch = audio_length / finalAnimDuration
            float pitch = originalAudioLength / finalAnimDuration;

            reconstructionAudio.pitch = pitch;
            reconstructionAudio.Play();
        }

        // 6. Wait for animation to finish
        StartCoroutine(WaitForAnimationEnd(finalAnimDuration));
    }

    IEnumerator WaitForAnimationEnd(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (reconstructionAudio != null && reconstructionAudio.isPlaying)
            reconstructionAudio.Stop();

        

        rewindAnimationController.OnAnimationCompleted();

        if (animationObject2 != null)
        {
            // Show the object with MotionTime but DO NOT play the animation
            animationObject2.SetActive(true); // Make the object visible

            // Ensure the animation is paused and set to the final frame
            reconstructionAnimator2.Play(animationStateName, 0, 1f); // Set normalized time to 1 (end of animation)

            // Ensure the animation stays paused and does not resume playing
            reconstructionAnimator2.Update(0f); // Force the animator to stay at the current time (no updates)

            reconstructionAnimator.SetFloat("MotionTime", 1f);

        }
        if (animationObject != null)
            animationObject.SetActive(false);

        resetButton.gameObject.SetActive(true);
        rewindSlider.gameObject.SetActive(true);
        rewindSlider.interactable = true;
        reconstructButton.gameObject.SetActive(false);
    }
}
