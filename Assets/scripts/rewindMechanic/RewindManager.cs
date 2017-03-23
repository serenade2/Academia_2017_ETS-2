using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using CustomUtile;
using UnityEngine;
using UnityEngine.Networking;

public class RewindManager : NetworkBehaviour, Observable
{
    public List<Rewindable> rewinds = new List<Rewindable>();
    public GameObject blackGlitch;

    public RewindParticle ambientParticle;

    [Tooltip("Frequency of the recording. The higher the value, the faster the replay.")]
    public float recordFrequency = 0.1f;

    [Tooltip("Max time in seconds the game objects should record themselves")]
    public float recordMaxTime = 10f;

    private float rewindRealLifeTime;

    private int recordCount = 0;
    private Coroutine record;
    private Coroutine rewind;
    private bool isRecording = false;

    private ArrayList listObserver = new ArrayList();
    private bool triggerableUpdate = false;

    private bool powerIsReady = true;
    private float currentTime;
    public float cooldownTime = 15f;
    public float yieldWaitingTime = 0.1f;
    public float progressCooldown = 0.1f;


    // Use this for initialization
    public override void OnStartServer()
    {
        // not sure if this is good or not... it's actually smelly, right ?
        rewindRealLifeTime = recordMaxTime*recordFrequency;

        Rewindable[] rewindComponents = FindObjectsOfType(typeof(Rewindable)) as Rewindable[];
        foreach (Rewindable rewind in rewindComponents)
        {
            rewind.recordFrequency = recordFrequency;
            rewind.recordMaxTime = recordMaxTime;
            rewinds.Add(rewind);
        }

        ambientParticle = FindObjectOfType<RewindParticle>();

        record = StartCoroutine(Record());
    }

    // Update is called once per frame
    void Update()
    {
        if (!powerIsReady)
            return;

        // rewind button down
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            StartRewind();
        }

        // rewind button up
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            StopRewind();
            activeCooldown();
        }

        // pause button down
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            blackGlitch.SetActive(true);
            Invoke("RemoveBlackGlitch",1f);
            foreach (Rewindable rewind in rewinds)
                rewind.StartPause();

            ambientParticle.Pause();
        }

        // pause button up
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            CancelInvoke();
            RemoveBlackGlitch();
            foreach (Rewindable rewind in rewinds)
                rewind.StopPause();

            ambientParticle.UnPause();
            activeCooldown();
        }
    }

    public void AddRewindable(Rewindable rewindable)
    {
        rewinds.Add(rewindable);
    }

    public void RemoveRewindable(Rewindable rewindable)
    {
        rewinds.Remove(rewindable);
    }

    public void RemoveBlackGlitch()
    {
        blackGlitch.SetActive(false);
    }

    /*IEnumerator StopRewindAfterMaxTime()
    {
        yield return new WaitForSeconds(rewindRealLifeTime);
        StopRewind();
    }*/

    IEnumerator Record()
    {
        while (true)
        {
            if (recordCount < recordMaxTime / recordFrequency)
                recordCount++;
            yield return new WaitForSeconds(recordFrequency);
        }
    }

    IEnumerator Rewind()
    {
        while (true)
        {
            if (recordCount > 0)
            {
                recordCount--;
            }
            else
            {
                StopRewind();
            }

            yield return null;
        }
    }

    public void StartRewind()
    {
        StopCoroutine(record);
        isRecording = false;
        rewind = StartCoroutine(Rewind());
        foreach (Rewindable rewind in rewinds)
        {
            if (rewind.gameObject.CompareTag("Hacker"))
            {
                rewind.RpcStartRewind();
            }
            else
            {
                rewind.StartRewind();
            }
        }

        ambientParticle.StartRewind();

    }

    public void StopRewind()
    {
        StopCoroutine(rewind);
        if (!isRecording)
        {
            record = StartCoroutine(Record());
            isRecording = true;
        }

        foreach (Rewindable rewind in rewinds)
        {
            if (rewind.gameObject.CompareTag("Hacker"))
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


    private void activeCooldown()
    {
        powerIsReady = false;
        currentTime = 0;
        StartCoroutine(CooldownCoroutine());
    }


    private IEnumerator CooldownCoroutine()
    {
        for (float i = 0; i <= cooldownTime; i += progressCooldown)
        {
            currentTime = i;
            setChanged();
            notify();
            yield return new WaitForSeconds(yieldWaitingTime);
        }

        powerIsReady = true;
    }

    public float CooldownTime
    {
        get { return cooldownTime; }
    }

    public float CurrentTime
    {
        get { return currentTime; }
    }


    public void addObserver(Observer o)
    {
        listObserver.Add(o);
    }

    public void setChanged()
    {
        triggerableUpdate = true;
    }

    public void notify()
    {
        if (triggerableUpdate)
        {
            foreach (Observer o in listObserver)
            {
                o.updateObserver();
            }
            triggerableUpdate = false;
        }
    }
}

