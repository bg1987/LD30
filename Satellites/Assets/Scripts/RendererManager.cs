using UnityEngine;
using System.Collections;

public class RendererManager : MonoBehaviour {

	public LineRenderer lines;
	public TrailRenderer trails;
	public int LodCount = 42;
	public Orbit orbitPreview;

	// Use this for initialization
	public void Start () {
		if (lines != null) 
		{
			lines.sortingLayerName = "Orbit";			
			lines.SetVertexCount (LodCount + 1);
		}
		if (trails != null) 
		{
			trails.sortingLayerName = "Trail";
		}
	}
	
	// Update is called once per frame
	public void Update () {
		if(lines != null)
			for(int i = 0; i < LodCount + 1; i++)
			{
				float angle = i * (Mathf.PI * 2) / LodCount;
				lines.SetPosition(i, new Vector3(Mathf.Cos(angle) * orbitPreview.radius + transform.parent.transform.position.x, 
				                                 Mathf.Sin(angle) * orbitPreview.radius + transform.parent.transform.position.y, 
				                                 0));
			}
	}
}
