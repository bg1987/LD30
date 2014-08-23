using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public GameObject orbiting;
    public float radius = 5.0f;
    public float rotationSpeed = 1.0f;

    void Start()
    {
        //transform.position = (transform.position - center.position).normalized * radius + center.position;
    }

    void Update()
    {
		transform.position = orbiting.transform.position + GetOffset(Time.time);
    }

	Vector3 GetOffset(float time)
	{
		float xOffset;
		float yOffset;

		xOffset = radius * Mathf.Cos (time * rotationSpeed);
		yOffset = radius * Mathf.Sin (time * rotationSpeed);

		return new Vector3(xOffset, yOffset, 0);

	}
}