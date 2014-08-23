using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SatConnection : MonoBehaviour {
    private static int ID_GENERATOR = 1;

    public Transform startPosition;
    public Transform endPosition;
    public float radius = 10;
    public int myID;
    public HashSet<int> connectedSats;

    void Awake()
    {
        myID = ID_GENERATOR;
        ID_GENERATOR++;
    }
    
    

	// Use this for initialization
	void Start () {
        connectedSats = new HashSet<int>();
        connectedSats.Add(myID);
	    //maybe some code that will make sure no satelites have the same ID.
	}
	


	// Update is called once per frame
	void Update () {
        /*
        Debug.DrawLine(startPosition.position, endPosition.position,Color.white);
        
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0.1f, LayerMask.GetMask("Satellite"));



        Debug.Log(hits.Length);
        RaycastHit2D rcHit = Physics2D.Linecast(startPosition.position, endPosition.position,LayerMask.GetMask("Satellite")); //8 = satellite mask
        if(rcHit.collider == null){
            return;
        }
        //Debug.Log("Ping!");
        SatConnection otherSate = rcHit.transform.gameObject.GetComponent<SatConnection>();
        if (otherSate)
        {
            connectedSats = otherSate.Connected(myID,connectedSats);
            printConnenctedSats();
        }
        */
        CheckConnection(new HashSet<int>());
        printConnenctedSats();
        
	}


    //Checks for all the satelites in a radius and makes sure they are in an unbostructed line
    //Then calls recrusively on all new connected (the ones that arent in connectedSats)
    public HashSet<int> CheckConnection(HashSet<int> preConnectedSats)
    {
        RaycastHit2D[] satsInRange = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0.1f, LayerMask.GetMask("Satellite"));
        preConnectedSats.Add(myID);
        foreach (RaycastHit2D sat in satsInRange)
        {
            SatConnection satCon = sat.transform.gameObject.GetComponent<SatConnection>();
            if (preConnectedSats.Contains(satCon.myID))
            {
                continue;//skip the already connected sats.
            }

            //if there is a direct line between the satelites
            if (!Physics2D.Linecast(transform.position,sat.transform.position,LayerMask.GetMask("Planet")))
            {
                Debug.DrawLine(transform.position, sat.transform.position, Color.green);
                preConnectedSats.Add(satCon.myID);
                preConnectedSats.UnionWith(satCon.CheckConnection(preConnectedSats));
            }

        }
        connectedSats = preConnectedSats;
        return preConnectedSats;
    }


    public HashSet<int> Connected(int otherID, HashSet<int> connected)
    {
        //Debug.Log(string.Format("Sat {0} and {1} said HI!",myID,otherID));
        
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

