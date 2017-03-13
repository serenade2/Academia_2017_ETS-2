using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour {

    public RawImage crossHair;

    float maxWidth;
    float maxHeight;

    public string horizontal;
    public string vertical;
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
        maxWidth = GetComponentInParent<Canvas>().pixelRect.width;
        maxHeight = GetComponentInParent<Canvas>().pixelRect.height;

        crossHair.rectTransform.position = new Vector3(maxWidth / 2f, maxHeight / 2f, 0);

        tuchWallNorth = false;
        tuchWallSouth = false;
        tuchWallEast = false;
        tuchWallWest = false;

    }

    // Update is called once per frame
    void Update()
    {

        walltouching();
        wallMovingPermission();

        Vector3 forward = crossHair.transform.TransformDirection(Vector3.forward) * 100;

        Debug.DrawRay(crossHair.transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out test))
        {
            dist = test.distance;
            Debug.Log(dist + " " + test.collider.gameObject.name);
        }

        debugMode();
    }

    /// <summary>
    /// gestion si un mur est toucher
    /// </summary>
    private void walltouching()
    {
        if (crossHair.rectTransform.position.x <= 0)
        {
            tuchWallWest = true;
        }
        else
        {
            tuchWallWest = false;
        }

        if (crossHair.rectTransform.position.x >= maxWidth)
        {
            tuchWallEast = true;
        }
        else
        {
            tuchWallEast = false;
        }

        if (crossHair.rectTransform.position.y <= 0)
        {
            tuchWallSouth = true;
        }
        else
        {
            tuchWallSouth = false;
        }

        if (crossHair.rectTransform.position.y >= maxHeight)
        {
            tuchWallNorth = true;
        }
        else
        {
            tuchWallNorth = false;
        }

    }

    /// <summary>
    /// si l'input est positif 
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    private bool isPositiveInput(float f) {
        if (f >= 0)
        {
            return true;
        }
        else {
            return false;
        }

    }

    /// <summary>
    /// production du vecteur de déplacement
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Vector2 borderDeplacement(string i) {
        Vector2 move;
        switch (i)
        {
            case "North":
                move = new Vector2(crossHair.rectTransform.position.x + (Input.GetAxis(horizontal) * multiplicator), maxHeight);
                break;
            case "South":
                move = new Vector2(crossHair.rectTransform.position.x + (Input.GetAxis(horizontal) * multiplicator), 0);
                break;
            case "East":
                move = new Vector2(maxWidth, crossHair.rectTransform.position.y + (Input.GetAxis(vertical) * multiplicator));
                break;
            case "West":
                move = new Vector2(0, crossHair.rectTransform.position.y + (Input.GetAxis(vertical) * multiplicator));
                break;
            default:
                move = new Vector2(crossHair.rectTransform.position.x + (Input.GetAxis(horizontal) * multiplicator), crossHair.rectTransform.position.y + (Input.GetAxis(vertical) * multiplicator));
                break;
        }
        return move;
    }

    /// <summary>
    /// permet de dire quelle est la permission de mouvement
    /// </summary>
    private void wallMovingPermission() {
        if (tuchWallNorth)
        {
            if (isPositiveInput(Input.GetAxis(vertical)))
            {
                input = borderDeplacement("North");
            }
            else
            {
                input = borderDeplacement("");
            }
        }
        else if (tuchWallSouth)
        {
            if (!isPositiveInput(Input.GetAxis(vertical)))
            {
                input = borderDeplacement("South");
            }
            else
            {
                input = borderDeplacement("");
            }
        }
        else if (tuchWallEast)
        {
            if (isPositiveInput(Input.GetAxis(horizontal)))
            {
                input = borderDeplacement("East");
            }
            else
            {
                input = borderDeplacement("");
            }
        }
        else if (tuchWallWest)
        {
            if (!isPositiveInput(Input.GetAxis(horizontal)))
            {
                input = borderDeplacement("West");
            }
            else
            {
                input = borderDeplacement("");
            }
        }
        else
        {
            input = borderDeplacement("");
        }
        crossHair.rectTransform.position = input;
    }

    /// <summary>
    /// debug mode
    /// </summary>
    private void debugMode() {
        if (debugModeIsActive) {
            Debug.DrawLine(crossHair.rectTransform.position, new Vector3(Input.GetAxis(horizontal), 0, Input.GetAxis(vertical)));

            Debug.Log("tuchWallWest : " + tuchWallWest);
            Debug.Log("tuchWallEast : " + tuchWallEast);
            Debug.Log("tuchWallSouth : " + tuchWallSouth);
            Debug.Log("tuchWallNouth : " + tuchWallNorth);
        }
        
    }
}


