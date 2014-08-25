using UnityEngine;
using System.Collections;

public class LaunchSatellite : MonoBehaviour {
    public GameObject prefab = null;
    public GameObject cam = null;

    //Creates a new satellite orbiting selected planet
    public void Launch()
    {

		GameObject selected = GlobalObjects.Instance.SelectedObject;

        if (!selected || selected.tag == "SpaceJunk" || selected.tag == "StartingSatellite")//cant launch sats around null or around other sats.
        {
            return;
        }
        if (selected.tag == "Satellite")
        {
            Destroy(selected);
        }
        Debug.Log(selected.tag);
        //TODO: Name Sats.
        GameObject newSat = Instantiate(prefab,selected.transform.position,Quaternion.identity) as GameObject;

        SatConnection satCon = newSat.GetComponent<SatConnection>();
        newSat.name = "Sat_" + satCon.myID;

        Orbit orbit = newSat.GetComponent<Orbit>();
		orbit.radius = selected.GetComponent<Orbit> ().baseRadius;
        orbit.radiusSpeed = 30.0f;
        orbit.rotationSpeed = 10f;

        orbit.center = selected.transform;

        newSat.transform.parent = selected.transform;

		newSat.transform.position = GameObject.FindGameObjectWithTag("StartingPlanet").transform.position + new Vector3 (0, 1, 0);

        newSat.transform.localScale = Vector3.one*0.5f;
    }
}
