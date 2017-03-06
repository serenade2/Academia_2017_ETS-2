using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class NavigationScript : MonoBehaviour {


    public GameObject main;
    public GameObject option;
    public GameObject play;
    public NetworkManager network;

    //public void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    public void toSelectPlay() {
        play.SetActive(true);
        option.SetActive(false);
        main.SetActive(false);
    }

    public void toOption() {
        play.SetActive(false);
        option.SetActive(true);
        main.SetActive(false);
    }

    public void quiter() {
        Application.Quit();
    }

    public void back()
    {
        play.SetActive(false);
        option.SetActive(false);
        main.SetActive(true);
    }

    public void prepareGame(Toggle t)
    {
        //modifier le network
        if (t.isOn)
        {
            //set network host

            network.StartHost();
        }
        else
        {
            // set network Client
            
            InputField i = play.GetComponentInChildren<InputField>();
            String ipHost = i.text;
            Debug.Log(ipHost);
            network.SetMatchHost(ipHost, 7777,false);
            network.StartClient();
            
        }
        play.SetActive(false);
    }


    public void optionPanelAccept(Slider s) {
        //modifier son
        int volume = Mathf.RoundToInt(s.value);

        back();
    }



}
