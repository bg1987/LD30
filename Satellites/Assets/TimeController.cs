using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {
    public void SetSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
