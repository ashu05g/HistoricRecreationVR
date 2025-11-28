using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SphinxResetController : MonoBehaviour
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
    
    [Tooltip("Your fully reconstructed Sphinx model (initially disabled)")]
    public GameObject newSphinx;
    [Tooltip("Your animation object for the reconstruction")]
    public GameObject animationObject;

    [Tooltip("Fade duration for reset")]
    public float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetClicked);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    void OnResetClicked()
    {
        if (oldSphinx != null)
            oldSphinx.SetActive(true);
        if (newSphinx != null)
            newSphinx.SetActive(false);
        if (animationObject != null)
            animationObject.SetActive(false);


        if (reconstructButton != null)
            reconstructButton.gameObject.SetActive(true);
        // Reset the reconstruction button and animation
        if (reconstructButton != null)
            reconstructButton.interactable = true;

        if (resetButton != null)
            resetButton.gameObject.SetActive(false);
        rewindSlider.gameObject.SetActive(false);
    }

}
