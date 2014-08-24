using UnityEngine;
using System.Collections;

public class LaunchSatellite : MonoBehaviour {
    public GameObject prefab = null;
    public GameObject cam = null;

    //Creates a new satellite orbiting selected planet
    public void Launch()
    {

        GameObject selected = cam.GetComponent<DragCamera>().selectedObject;

        if (!selected)//cant launch sats around null or around other sats.
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
        orbit.radius = 5.0f;
        orbit.radiusSpeed = 30.0f;
        orbit.rotationSpeed = 20f;

        orbit.center = selected.transform;

        newSat.transform.parent = selected.transform;
       
        

    }
}
