using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{

    public Camera camera;
    private Vector3 lastMousePos;
    public float dragSensitivity = .004f;
    public float zoomSensitivity = .04f;
    public float maxZoom = 25;
    public float minZoom = 2;
    public bool scaleDragWithZoom = true;
	bool lockCameraToSelection = false;

	public GameObject selectedObject;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {

		float zoom = camera.orthographicSize;

		//Selection
		if (Input.GetMouseButtonDown (0)) {


			//Select object
			RaycastHit2D selection = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(selection != null && selection.collider != null && selection.collider.gameObject != null)
			{
				selectedObject = selection.collider.gameObject;
				lockCameraToSelection = true;
			}
			else
			{
				selectedObject = null;
				lockCameraToSelection = false;
				lastMousePos = Input.mousePosition;
			}
		}
		//Drag camera
		else if (Input.GetMouseButton(0) && !lockCameraToSelection)
        {
            Vector3 distanceToMove = (lastMousePos - Input.mousePosition);
            if(scaleDragWithZoom)
                distanceToMove.Scale(new Vector3(dragSensitivity * zoom, dragSensitivity * zoom, dragSensitivity * zoom));
            else
                distanceToMove.Scale(new Vector3(dragSensitivity, dragSensitivity, dragSensitivity));


			camera.transform.position = camera.transform.position + distanceToMove;
            lastMousePos = Input.mousePosition;
        }
		//Zoom
		else if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        } else if (Input.GetMouseButton(1))
        {
            zoom += (lastMousePos - Input.mousePosition).y * zoomSensitivity;
            lastMousePos = Input.mousePosition;
        }

		//Set min/max zoom
        if (zoom < minZoom)
        {
            zoom = minZoom;
        } else if (zoom > maxZoom)
        {
            zoom = maxZoom;
        }


		
		if(lockCameraToSelection)
			camera.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, -zoom / 10);
		camera.orthographicSize = zoom;
	}

	void OnGUI () {
		GUI.Window (1, new Rect (0, 0, 160, 240), drawSelectionWindow, "Selection");
	}

		void drawSelectionWindow(int id)
		{
			if (selectedObject != null) 
			{
				GUI.DrawTexture (new Rect (30, 30, 100, 100), ((SpriteRenderer)selectedObject.gameObject.GetComponent (typeof(SpriteRenderer))).sprite.texture);
				GUI.Label(new Rect (10, 140, 140, 20), "Object: " + selectedObject.name);
				GUI.Label(new Rect (10, 160, 140, 20), "Speed: " + ((Orbit)selectedObject.gameObject.GetComponent (typeof(Orbit))).rotationSpeed);
			}

		}
}
