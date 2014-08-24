using UnityEngine;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadScene(string Name)
    {
        Application.LoadLevel(name);
    }

    public void LoadScene(int id)
    {
        Application.LoadLevel(id);
    }
}
