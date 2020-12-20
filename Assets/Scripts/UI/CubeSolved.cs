using UnityEngine;
using UnityEngine.UI;

public class CubeSolved : MonoBehaviour
{
    private Text text = null;

    void Start()
    {
        text = GetComponent<Text>();
        text.color = new Color(.902f, .6275f, .2118f, 1f);
        text.CrossFadeAlpha(0f, 0f, true);
    }

    public void Trigger()
    {
        text.CrossFadeAlpha(1f, 0f, true);
        text.CrossFadeAlpha(0f, 3f, true);
    }
}
