using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CustomUtile;

public class RewindManager : NetworkBehaviour, Observable
{
    private bool trigerableUpdate = false;

    private bool powerIsReady = true;
    public float cooldownTime;
    private float currentTime;
    public float yieldWaitingTime;
    public float progressCooldown;
    private ArrayList listObserver = new ArrayList();

    public List<Rewindable> rewinds = new List<Rewindable>();
    public GameObject blackGlitch;

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

    private void Pause(bool release)
    {
        if (release)
        {
            CancelInvoke();
            RemoveBlackGlitch();
            foreach (Rewindable rewind in rewinds)
                rewind.StopPause();
        }
        else
        {
            blackGlitch.SetActive(true);
            Invoke("RemoveBlackGlitch", 1f);
            foreach (Rewindable rewind in rewinds)
                rewind.StartPause();
        }
    }

    private void InputChecker()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            Rewinder(false);
            
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            Rewinder(true);
            activeCooldown();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Pause(false);

        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            Pause(true);
            activeCooldown();
        }
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
        for (float i = 0; i <= cooldownTime; i+= progressCooldown)
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
                o.update();
            }
            trigerableUpdate = false;
        }
        
    }
}

