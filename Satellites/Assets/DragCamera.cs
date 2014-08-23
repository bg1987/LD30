using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{

    public Camera cameraTransform;
    public Vector3 lastMousePos;
    public float dragSensitivity = .004f;
    public float zoomSensitivity = .04f;
    public float maxZoom = 15;
    public float minZoom = 2;
    public bool scaleDragWithZoom = true;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {

        float zoom = cameraTransform.orthographicSize;

        //Drag camera
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        } else if (Input.GetMouseButton(0))
        {
            Vector3 distanceToMove = (lastMousePos - Input.mousePosition);
            if(scaleDragWithZoom)
                distanceToMove.Scale(new Vector3(dragSensitivity * zoom, dragSensitivity * zoom, dragSensitivity * zoom));
            else
                distanceToMove.Scale(new Vector3(dragSensitivity, dragSensitivity, dragSensitivity));


            cameraTransform.transform.position = cameraTransform.transform.position + distanceToMove;
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

        cameraTransform.orthographicSize = zoom;

    }
}
