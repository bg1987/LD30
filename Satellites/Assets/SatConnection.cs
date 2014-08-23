using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SatConnection : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;
    public int myID;
    public HashSet<int> connectedSats;
	// Use this for initialization
	void Start () {
        connectedSats = new HashSet<int>();
        connectedSats.Add(myID);
	    //maybe some code that will make sure no satelites have the same ID.
	}
	


	// Update is called once per frame
	void Update () {
        Debug.DrawLine(startPosition.position, endPosition.position,Color.white);
        RaycastHit2D rcHit = Physics2D.Linecast(startPosition.position, endPosition.position,LayerMask.GetMask("Satellite")); //8 = satellite mask
        if(rcHit.collider == null){
            return;
        }
        Debug.Log("Ping!");
        SatConnection otherSate = rcHit.transform.gameObject.GetComponent<SatConnection>();
        if (otherSate)
        {
            connectedSats = otherSate.Connected(myID,connectedSats);
            printConnenctedSats();
        }
        
	}

    public HashSet<int> Connected(int otherID, HashSet<int> connected)
    {
        Debug.Log(string.Format("Sat {0} and {1} said HI!",myID,otherID));
        
        connectedSats.UnionWith(connected);
        printConnenctedSats();

        return connectedSats;

    }

    public void printConnenctedSats()
    {
        string tmp = "";
        foreach (int id in connectedSats)
        {
            tmp += id + " ";
        }
        Debug.Log(string.Format("Sat{0}, has talked to the following sats: {1}", myID, tmp));
    }
}

