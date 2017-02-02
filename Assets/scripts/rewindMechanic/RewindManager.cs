using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour {
    List<Rewindable> rewinds = new List<Rewindable>();

	// Use this for initialization
	void Awake () {
        Rewindable[] rewindComponents = FindObjectsOfType(typeof(Rewindable)) as Rewindable[];
        foreach (Rewindable rewind in rewindComponents)
        {
            rewinds.Add(rewind);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick2Button4))
        {
            foreach(Rewindable rewind in rewinds)
                rewind.StartRewind();

        }
        else if (Input.GetKeyUp(KeyCode.Joystick2Button4))
        {
            foreach (Rewindable rewind in rewinds)
                rewind.StopRewind();
        }

        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
        {
            foreach (Rewindable rewind in rewinds)
                rewind.StartPause();

        }
        else if (Input.GetKeyUp(KeyCode.Joystick2Button1))
        {
            foreach (Rewindable rewind in rewinds)
                rewind.StopPause();
        }
	}
}
