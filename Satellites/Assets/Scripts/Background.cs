using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public GameObject background;
	public GameObject camera;
	public float scale = 0.5f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
        if (!camera)
        {
            camera = GameObject.Find("Main Camera");
        }
		background.transform.position = new Vector3(camera.transform.position.x * scale, camera.transform.position.y * scale, background.transform.position.z);
	}
}
