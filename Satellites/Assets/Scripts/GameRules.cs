using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameRules : MonoBehaviour {
    


	public SatConnection sat1;
	public SatConnection sat2;

	public float timeRequired;

	private float startTime;

	public float counter = 0;

	public bool isStarted = false;

	public bool hasWon = false;

	public int totalAllowedFuel = 500;

	public float currentFuel = 0;

	public enum RuleType
	{
		percentageOverTime,
		continuousTime,
        endlessPercentage
	}

	public RuleType rule = RuleType.continuousTime;

	public float percentageRequired = .7f;

	public float currPercentage;

    public UnityEngine.UI.Text fuelLabel;
    public UnityEngine.UI.Text uplinkLabel;
    public GameObject VictoryScreen;



    //used for endlessPercentage mode
    //frameTime is the time to draw the frame
    //linked is whether the main sats are linked.
    /*
     *Every frame is inserted into the stack and its weight (frameTime) is added, and a weighted avg of the stack is calculated
     * frameTime*linked(1/0)
     * when the sum of frameTime in the stack is over the timeRequired and the avg is above percentageRequired hasWon is set to true.
     * when a new frame enters and the sum of frames in stack is greater than timeRequired, the stack is poped and the avg is recalculated with the new frame.
     */
    private struct UplinkFrame
    {
        public float frameTime;
        public bool linked;
        public UplinkFrame(bool link)
        {
            frameTime = Time.deltaTime;
            linked = link;
        }
    }
    private Queue<UplinkFrame> frameQueue = new Queue<UplinkFrame>();
    public float frameTimeSum = 0;
    public float frameTimeWeightedSum = 0;
    public bool connected = false;


	// Use this for initialization
	void Start () {

        if (rule == RuleType.endlessPercentage ) {
			StartRules();
		}
	}
	
	// Update is called once per frame
	void Update () {


		GameObject[] sats = GameObject.FindGameObjectsWithTag ("Satellite"); //main sats are now free. is that good?
		
		currentFuel = 0;
		
		foreach (GameObject sat in sats) {
			currentFuel += ((FuelCost) sat.GetComponent("FuelCost")).GetFuelCost();
		}


		if(isStarted)
			switch (rule) {
			case RuleType.continuousTime:

                //StartTime is logged, if timeRequired has passed and there was no link disruption, winner.
				if(sat1.connectedSats.Contains(sat2.myID))
				{
					counter = Time.time;
				}
				else
				{
					End();
				}

				if(counter >= startTime + timeRequired)
				{
					End();
					hasWon = true;
					Debug.Log("winner");
				}
                
				break;

			case RuleType.percentageOverTime:

                
				if(sat1.connectedSats.Contains(sat2.myID))
				{
					counter += Time.deltaTime;
				}
            
			    currPercentage = counter / (Time.time - startTime);

				if(Time.time >= startTime + timeRequired)
				{
					End();
					if(currPercentage >= percentageRequired)
					{
						hasWon = true;
						Debug.Log("winner");
                        VictoryScreen.SetActive(true);
                        GlobalObjects.Instance.speedSlider.value = 0;
					}
				}

                uplinkLabel.text = "Uplink:\n" + currPercentage.ToString("F2") + "/" + percentageRequired;
				break;

                case RuleType.endlessPercentage:
                    UplinkFrame f = new UplinkFrame(sat1.connectedSats.Contains(sat2.myID));
                    connected = f.linked;
                    if (frameTimeSum >= timeRequired)
                    {
                        
                        if (currPercentage >= percentageRequired)
                        {
                            End();
                            hasWon = true;
                            Debug.Log("winner");
                            VictoryScreen.SetActive(true);
                            GlobalObjects.Instance.speedSlider.value = 0;
                        }

                        UplinkFrame popped = frameQueue.Dequeue();
                        frameTimeSum -= popped.frameTime;
                        frameTimeWeightedSum -= popped.linked ? popped.frameTime : 0;
                    }
                    
                    frameQueue.Enqueue(f);
                    frameTimeSum += f.frameTime;
                    frameTimeWeightedSum += f.linked? f.frameTime : 0;
                    currPercentage = frameTimeWeightedSum / frameTimeSum;

                    uplinkLabel.text = "Last "+timeRequired+"s Uplink:\n" + currPercentage.ToString("P") + "/" + percentageRequired.ToString("P");
                    break;

			}

		if (Input.GetKeyDown (KeyCode.Space)  ) {
			StartRules();
		}


        fuelLabel.text = "Fuel:\n" + currentFuel.ToString("F2") + "/" + totalAllowedFuel.ToString();
        if (currentFuel > totalAllowedFuel)
        {
            fuelLabel.color = Color.red;
        }
        else
        {
            fuelLabel.color = Color.white;
        }
	}

	public void StartRules()
	{
		End ();

		if (currentFuel <= totalAllowedFuel) {

			isStarted = true;
			startTime = Time.time;
			counter = 0;

		}
	}

	public void End()
	{
		hasWon = false;
		isStarted = false;
	}
}
