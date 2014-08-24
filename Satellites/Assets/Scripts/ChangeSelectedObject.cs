using UnityEngine;
using System.Collections;

public class ChangeSelectedObject : MonoBehaviour {

    public Orbit selectedObjectOrbit;

    public void ChangeRadius(float value)
    {
        if (selectedObjectOrbit)
        {
            selectedObjectOrbit.radius = value;
        }
    }

    public void ChangeSpeed(float value)
    {
        if (selectedObjectOrbit)
        {
            selectedObjectOrbit.rotationSpeed = value;
        }
    }
}
