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

	public GameObject selectedObject;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {

		float zoom = camera.orthographicSize;

        //Drag camera
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;


			
			//Select object
			RaycastHit2D selection = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			Debug.Log (selection.collider.gameObject.name);
			selectedObject = selection.collider.gameObject;

        } else if (Input.GetMouseButton(0))
        {
            Vector3 distanceToMove = (lastMousePos - Input.mousePosition);
            if(scaleDragWithZoom)
                distanceToMove.Scale(new Vector3(dragSensitivity * zoom, dragSensitivity * zoom, dragSensitivity * zoom));
            else
                distanceToMove.Scale(new Vector3(dragSensitivity, dragSensitivity, dragSensitivity));


			camera.transform.position = camera.transform.position + distanceToMove;
            lastMousePos = Input.mousePosition;
        } else if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        } else if (Input.GetMouseButton(1))
        {
            zoom += (lastMousePos - Input.mousePosition).y * zoomSensitivity;
            lastMousePos = Input.mousePosition;
        }


        if (zoom < minZoom)
        {
            zoom = minZoom;
        } else if (zoom > maxZoom)
        {
            zoom = maxZoom;
        }

		camera.orthographicSize = zoom;
    }

	void OnGUI () {
		GUI.Window (1, new Rect (0, 0, 100, 100), drawSelectionWindow, "Selection");
	}

		void drawSelectionWindow(int id)
		{
			GUI.DrawTexture (new Rect (0, 0, 100, 100), ((SpriteRenderer)selectedObject.gameObject.GetComponent (typeof (SpriteRenderer))).sprite.texture);
		}
}
