using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManageur : MonoBehaviour
{
    public Image pauseImg;
    public Image rewindImg;

    private bool pauseNotEmpty;
    private bool rewindNotEmpty;

    private IStratInteraction pauseStrat;
    private IStratInteraction rewindStrat;


	// Use this for initialization
	void Start () {
		pauseStrat = new StratPause();
		rewindStrat = new StratRewind();
	    rewindNotEmpty = true;
	    pauseNotEmpty = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    usePower();

	}

    void FixedUpdate()
    {

    }

    private void usePower()
    {
        if (false) // "key rewind down"
        {
            activeRewind();
        }
        else if (false) // "key pause down"
        {
            activePause();
        }
    }

    private void activeRewind()
    {
        if (rewindNotEmpty)
        {
            
        }
    }




}
