using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : NetworkBehaviour
{
    public AudioSource WinningSound;
    public AudioSource PlaySound;
    public AudioSource RewindSound;
    public AudioSource LosingSound;


    // Use this for initialization
    public void Start()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.A))
	    {
	        ToggleReverse();
	    }
    }

    public void GameStartPlay()
    {
        PlayStageClip(true);
        PlayRevertStageClip(true);
        MuteRevertStageClip();
    }

    public void PlayWinningClip(bool looping)
    {
        WinningSound.Play();
        StopStageClip();
        StopRevertStageClip();
    }

    public void PlayLosingClip(bool looping)
    {
        LosingSound.Play();
        StopStageClip();
        StopRevertStageClip();
    }

    public void PlayStageClip(bool looping)
    {
        PlaySound.Play();
        PlaySound.loop = looping;
    }

    public void PlayRevertStageClip(bool looping)
    {
        RewindSound.Play();
        RewindSound.loop = looping;
    }

    public void StopWinningClip()
    {
        WinningSound.Stop();
    }

    public void StopStageClip()
    {
        PlaySound.Stop();
    }

    public void StopRevertStageClip()
    {
        RewindSound.Stop();
    }
    public void StopLosingClip()
    {
        LosingSound.Stop();
    }

    public void MuteStageClip()
    {
        PlaySound.mute = true;
    }

    [ClientRpc]
    public void RpcMuteStageClip()
    {
        MuteRevertStageClip();
    }

    public void MuteRevertStageClip()
    {
        RewindSound.mute = true;
    }

    [ClientRpc]
    public void RpcMuteRevertStageClip()
    {
        MuteRevertStageClip();
    }

    public void UnMuteStageClip()
    {
        PlaySound.mute = false;
    }

    [ClientRpc]
    public void RpcUnMuteStageClip()
    {
        RpcUnMuteStageClip();
    }

    public void UnMuteRevertStageClip()
    {
        RewindSound.mute = false;
    }

    [ClientRpc]
    public void RpcUnMuteRevertStageClip()
    {
        UnMuteRevertStageClip();
    }

    public void ToggleReverse()
    {
        if (PlaySound.pitch < 0)
        {
            PlaySound.pitch = 1;
        }
        else
        {

            PlaySound.pitch = -1.6f;
        }
    }
}
