using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayUISound : MonoBehaviour {
	
	public AudioSource player;
	
	public List<AudioClip> clickClips;
	public AudioClip currentClickClip;
	public int previousClickClip = 100000;

	public List<AudioClip> rollOverClips;
	public AudioClip currentrollOverClip;
	public int previousrollOverClip = 100000;

	public List<AudioClip> switchClips;
	public AudioClip currentSwitchClip;
	public int previousSwitchClip = 100000;

	public void Start()
	{
	}

	public void PlayRandomClickClip()
	{
		PlayRandomSound (ref clickClips, ref currentClickClip, ref previousClickClip);
	}

	public void PlayRandomRolloverClip()
	{
		PlayRandomSound (ref rollOverClips, ref currentrollOverClip, ref previousrollOverClip);
	}

	public void PlayRandomSwitchClip()
	{
		PlayRandomSound (ref switchClips, ref currentSwitchClip, ref previousSwitchClip);
	}

	public void PlayRandomSound(ref List<AudioClip> soundSources, ref AudioClip current, ref int previousClip)
	{
		if(!player.isPlaying) {
			//Do not allow for duplicate songs to be played
			
			int clipToUse = Random.Range(0, soundSources.Count);
			while(previousClip == clipToUse)
			{
				clipToUse = Random.Range(0, soundSources.Count);
			}
			previousClip = clipToUse;
			current = soundSources[clipToUse];
			player.clip = current;
			player.Play();
		}
	}

}
