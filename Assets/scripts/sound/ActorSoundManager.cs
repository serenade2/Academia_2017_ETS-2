using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActorSoundManager : MonoBehaviour
{
    public List<AudioClip> StepAudioClips;
    public AudioClip InteractionAudioClip;
    public AudioClip DesintegrateAudioClip;
    private AudioSource audioPlayer;
    private NetworkAnimator networkAnimator;
    private int maxClip;
    private int nextClip;
	// Use this for initialization
	void Start ()
    {
        networkAnimator = GetComponent<NetworkAnimator>();
	    audioPlayer = GetComponent<AudioSource>();
	    maxClip = StepAudioClips.Count;
	    nextClip = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlayStepSound()
    {
        if (audioPlayer.isPlaying)
        {
            
        }
        if (nextClip == StepAudioClips.Count)
        {
            nextClip = 0;
        }
        AudioClip currentStep = StepAudioClips[nextClip];
        audioPlayer.clip = currentStep;
        audioPlayer.Play();
        nextClip++;
    }

    public void PlayDesintegrateSound()
    {
        audioPlayer.clip = DesintegrateAudioClip;
        audioPlayer.Play();
    }


}
