using UnityEngine;
using System.Collections;

public class GameSpeed : MonoBehaviour {
    public UnityEngine.UI.Text speedText;
	

    public void SetSpeed(float speed)
    {
        speedText.text = speed.ToString();
        Time.timeScale = speed;
    }
}
