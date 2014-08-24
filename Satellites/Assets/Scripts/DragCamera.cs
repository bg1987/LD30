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
                    GlobalObjects.Instance.SelectedObject = selection.collider.gameObject;
                    SetSelectedImage();
                
			    }
			    else
			    {
                    ClearSelectedImage();
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

    void ClearSelectedImage() 
    {
        SetSatPanel(null,"",null);
    }

    void SetSelectedImage()
    {

        SetSatPanel(selectedObject.GetComponent<SpriteRenderer>().sprite, selectedObject.name, selectedObject.GetComponent<Orbit>());

        if (launchButton != null)
        {
            Debug.Log(launchButton.transform.position);
        }
        //if its not a satellite:
        if (!(selectedObject.tag == "Satellite"))
        {
            //Disable sliders

        }
        else
        {
            
        }
        
    }

    void SetSatPanel(Sprite sprite, string name, Orbit orbit) 
    {
        GameObject img = GameObject.Find("SelectionImage");
        UnityEngine.UI.Image com = img.GetComponent<UnityEngine.UI.Image>();

        if (sprite)
        {
            com.sprite = sprite;
        }
        else
        {
            Debug.Log("Def Sprite");
            com.sprite = GlobalObjects.Instance.DefaultSelectionSprite;
        }
        
        
        GameObject original = GameObject.Find("SatPanel");


        Debug.Log("Satellite selected");
        //Show/Create the SatPanel thing
        GameObject radSlide = original.transform.Find("Sliders Panel/RadiusSlider").gameObject;
        GameObject speedSlide = original.transform.Find("Sliders Panel/SpeedSlider").gameObject;
        UnityEngine.UI.Text NameText = original.transform.Find("Name Plate/NameText").gameObject.GetComponent<UnityEngine.UI.Text>();

        NameText.text = name;

        radSlide.GetComponent<ChangeSelectedObject>().selectedObjectOrbit = orbit;
        speedSlide.GetComponent<ChangeSelectedObject>().selectedObjectOrbit = orbit;

        if (orbit)
        {
            SetGUISlider(radSlide, orbit.radius);
            SetGUISlider(speedSlide, orbit.rotationSpeed);

        }
        else
        {
            SetGUISlider(speedSlide,0);
            SetGUISlider(radSlide,0);
        }
    }



    void SetGUISlider(GameObject sliderObj, float value)
    {
        UnityEngine.UI.Slider slider = sliderObj.GetComponent<UnityEngine.UI.Slider>();
        slider.value = value;
    }
}
