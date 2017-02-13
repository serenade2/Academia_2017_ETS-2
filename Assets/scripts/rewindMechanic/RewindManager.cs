using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class that manage the inputs to rewind and pause. Also calls the respective functions in every gameobjects(client) that have the "Rewindable" script attached to it.
/// </summary>
public class RewindManager : NetworkBehaviour
{
    private List<Rewindable> rewinds = new List<Rewindable>();
    public GameObject blackGlitch;

    // Use this for initialization
    public override void OnStartServer()
    {
        //Finds and adds every gameobjects that has the "Rewindable" script attached to it.
        Rewindable[] rewindComponents = FindObjectsOfType(typeof(Rewindable)) as Rewindable[];
        foreach (Rewindable rewind in rewindComponents)
        {
            rewinds.Add(rewind);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            foreach (Rewindable rewind in rewinds)
            {
                if (rewind.hasAuthority)
                {
                    rewind.StartRewind();
                }
                else
                {
                    rewind.RpcStartRewind(rewind.netId);
                }
            }


        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        { 
            foreach (Rewindable rewind in rewinds)
            {
                if (rewind.hasAuthority)
                {
                    rewind.StopRewind();
                }
                else
                {
                    rewind.RpcStopRewind(rewind.netId);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("MolinetteNetworking");
            blackGlitch.SetActive(true);
            Invoke("RemoveBlackGlitch",1f);
            foreach (Rewindable rewind in rewinds)
                rewind.StartPause();

        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            CancelInvoke();
            RemoveBlackGlitch();
            foreach (Rewindable rewind in rewinds)
                rewind.StopPause();
        }
    }

    public void AddRewindable(Rewindable rewindable)
    {
        rewinds.Add(rewindable);
    }

    public void RemoveBlackGlitch()
    {
        blackGlitch.SetActive(false);
    }
}

