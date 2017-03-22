using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CustomUtile;

public class RewindManager : NetworkBehaviour, Observable
{
    private bool trigerableUpdate = false;

    private bool powerIsReady = true;
    public float cooldownTime =15f;
    private float currentTime;

    [Header("Temp entre les tics")]
    public float yieldWaitingTime = 0.1f;
    public float progressCooldown = 0.1f;
    [Header("Temp entre les tics")]
    private ArrayList listObserver = new ArrayList();

    public List<Rewindable> rewinds = new List<Rewindable>();
    public GameObject blackGlitch;

    private float rewindRealLifeTime;

    public RewindParticle ambientParticle;

    [Tooltip("Frequency of the recording. The higher the value, the faster the replay.")]
    public float recordFrequency = 0.1f;

    [Tooltip("Max time in seconds the game objects should record themselves")]
    public float recordMaxTime = 10f;


    // Use this for initialization
    public override void OnStartServer()
    {
        // not sure if this is good or not... it's actually smelly, right ?
        rewindRealLifeTime = recordMaxTime * recordFrequency;

        Rewindable[] rewindComponents = FindObjectsOfType(typeof(Rewindable)) as Rewindable[];
        foreach (Rewindable rewind in rewindComponents)
        {
            rewinds.Add(rewind);
        }

        ambientParticle = FindObjectOfType<RewindParticle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (powerIsReady)
        {
            InputChecker();
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

    private void Rewinder(bool release)
    {
        if (release)
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
        else
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
    }


    private void InputChecker()
    {

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
            Invoke("RemoveBlackGlitch", 1f);
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


    private void activeCooldown()
    {
        powerIsReady = false;
        currentTime = 0;
        StartCoroutine(CooldownCoroutine());
        finishCoolDonw();
    }

    private void finishCoolDonw()
    {
        StopCoroutine(CooldownCoroutine());
        powerIsReady = true;
    }

    private IEnumerator CooldownCoroutine()
    {
        for (float i = 0; i <= cooldownTime; i += progressCooldown)
        {
            currentTime = i;
        }
        setChanged();
        notify();
        yield return new WaitForSeconds(yieldWaitingTime);
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
        trigerableUpdate = true;
    }

    public void notify()
    {
        if (trigerableUpdate)
        {
            foreach (Observer o in listObserver)
            {
                o.updateObserver();
            }
            trigerableUpdate = false;
        }

    }

    IEnumerator StopRewindAfterMaxTime()
    {
        yield return new WaitForSeconds(rewindRealLifeTime);
        StopRewind();
        activeCooldown();
    }
}

