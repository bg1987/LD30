using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
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
	Vector3 lockedOffset = new Vector3();
	public int scrollSensitivity = 100;

	public PlayUISound uiSound;

	public GameObject selectedObject;

    GameObject launchButton = null;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {



        
		float zoom = camera.orthographicSize;

		//Do nothing here if the mouse is clicked over a UI element.
        if (!((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && EventSystemManager.currentSystem.IsPointerOverEventSystemObject()))
        {
            //Selection
		    if (Input.GetMouseButtonDown (0)) {

			    lockedOffset = new Vector3(0, 0, 0);

			    //Select object
			    RaycastHit2D selection = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            

			    if(selection != null && selection.collider != null && selection.collider.gameObject != null)
			    {
				    selectedObject = selection.collider.gameObject;
				    lockCameraToSelection = true;
				    uiSound.PlayRandomSwitchClip();
                
                    SetSelectedImage();
                
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

			    lockedOffset = new Vector3(0, 0, 0);

                Vector3 distanceToMove = (lastMousePos - Input.mousePosition);
                if(scaleDragWithZoom)
                    distanceToMove.Scale(new Vector3(dragSensitivity * zoom, dragSensitivity * zoom, dragSensitivity * zoom));
                else
                    distanceToMove.Scale(new Vector3(dragSensitivity, dragSensitivity, dragSensitivity));


			    camera.transform.position = camera.transform.position + distanceToMove;
                lastMousePos = Input.mousePosition;
            }
		    //Offset
		    else if(Input.GetMouseButtonDown(1))
		    {			
			    lastMousePos = Input.mousePosition;
		    }
		    else if (Input.GetMouseButton(1))
            {
			    if(scaleDragWithZoom)
			    {
				    Vector3 tempPos = (lastMousePos - Input.mousePosition);
				    tempPos.Scale(new Vector3(dragSensitivity * zoom, dragSensitivity * zoom, dragSensitivity * zoom));
				    lockedOffset += tempPos;
			    }
			    else
				    lockedOffset += (lastMousePos - Input.mousePosition);

			    lastMousePos = Input.mousePosition;
            }
        }

		//Zoom
		zoom -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

		//Set min/max zoom
        if (zoom < minZoom)
        {
            zoom = minZoom;
        } else if (zoom > maxZoom)
        {
            zoom = maxZoom;
        }


		
		if(lockCameraToSelection)
			camera.transform.position = new Vector3(selectedObject.transform.position.x + lockedOffset.x, selectedObject.transform.position.y + lockedOffset.y, camera.transform.position.z);

		camera.orthographicSize = zoom;
	}

    //TODO: unselect when clicked outside
    
    void SetSelectedImage()
    {
        GameObject img = GameObject.Find("SelectionImage");
        UnityEngine.UI.Image com = img.GetComponent<UnityEngine.UI.Image>();
        com.sprite = selectedObject.GetComponent<SpriteRenderer>().sprite;
        /*
        GameObject b = GameObject.Find("SatPanel");
        GameObject canvas = GameObject.Find("Canvas");
        
        GameObject panel = Instantiate(original, new Vector3(300, 100), Quaternion.identity) as GameObject;
        panel.transform.parent = canvas.transform;
        Debug.Log(b.transform.position);

         */
        GameObject original = GameObject.Find("SatPanel");


        Debug.Log("Satellite selected");
        //Show/Create the SatPanel thing
        GameObject radSlide = original.transform.Find("Sliders Panel/RadiusSlider").gameObject;
        GameObject speedSlide = original.transform.Find("Sliders Panel/SpeedSlider").gameObject;
        UnityEngine.UI.Text NameText = original.transform.Find("Name Plate/NameText").gameObject.GetComponent<UnityEngine.UI.Text>();

        NameText.text = selectedObject.name;



        GameObject tmp = original.transform.Find("Name Plate/NameText").gameObject;
        UnityEngine.UI.Text tmp2 = tmp.GetComponent<UnityEngine.UI.Text>();
        tmp2.text = selectedObject.name;

        radSlide.GetComponent<ChangeSelectedObject>().selectedObjectOrbit = selectedObject.GetComponent<Orbit>();
        speedSlide.GetComponent<ChangeSelectedObject>().selectedObjectOrbit = selectedObject.GetComponent<Orbit>();

        SetGUISlider(radSlide, selectedObject.GetComponent<Orbit>().radius);
        SetGUISlider(speedSlide, selectedObject.GetComponent<Orbit>().rotationSpeed);

        
        if (launchButton != null)
        {
            Debug.Log(launchButton.transform.position);
        }
        //if its not a satellite:
        if (!(selectedObject.tag == "Satellite"))
        {
            //Disable sliders
            //add a launch satellite button.
            if (launchButton == null)
            {

                //launchButton = Instantiate(Resources.Load("SatLaunch")) as GameObject;
                //launchButton.transform.parent = original.transform;
                //launchButton.transform.position = new Vector3(270,22);
            }

        }
        else
        {
            if (launchButton != null)
            {
                Destroy(launchButton);    
            }
            
        }
        
    }

    void SetGUISlider(GameObject sliderObj, float value)
    {
        UnityEngine.UI.Slider slider = sliderObj.GetComponent<UnityEngine.UI.Slider>();
        slider.value = value;
    }
}
