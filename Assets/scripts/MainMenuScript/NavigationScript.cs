﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class NavigationScript : MonoBehaviour {

    public GameObject main;
    public GameObject option;
    public GameObject play;


    public void prepareGame(Toggle t, InputField i) {
        //modifier le network
        if (t.isOn)
        {
            //set network host

        }
        else {
            // set network Client

            String ipHost = i.text;
        }

    }


    public void optionPanelAccept(Slider s) {
        //modifier son
        int volume = Mathf.RoundToInt(s.value);

       


    }

    public void back() {
        play.SetActive(false);
        option.SetActive(false);
        main.SetActive(true);
    }


}