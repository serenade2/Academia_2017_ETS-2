using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RewindManager : NetworkBehaviour
{
    public List<Rewindable> rewinds = new List<Rewindable>();

    // Use this for initialization
    public override void OnStartServer()
    {
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
                //rewind.StartRewind();
                if (rewind.isClient)
                {
                    rewind.RpcStartRewind();
                }
                else
                {
                    rewind.StartRewind();
                }
            }


        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            foreach (Rewindable rewind in rewinds)
            {
                //rewind.StartRewind();
                if (rewind.isClient)
                {
                    rewind.RpcStopRewind();
                }
                else
                {
                    rewind.StopRewind();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            foreach (Rewindable rewind in rewinds)
                rewind.StartPause();

        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            foreach (Rewindable rewind in rewinds)
                rewind.StopPause();
        }
    }

    public void AddRewindable(Rewindable rewindable)
    {
        rewinds.Add(rewindable);
    }
}

