using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RewindManager : NetworkBehaviour
{
    public List<Rewindable> rewinds = new List<Rewindable>();
    public GameObject blackGlitch;

    public RewindParticle ambientParticle;

    [Tooltip("Frequency of the recording. The higher the value, the faster the replay.")]
    public float recordFrequency = 0.1f;

    [Tooltip("Max time in seconds the game objects should record themselves")]
    public float recordMaxTime = 10f;

    [Tooltip("How much time in real life the rewind effect lasts. ONLY USED TO STOP REWIND EFFECTS EVEN IF THE PLAYER HOLDS THE BUTTON")]
    public float rewindRealLifeTime = 5f;

    // Use this for initialization
    public override void OnStartServer()
    {
        Rewindable[] rewindComponents = FindObjectsOfType(typeof(Rewindable)) as Rewindable[];
        foreach (Rewindable rewind in rewindComponents)
        {
            rewind.recordFrequency = recordFrequency;
            rewind.recordMaxTime = recordMaxTime;
            rewinds.Add(rewind);
        }

        ambientParticle = FindObjectOfType<RewindParticle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            StartRewind();
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            StopRewind();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
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

    IEnumerator StopRewindAfterMaxTime()
    {
        yield return new WaitForSeconds(rewindRealLifeTime);
        StopRewind();
    }

    public void StartRewind()
    {
        foreach (Rewindable rewind in rewinds)
        {
            if (rewind.isClient)
            {
                rewind.RpcStartRewind();
            }
            else
            {
                rewind.StartRewind();
            }
        }

        ambientParticle.StartRewind();

        // make sure the rewinding stops after the allowed time
        StartCoroutine(StopRewindAfterMaxTime());
    }

    public void StopRewind()
    {
        StopCoroutine(StopRewindAfterMaxTime());

        foreach (Rewindable rewind in rewinds)
        {
            if (rewind.isClient)
            {
                rewind.RpcStopRewind();
            }
            else
            {
                rewind.StopRewind();
            }
        }

        ambientParticle.StopRewind();
    }

}

