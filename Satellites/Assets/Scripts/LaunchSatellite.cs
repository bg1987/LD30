using UnityEngine;
using System.Collections;

public class LaunchSatellite : MonoBehaviour {
    public GameObject prefab = null;
    public GameObject cam = null;

    //Creates a new satellite orbiting selected planet
    public void Launch()
    {

        GameObject selected = cam.GetComponent<DragCamera>().selectedObject;

        if (!selected || selected.tag == "Satellite")//cant launch sats around null or around other sats.
        {
            return;
        }
        //TODO: Name Sats.
        GameObject newSat = Instantiate(prefab,selected.transform.position,Quaternion.identity) as GameObject;
        
        
        Orbit orbit = newSat.GetComponent<Orbit>();
        orbit.radius = 5.0f;
        orbit.radiusSpeed = 30.0f;
        orbit.rotationSpeed = 20f;

        orbit.center = selected.transform;

        newSat.transform.parent = selected.transform;
       
        

    }
}
