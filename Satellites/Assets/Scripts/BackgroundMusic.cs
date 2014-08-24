using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	public AudioSource player;

	public AudioClip[] musicClips;
	public AudioClip currentClip;
	private int previousClip = 100000;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.isPlaying) {
			//Do not allow for duplicate songs to be played

			int clipToUse = Random.Range(0, musicClips.Length);
			while(previousClip == clipToUse)
			{
				clipToUse = Random.Range(0, musicClips.Length);
			}
			previousClip = clipToUse;
			currentClip = musicClips[clipToUse];
			player.clip = currentClip;
            player.Play();
		}
	}
}
