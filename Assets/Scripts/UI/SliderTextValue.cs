using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValue : MonoBehaviour
{
    [SerializeField]
    Slider  slider = null;
    Text    text;

    void Start()
    {
        text    = GetComponent<Text>();
        slider  = GetComponentInParent<Slider>();

        UpdateText();
    }

    public void UpdateText()
    {
        text.text = slider.value.ToString();
    }
}
