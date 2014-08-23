using UnityEngine;
using System.Collections;

public class SatConnection : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;
    public bool connected;
	// Use this for initialization
	void Start () {
	    
	}
	


	// Update is called once per frame
	void Update () {
        Debug.DrawLine(startPosition.position, endPosition.position,Color.white);
        connected = Physics2D.Linecast(startPosition.position, endPosition.position,LayerMask.GetMask("Satellite")); //8 = satellite mask
        if (connected)
        {
            Debug.Log("PING!");
        }
	}
}
