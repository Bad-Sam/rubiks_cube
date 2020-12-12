using UnityEngine;
using UnityEngine.UI;

public class InstructionsText : MonoBehaviour
{
    [SerializeField]
    Text    buttonText      = null;

    [SerializeField]
    Text    instructions    = null;

    bool    shown           = false;

    void Start()
    {
        instructions.enabled = false;
    }

    // Update is called once per frame
    public void ToggleInstructions()
    {
        shown = !shown;
        buttonText.text = shown ? "Hide instructions" : "Show instructions";
        instructions.enabled = shown;
    }
}
