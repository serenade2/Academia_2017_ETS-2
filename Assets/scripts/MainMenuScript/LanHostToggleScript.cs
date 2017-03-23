using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Net;
using System.Net.Sockets;

public class LanHostToggleScript : MonoBehaviour
{

    
    public Text IpText;
    public InputField clientField;
    private bool isSelected;

    // Use this for initialization
    void Start()
    {
        isSelected = false;
        Debug.Log(clientField);
        IpText.enabled = false;
        //clientField.enabled = true;
        clientField.interactable = true;
    }

    public void isToggleActif() {

        if (!isSelected)
        {
            isSelected = true;
            // rendre le joueur host

            Debug.Log(clientField.isActiveAndEnabled);
            IpText.text = "IP Adresse : " + LocalIPAddress();
            IpText.enabled = true;
            clientField.interactable = false;
            //clientField.enabled = false;
            
        }
        else {
            isSelected = false;
            // rendre le joueur Client

            //clientField.enabled = true;
            clientField.interactable = true;
            IpText.text = "";
        }
    }

    

    /// <summary>
    /// affiche l'adress ip de la machine sur le reseau
    /// 
    /// </summary>
    /// *** Source *** http://answers.unity3d.com/questions/1004969/local-ip-adress-in-unet.html
    /// <returns></returns>
    private string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }



}