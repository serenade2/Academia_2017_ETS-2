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
    private SoundManager soundManager;
	private int recordCount = 0;
    private Coroutine record;
    private Coroutine rewind;
    private Coroutine gCD;//Global CoolDown
    private Coroutine rewindCD;
    private Coroutine pauseCD;
    private bool isRecording = false;
    private ArrayList listObserver = new ArrayList();
    private bool triggerableUpdate = false;

    private bool powerIsReady = true;
    private float currentTime;
    public float cooldownTime = 15f;
    public float yieldWaitingTime = 0.1f;
    public float progressCooldown = 0.1f;

    [Header("Cooldown elements")]
    [Tooltip("Cooldown separer")]
    public bool cooldownSplit = false;

    private bool pauseCooldown = false;
    private bool rewindCooldown = false;

    private float pauseCooldownTime;
    private float rewindCooldownTime;

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
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        record = StartCoroutine(Record());
    }

    // Update is called once per frame
    void Update()
    {
        if(!cooldownSplit && !powerIsReady)
            return;

        // rewind button down
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            if (cooldownSplit)
                rewindCooldown = true;

            StartRewind();

            soundManager.MuteStageClip();
            soundManager.UnMuteRevertStageClip();
        }

        // rewind button up
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            StopRewind();

            soundManager.MuteRevertStageClip();
            soundManager.UnMuteStageClip();
            activeCooldown();
        }

        // pause button down
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (cooldownSplit)
                pauseCooldown = true;

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
        rewindCooldownTime = 0;
        pauseCooldownTime = 0;

        if (!cooldownSplit)
        {
            gCD = StartCoroutine(CooldownCoroutine(0));
        }
        else
        {
            if (pauseCooldown)
               rewindCD = StartCoroutine(CooldownCoroutine(2));
            if (rewindCooldown)
               pauseCD = StartCoroutine(CooldownCoroutine(1));
        }
    }


    private IEnumerator CooldownCoroutine(int j)
    {
        for (float i = 0; i <= cooldownTime; i += progressCooldown)
        {
            switch (j)
            {
                case 1:
                    pauseCooldownTime = i;
                    break;
                case 2:
                    rewindCooldownTime = i;
                    break;
                default:
                    currentTime = i;
                    break;
            }   
            setChanged();
            notify();
            yield return new WaitForSeconds(yieldWaitingTime);
        }

        switch (j)
        {
            case 1:
                pauseCooldown = false;
                break;
            case 2:
                rewindCooldown = false;
                break;
            default:
                powerIsReady = true;
                break;
        }

        powerIsReady = true;
    }

    public bool CooldownSplit
    {
        get { return cooldownSplit; }
    }

    public bool PauseCooldown
    {
        get { return pauseCooldown; }
    }

    public bool RewindCooldown
    {
        get { return rewindCooldown; }
    }

    public float PauseCooldownTime
    {
        get { return pauseCooldownTime; }
    }

    public float RewindCooldownTime
    {
        get { return rewindCooldownTime; }
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

