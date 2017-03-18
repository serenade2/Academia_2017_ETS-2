﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CrosshairManager : MonoBehaviour {

    public RawImage crossHair;
    private Camera worldCamera;
    private Canvas uiGeneralCanvas;

    RaycastHit hit;
    public LayerMask layer;
    TagCharacter characterTag;
    private List<TagCharacter> tags = new List<TagCharacter>();
    public Color color;


    float maxWidth;
    float maxHeight;

    private string horizontal = "RHorizontal1";
    private string vertical = "RVertical1";
    public int multiplicator;
    public bool debugModeIsActive;

    Vector2 input;

    bool tuchWallNorth;
    bool tuchWallSouth;
    bool tuchWallEast;
    bool tuchWallWest;


    RaycastHit test;
    float dist;

    // Use this for initialization
    void Start()
    {
        uiGeneralCanvas = GetComponentInParent<Canvas>();
       


        maxWidth = uiGeneralCanvas.pixelRect.width;
        maxHeight = uiGeneralCanvas.pixelRect.height;

        crossHair.rectTransform.position = new Vector3(maxWidth / 2f, maxHeight / 2f, 0);
        
        // TODO change main camera
        worldCamera = Camera.main;

        uiGeneralCanvas.worldCamera = worldCamera;

        tuchWallNorth = false;
        tuchWallSouth = false;
        tuchWallEast = false;
        tuchWallWest = false;

    }


    // Update is called once per frame
    void Update()
    {
        float newX = Mathf.Clamp(crossHair.rectTransform.position.x + (Input.GetAxis(horizontal) * multiplicator), 0f, maxWidth);
        float newY = Mathf.Clamp(crossHair.rectTransform.position.y + (Input.GetAxis(vertical) * multiplicator), 0f, maxHeight);
        Vector2 newPosition = new Vector2(newX, newY);

        crossHair.rectTransform.position = newPosition;

        tagEvent();
        debugMode();
    }

    private void tagEvent()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            debugLog("1");
            Ray ray = worldCamera.ScreenPointToRay(crossHair.transform.position);
            debugLog(ray.ToString());
            debugLog("physique ray cast : " + Physics.Raycast(ray.origin, ray.direction, out hit, 50f, layer).ToString());
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 50f, layer))
            {
                debugLog(hit.ToString());
                if (hit.collider.transform.parent.GetComponent<TagCharacter>() != null)
                {
                    debugLog("hit collider");
                    characterTag = hit.collider.transform.parent.GetComponent<TagCharacter>();
                    if (characterTag.GetIsTagged())
                    {
                        tags.Remove(characterTag);
                        characterTag.UnTag();
                        debugLog("tag remove");
                    }
                    else
                    {
                        debugLog("tag set");
                        if (tags.Count <= 2)
                        {
                            tags.Add(characterTag);
                            characterTag.Tag(color);
                            Debug.Log("Tagged");
                        }
                    }
                }
            }
        }
    }



    /// <summary>
    /// debug mode
    /// </summary>
    private void debugMode() {
        if (debugModeIsActive) {
            Debug.DrawLine(crossHair.rectTransform.position, new Vector3(0, 0, 0));

            //Debug.Log("tuchWallWest : " + tuchWallWest);
            //Debug.Log("tuchWallEast : " + tuchWallEast);
            //Debug.Log("tuchWallSouth : " + tuchWallSouth);
            //Debug.Log("tuchWallNouth : " + tuchWallNorth);
           

            // test raycast

            Vector3 forward = crossHair.transform.TransformDirection(Vector3.forward) * 100;

            Debug.DrawRay(crossHair.transform.position, forward, Color.green);

            //Debug.Log(worldCamera);
            Ray ray = worldCamera.ScreenPointToRay(crossHair.transform.position);

            

            Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red);
        } 
    }


    /// <summary>
    /// debug mode
    /// </summary>
    private void debugLog(string s)
    {
        if (debugModeIsActive)
        {
            Debug.Log(s);
        }
    }



}


