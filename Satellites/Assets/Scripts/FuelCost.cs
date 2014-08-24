using UnityEngine;
using System.Collections;

public class FuelCost : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public float GetFuelCost()
	{

		Orbit orbit = (Orbit) GetComponent ("Orbit");
		Orbit orbitParent = (Orbit) transform.parent.GetComponent("Orbit");

		float baseCost = orbitParent.baseCost;
		float baseRadius = orbitParent.baseRadius;
		float radiusCost = orbitParent.additionalRadiusCost;

		float curRadius = orbit.radius;
		float curSpeed = orbit.rotationSpeed;

		float cost;

		cost = baseCost + curSpeed + Mathf.Abs (curRadius - baseRadius) * radiusCost;

		return cost;

	}
}
