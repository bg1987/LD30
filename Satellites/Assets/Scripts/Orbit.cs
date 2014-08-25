using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public Transform center;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

	public float baseRadius = 5;
	public float baseCost = 5;
	public float additionalRadiusCost = 1;
	public float minRadius = 3;

    public bool logLapTime = false;
    private float lapStartTime = 0;
    void Start()
    {
        //transform.position = (transform.position - center.position).normalized * radius + center.position;
    }

    void Update()
    {
		if (center != null) {
			transform.RotateAround (center.position, axis, rotationSpeed * Time.deltaTime);
			desiredPosition = (transform.position - center.position).normalized * radius + center.position;
			transform.position = Vector3.MoveTowards (transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
		}

        if (Mathf.RoundToInt(transform.position.y) == 0)
        {
            if (lapStartTime == 0)
            {
                lapStartTime = Time.time;
            }
            else
            {
                if (logLapTime)
                {
                Debug.Log(string.Format("{0} took {1} seconds to complete half a lap", gameObject.name, Time.time - lapStartTime));        
                }
                lapStartTime = 0;

                
            }
        }
    }
}