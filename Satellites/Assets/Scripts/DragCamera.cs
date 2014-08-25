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


	public Background bkg;

	public PlayUISound uiSound;

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
				    lockCameraToSelection = true;
				    uiSound.PlayRandomSwitchClip();
                    GlobalObjects.Instance.SelectedObject = selection.collider.gameObject;
                    SetSelectedImage();
                
			    }
			    else
			    {
                    ClearSelectedImage();
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
		zoom += Input.GetKeyDown (KeyCode.Equals) ? scrollSensitivity / 4 : 0;
		zoom -= Input.GetKeyDown (KeyCode.Minus) ? scrollSensitivity / 4 : 0;

		//Set min/max zoom
        if (zoom < minZoom)
        {
            zoom = minZoom;
        } else if (zoom > maxZoom)
        {
            zoom = maxZoom;
        }


		
		if(lockCameraToSelection)
			camera.transform.position = new Vector3(GlobalObjects.Instance.SelectedObject.transform.position.x + lockedOffset.x, 
			                                        GlobalObjects.Instance.SelectedObject.transform.position.y + lockedOffset.y, 
			                                        camera.transform.position.z);

		camera.orthographicSize = zoom;

		int xLimit = (int) (512 * 0.70710678118f);
		int yLimit = (int) (512 * 0.70710678118f);

		if (camera.transform.position.x > xLimit) {
			camera.transform.position = new Vector3(xLimit, camera.transform.position.y, camera.transform.position.z);
		}
		else if (camera.transform.position.x < -xLimit) {
			camera.transform.position = new Vector3(-xLimit, camera.transform.position.y, camera.transform.position.z);
		}

		if (camera.transform.position.y > yLimit) {
			camera.transform.position = new Vector3 (camera.transform.position.x, yLimit, camera.transform.position.z);
		}
		else if (camera.transform.position.y < -yLimit) {
			camera.transform.position = new Vector3(camera.transform.position.x, -yLimit, camera.transform.position.z);
		}



		if (Input.GetKeyDown (KeyCode.Tab)) {

			Transform parentTrans = GlobalObjects.Instance.SelectedObject.transform.parent;

			int thisPosInParentList = 0;
			for(int i = 0; i < parentTrans.childCount; i++)
			{
				if(parentTrans.GetChild(i) == GlobalObjects.Instance.SelectedObject.transform)
				{
					thisPosInParentList = i;
					break;
				}
			}

			thisPosInParentList += 1;

			if(thisPosInParentList >= parentTrans.childCount)
			{
				thisPosInParentList = 0;
			}

			lockCameraToSelection = true;
			lockedOffset = new Vector3();
			GlobalObjects.Instance.SelectedObject = parentTrans.GetChild(thisPosInParentList).gameObject;
			SetSelectedImage();

		}


	}

    //TODO: unselect when clicked outside

    void ClearSelectedImage() 
    {
        SetSatPanel(null,"",null);
    }

    void SetSelectedImage()
    {

		SetSatPanel(GlobalObjects.Instance.SelectedObject.GetComponent<SpriteRenderer>().sprite, 
		            GlobalObjects.Instance.SelectedObject.name, 
		            GlobalObjects.Instance.SelectedObject.GetComponent<Orbit>());



        if (launchButton != null)
        {
            Debug.Log(launchButton.transform.position);
        }
        //if its not a satellite:
		if (!(GlobalObjects.Instance.SelectedObject.tag == "Satellite"))
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

		GameObject img2 = GameObject.Find("SelectionImageOrbiting");
		UnityEngine.UI.Image com2 = img2.GetComponent<UnityEngine.UI.Image>();

        if (sprite)
        {
            com.sprite = sprite;


			if(orbit.transform.parent != null)
			{
				com2.sprite = orbit.transform.parent.GetComponent<SpriteRenderer>().sprite;
			}
			else
			{
				com2.sprite = GlobalObjects.Instance.DefaultSelectionSprite;
			}
        }
        else
        {
            com.sprite = GlobalObjects.Instance.DefaultSelectionSprite;
			com2.sprite = GlobalObjects.Instance.DefaultSelectionSprite;
        }

		com2.transform.Rotate (new Vector3 (0, Time.deltaTime * 100, 0));
        
        
        GameObject original = GameObject.Find("SatPanel");

        //Show/Create the SatPanel thing
        GameObject radSlide = original.transform.Find("Sliders Panel/RadiusSlider").gameObject;
        GameObject speedSlide = original.transform.Find("Sliders Panel/SpeedSlider").gameObject;
        UnityEngine.UI.Text NameText = original.transform.Find("Name Plate/NameText").gameObject.GetComponent<UnityEngine.UI.Text>();

        NameText.text = name;

        radSlide.GetComponent<ChangeSelectedObject>().selectedObjectOrbit = orbit;
        speedSlide.GetComponent<ChangeSelectedObject>().selectedObjectOrbit = orbit;

        GameObject satLaunchText = original.transform.Find("SatLaunch/Text").gameObject;
        

        if (orbit)
        {

            //logic for planets vs satellites
			if (GlobalObjects.Instance.SelectedObject.tag == "Planet" || GlobalObjects.Instance.SelectedObject.tag == "StartingPlanet")
            {
                satLaunchText.GetComponent<UnityEngine.UI.Text>().text = "Launch Satellite";
				setGuiSldierEdgeValues(radSlide, 
				                       GlobalObjects.Instance.SelectedObject.transform.parent.GetComponent<Orbit>().minRadius, 
				                       5 * GlobalObjects.Instance.SelectedObject.transform.parent.GetComponent<Orbit>().baseRadius,
				                       false);
                setGuiSldierEdgeValues(speedSlide, 1, 50, false);
                SetAdditionalInfo(orbit);
            }
			else if (GlobalObjects.Instance.SelectedObject.tag == "Satellite")
            {
                
                satLaunchText.GetComponent<UnityEngine.UI.Text>().text = "Destroy Satellite";

				setGuiSldierEdgeValues(radSlide, 
				                       GlobalObjects.Instance.SelectedObject.transform.parent.GetComponent<Orbit>().minRadius, 
				                       2.5f * GlobalObjects.Instance.SelectedObject.transform.parent.GetComponent<Orbit>().baseRadius,
				                       true);
                setGuiSldierEdgeValues(speedSlide, 5, 200, true);
                ClearAdditionalInfo();
            }
            SetGUISlider(radSlide, orbit.radius);
            SetGUISlider(speedSlide, orbit.rotationSpeed);

        }

        else
        {
            satLaunchText.GetComponent<UnityEngine.UI.Text>().text = "";
            SetGUISlider(speedSlide,0);
            SetGUISlider(radSlide,0);
            ClearAdditionalInfo();
        }
    }

    private void SetAdditionalInfo(Orbit orbit)
    {
        GameObject topPanel = GameObject.Find("ExtraInfoText");
        UnityEngine.UI.Text label = topPanel.GetComponent<UnityEngine.UI.Text>();
        label.text = string.Format("Base$:{0} CostPerRadius:{1} BaseRadius:{2}",orbit.baseCost,orbit.additionalRadiusCost,orbit.baseRadius);
    }

    private void ClearAdditionalInfo() 
    {
        GameObject topPanel = GameObject.Find("ExtraInfoText");
        UnityEngine.UI.Text label = topPanel.GetComponent<UnityEngine.UI.Text>();
        label.text = "";
    }


    void SetGUISlider(GameObject sliderObj, float value)
    {
        UnityEngine.UI.Slider slider = sliderObj.GetComponent<UnityEngine.UI.Slider>();
        Debug.Log(string.Format("Setting slider to val {0}, edge values are {1},{2}", value, slider.minValue, slider.maxValue));
        slider.value = value;

        //different scales for planets vs satellites
    }

    void setGuiSldierEdgeValues(GameObject sliderObj, float min, float max, bool interactable)
    {
        UnityEngine.UI.Slider slider = sliderObj.GetComponent<UnityEngine.UI.Slider>();
        slider.maxValue = max;
        slider.minValue = min;
        slider.interactable = interactable;
    }
}
