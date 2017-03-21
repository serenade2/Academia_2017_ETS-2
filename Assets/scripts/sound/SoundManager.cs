using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource WinningAudio;
    public AudioSource StageAudio;
    public AudioSource RevertedAudio;
    public AudioSource LosingAudio;


    // Use this for initialization
    void Start ()
    {
        //InitializeComponents();
        PlayStageClip(true);
        PlayRevertStageClip(true);
        MuteRevertStageClip();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlayWinningClip(bool looping)
    {
        WinningAudio.Play();
    }

    public void PlayLosingClip(bool looping)
    {
        LosingAudio.Play();
    }

    public void PlayStageClip(bool looping)
    {
        StageAudio.Play();
        StageAudio.loop = looping;
    }

    public void PlayRevertStageClip(bool looping)
    {
        RevertedAudio.Play();
        RevertedAudio.loop = looping;
    }

    public void StopWinningClip()
    {
        WinningAudio.Stop();
    }

    public void StopStageClip()
    {
        StageAudio.Stop();
    }

    public void StopRevertStageClip()
    {
        RevertedAudio.Stop();
    }
    public void StopLosingClip()
    {
        LosingAudio.Stop();
    }

    public void MuteStageClip()
    {
        StageAudio.mute = true;
    }

    public void MuteRevertStageClip()
    {
        RevertedAudio.mute = true;
    }

    public void UnMuteStageClip()
    {
        StageAudio.mute = false;
    }

    public void UnMuteRevertStageClip()
    {
        RevertedAudio.mute = false;
    }
}
