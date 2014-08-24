using UnityEngine;
using System.Collections;

public class GameRules : MonoBehaviour {

	public SatConnection sat1;
	public SatConnection sat2;

	public float timeRequired;

	private float startTime;

	public float counter = 0;

	public bool isStarted = false;

	public bool hasWon = false;

	public enum RuleType
	{
		percentageOverTime,
		continuousTime
	}

	public RuleType rule = RuleType.continuousTime;

	public float percentageRequired = .7f;

	public float currPercentage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isStarted)
			switch (rule) {
			case RuleType.continuousTime:

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
					}
				}


				break;
			}

		if (Input.GetKeyDown (KeyCode.Space)) {
			StartRules();
		}
	}

	public void StartRules()
	{
		End ();
		isStarted = true;
		startTime = Time.time;
		counter = 0;
	}

	public void End()
	{
		hasWon = false;
		isStarted = false;
	}
}
