using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DebugSelectSimulator : MonoBehaviour
{
    public XRGrabInteractable interactable;

    void Update()
    {
        // Simulate Select Enter with Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Simulating Select Enter");
            interactable.selectEntered.Invoke(new SelectEnterEventArgs());
        }

        // Simulate Select Exit with Backspace key
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Simulating Select Exit");
            interactable.selectExited.Invoke(new SelectExitEventArgs());
        }
    }
}
