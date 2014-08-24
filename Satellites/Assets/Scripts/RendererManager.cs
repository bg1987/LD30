using UnityEngine;
using System.Collections;

public class RendererManager : MonoBehaviour {

	public LineRenderer lines;
	public TrailRenderer trails;
	public int LodCount = 42;
	public Orbit orbitPreview;

	// Use this for initialization
	public void Start () {
		lines.sortingLayerName = "Orbit";
		trails.sortingLayerName = "Trail";

		lines.SetVertexCount (LodCount + 1);
	}
	
	// Update is called once per frame
	public void Update () {

		for(int i = 0; i < LodCount + 1; i++)
		{
			float angle = i * (Mathf.PI * 2) / LodCount;
			if(transform.parent){
			lines.SetPosition(i, new Vector3(Mathf.Cos(angle) * orbitPreview.radius + transform.parent.transform.position.x, 
			                                 Mathf.Sin(angle) * orbitPreview.radius + transform.parent.transform.position.y, 
			                                 0));
			}
		}
	}
}
