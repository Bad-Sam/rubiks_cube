using UnityEngine;
using UnityEngine.UI;

public class GenerateButton : MonoBehaviour
{
    [SerializeField]
    Slider sizeSlider = null;

    [SerializeField]
    Slider shuffleSlider = null;

    [SerializeField]
    RubiksCube rubiksCube = null; 

    void Awake()
    {
        Debug.Assert(sizeSlider != null);
        Debug.Assert(shuffleSlider != null);
        Debug.Assert(rubiksCube != null);
    }

    
    public void UpdateRubiksCube()
    {
        rubiksCube.Generate((int)Mathf.Round(sizeSlider.value), (int)Mathf.Round(shuffleSlider.value));
    }
}
