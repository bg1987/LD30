using UnityEngine;
using System.Collections;

public class EnableObject : MonoBehaviour {

    public GameObject obj;

    public void EnableObj()
    {
        obj.SetActive(true);
    }

}
