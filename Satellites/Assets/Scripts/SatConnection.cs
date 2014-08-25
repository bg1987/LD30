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
	public List<LineRenderer> lines = new List<LineRenderer>(1);
	public int currentLinerenderer = 0;

    void Awake()
    {
        myID = ID_GENERATOR;
        ID_GENERATOR++;
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
            if (!Physics2D.Linecast(transform.position,sat.transform.position,LayerMask.GetMask("Planet"))
			    && !Physics2D.Linecast(transform.position,sat.transform.position,LayerMask.GetMask("StartingPlanet")))
            {
                //Debug.DrawLine(transform.position, sat.transform.position, Color.green);
				if(currentLinerenderer >= lines.Count)
				{
					LineRenderer exampleLine = ((LineRenderer)GameObject.FindGameObjectWithTag("Satellite").GetComponent("LineRenderer"));
					LineRenderer newLine = new GameObject().AddComponent("LineRenderer") as LineRenderer;
					newLine.sortingLayerName = "Orbit";
					newLine.name = "LineConnector";
					newLine.SetWidth(0.2f, 0.2f);
					newLine.material = exampleLine.material;					
					Color colors = Color.green;
					newLine.SetColors(colors, colors);
					newLine.SetVertexCount(2);
					newLine.transform.parent = transform;
					lines.Add(newLine);
				}

				//Debug.Log(currentLinerenderer);


				LineRenderer lRend = lines[currentLinerenderer];
				lRend.enabled = true;
				lRend.SetPosition(0, transform.position);
				lRend.SetPosition(1, sat.transform.position);
				lRend.transform.parent = transform;

                preConnectedSats.Add(satCon.myID);
                preConnectedSats.UnionWith(satCon.CheckConnection(preConnectedSats));
				currentLinerenderer++;
            }
        }

        connectedSats = preConnectedSats;
        return preConnectedSats;
    }

	IEnumerator Start() {
		connectedSats = new HashSet<int>();
		connectedSats.Add(myID);
		while (true) 
		{
			yield return new WaitForEndOfFrame ();
			if(lines == null || lines.Count == 0)
			{
				lines = new List<LineRenderer>();
			}
			foreach(LineRenderer rend in lines) {
				rend.enabled = false;
			}
			currentLinerenderer = 0;
		}
	}
	/*
	public void OnDrawGizmos()
	{
			//yield return new WaitForEndOfFrame();
			
			Debug.Log("Draw numer");

			Gizmos.DrawLine (transform.position, new Vector3(0, 0, 0));
			//Gizmos.


			GL.PushMatrix();
			
			GL.Begin( GL.LINES );



			foreach(int id in connectedSats)
			{

				//Transform sat = transform.Find("Sat_" + id);

				
				//float dist = Vector3.Distance(transform.position, sat.position);
				
				GL.Color(new Color(0,1,0,1) );
				GL.Vertex(transform.position);
				GL.Color(new Color(1 / 100,1,0,1) );
				GL.Vertex3(0, 0, 0);

			}

			
			GL.End();
			
			GL.PopMatrix ();

	}
	*/


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
        //Debug.Log(string.Format("Sat{0}, has talked to the following sats: {1}", myID, tmp));
    }
}

