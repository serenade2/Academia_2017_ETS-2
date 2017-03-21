using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rewindable : NetworkBehaviour
{
    List<IRewindable> rewindables = new List<IRewindable>(); //Contains every script that implements rewindable

    //Memento
    private LinkedList<IRewindMemento> stateHistory = new LinkedList<IRewindMemento>();
    private RewindMementoFactory rewindMementoFactory = new RewindMementoFactory();
    public RewindMementoFactory.Type memento = 0; //The memento to use with the memento factory;

    // These should be set by the RewindManager so it's uniform
    [HideInInspector]
    public float recordFrequency = 0.1f; //Recording frequency -> higher value = faster replay
    [HideInInspector]
    public float recordMaxTime = 10f;

    //Coroutines
    private Coroutine record;
    private Coroutine rewind;

    private bool isRecording = false;

    // Use this for initialization
    void Start()
    {
        //Start recording
        record = StartCoroutine(Record());
        isRecording = true;

        IRewindable[] rewindableComponents = GetComponentsInChildren<IRewindable>();
        foreach (IRewindable rewindable in rewindableComponents)
        {
            rewindables.Add(rewindable);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ClientRpc]
    public void RpcStartRewind()
    {
        //Stop recording
        StopCoroutine(record);
        isRecording = false;

        //Start rewinding
        rewind = StartCoroutine(Rewind());

        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Rewind(true);
        }
    }

    public void StartRewind()
    {
        //Stop recording
        StopCoroutine(record);
        isRecording = false;

        //Start rewinding
        rewind = StartCoroutine(Rewind());

        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Rewind(true);
        }
    }

    [ClientRpc]
    public void RpcStopRewind()
    {
        //Stop rewinding
        StopCoroutine(rewind);

        //Start recording
        if (!isRecording)
        {
            record = StartCoroutine(Record());
            isRecording = true;
        }

        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Rewind(false);
        }
    }

    public void StopRewind()
    {
        //Stop rewinding
        StopCoroutine(rewind);

        //Start recording
        if (!isRecording)
        {
            record = StartCoroutine(Record());
            isRecording = true;
        }

        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Rewind(false);
        }
    }

    public void RpcStartPause()
    {
        //Stop recording
        StopCoroutine(record);

        //Stop rewinding
        if (rewind != null)
            StopCoroutine(rewind);
        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Pause(true);
        }
    }

    public void StartPause()
    {
        //Stop recording
        StopCoroutine(record);

        //Stop rewinding
        if (rewind != null)
            StopCoroutine(rewind);
        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Pause(true);
        }
    }

    public void StopPause()
    {
        //Start recording
        record = StartCoroutine(Record());
        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.Pause(false);
        }

    }

    IEnumerator Record()
    {
        while (true)
        {
            if (stateHistory.Count < recordMaxTime / recordFrequency)
                stateHistory.AddLast(rewindMementoFactory.MakeRewindMemento(memento, gameObject));
            else
            {
                stateHistory.RemoveFirst();
                stateHistory.AddLast(rewindMementoFactory.MakeRewindMemento(memento, gameObject));
            }
            yield return new WaitForSeconds(recordFrequency);
        }
    }

    IEnumerator Rewind()
    {
        while (true)
        {
            if (stateHistory.Count > 0)
            {
                stateHistory.Last.Value.RestoreFromMemento(gameObject);
                stateHistory.RemoveLast();
            }
            else
            {
                StopRewind();
            }

            yield return null;
        }
    }
}
