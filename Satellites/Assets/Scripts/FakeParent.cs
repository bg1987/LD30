using UnityEngine;
using System.Collections;

public class FakeParent : MonoBehaviour {

	public GameObject fakeParent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = fakeParent.transform.position;
	}
}
