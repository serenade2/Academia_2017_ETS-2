using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorSoundManager : MonoBehaviour
{
    public List<AudioClip> StepAudioClips;
    public AudioClip InteractionAudioClip;
    public AudioClip DesintegrateAudioClip;

    private NetworkAnimator networkAnimator;
	// Use this for initialization
	void Start ()
    {

         networkAnimator = GetComponent<NetworkAnimator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
