using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using System;

public class NavigationScript : MonoBehaviour {


    public GameObject main;
    //public GameObject option;
    public GameObject play;
    public NetworkManager network;

    [Header("Gestion des boutton first Select")]
 

    public Button pageJeuxFristSelect;
    public Button pageMainFristSelect;

    //public void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    public void toSelectPlay() {
        play.gameObject.SetActive(true);
        //option.gameObject.SetActive(false);
        main.gameObject.SetActive(false);

        pageJeuxFristSelect.Select();
        
    }

    //public void toOption() {
    //    play.gameObject.SetActive(false);
    //    //option.gameObject.SetActive(true);
    //    main.gameObject.SetActive(false);
    //}

    public void quiter() {
        Application.Quit();
    }

    public void back()
    {
        play.gameObject.SetActive(false);
        //option.gameObject.SetActive(false);
        main.gameObject.SetActive(true);
        pageMainFristSelect.Select();
    }

    public void prepareGame(Toggle t)
    {
        //modifier le network
        if (t.isOn)
        {
            //set network host
            InputField i = play.GetComponentInChildren<InputField>();
            network.networkPort = int.Parse(i.text);
            network.StartHost();
        }
        else
        {
            // set network Client
            InputField i = play.GetComponentInChildren<InputField>();
            String ipHost = i.text;
            String[] ip = ipHost.Split(':');
            network.networkAddress = ip[0];
            network.networkPort = int.Parse(ip[1]);
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
