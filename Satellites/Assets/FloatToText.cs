using UnityEngine;
using System.Collections;

public class FloatToText : MonoBehaviour {

    public UnityEngine.UI.Text txt;

    public void UpdateValue(float value)
    {
        txt.text = value.ToString();
    }
}
