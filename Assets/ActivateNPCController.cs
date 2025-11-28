using UnityEngine;

public class ActivateNPCController : MonoBehaviour
{
    [SerializeField]
    private STT_HF_OpenAI STT_HF_OpenAI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (STT_HF_OpenAI)
        {
            STT_HF_OpenAI.StartSpeaking();
        }
        else
        {
            Debug.Log("STT_HF not found in NPCCOntroller");
        }

    }
}
    
