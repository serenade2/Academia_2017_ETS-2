using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CrosshairManager : NetworkBehaviour {

    public enum CameraMode { Rotation, Fps };
    public CameraMode cameraMode = CameraMode.Fps;
    public RawImage crossHair;
    private Camera worldCamera;
    public Canvas worldCanvas;
    public int maxTaggableCharacters = 3;
    public GameObject WinnningDisplay;
    private GameObject _winDisplay;
    RaycastHit hit;
    public LayerMask layer;
    TagCharacter characterTag;
    private List<TagCharacter> tags = new List<TagCharacter>();
    DestroyCharacter characterDestroy;

    float maxWidth;
    float maxHeight;

    private string horizontal = "RHorizontal1";
    private string vertical = "RVertical1";
    public int multiplicator;
    public bool debugModeIsActive;

    Vector2 input;

    RaycastHit test;
    float dist;
    private SoundManager soundManager;
    private ActorSoundManager actorSoundManager;
    // Use this for initialization
    void Start()
    {
        maxWidth = GetComponentInParent<Canvas>().pixelRect.width;
        maxHeight = GetComponentInParent<Canvas>().pixelRect.height;

        crossHair.rectTransform.position = new Vector3(maxWidth / 2f, maxHeight / 2f, 0);

        worldCamera = Camera.main;
        soundManager = FindObjectOfType<SoundManager>();
        _winDisplay = GameObject.Instantiate(WinnningDisplay, worldCamera.transform.position, worldCamera.transform.rotation);
        _winDisplay.SetActive(false);
    }

    void OnRectTransformDimensionsChange()
    {
        maxWidth = GetComponentInParent<Canvas>().pixelRect.width;
        maxHeight = GetComponentInParent<Canvas>().pixelRect.height;
    }


    // Update is called once per frame
    void Update()
    {
        // restrict crosshair movement to the screen size
        if (cameraMode == CameraMode.Rotation)
        {
            float newX = Mathf.Clamp(crossHair.rectTransform.position.x + (Input.GetAxis(horizontal) * multiplicator), 0f, maxWidth);
            float newY = Mathf.Clamp(crossHair.rectTransform.position.y + (Input.GetAxis(vertical) * multiplicator), 0f, maxHeight);
            Vector2 newPosition = new Vector2(newX, newY);
            crossHair.rectTransform.position = newPosition;
        }

        tagEvent();
        destroyEvent();
        debugMode();
    }

    private void destroyEvent()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            Ray ray = worldCamera.ScreenPointToRay(crossHair.transform.position);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 50f, layer))
            {
                if (hit.collider.transform.parent.GetComponent<DestroyCharacter>() != null)
                {

                    if (hit.collider.transform.gameObject.tag == "Hacker")
                    {
                        // temp hard code 
                        HackerCharacterController hackerController = hit.collider.transform.parent.GetComponent<HackerCharacterController>();
                        
                        // trigger victory for Watcher
                        Debug.Log("=================     Watcher WINS   ===================");
                        // play the winning sound
                        soundManager.PlayWinningClip(false);
                        DisplayWinningUIForWatcher();

                        hackerController.RpcDisplayLoseScreen();
                        hackerController.RpcPlayLoseSound();

                       
                        
                    }
                    else
                    {
                        characterDestroy = hit.collider.transform.parent.GetComponent<DestroyCharacter>();
                        // play 
                        characterDestroy.Destroy();
                    }
                }
            }
        }
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
                        if (tags.Count < maxTaggableCharacters)
                        {
                            tags.Add(characterTag);
                            characterTag.Tag();
                            Debug.Log("Tagged");
                        }
                    }
                }
            }
        }
    }

    private void DisplayWinningUIForWatcher()
    {
        _winDisplay.SetActive(true);
    }


    /// <summary>
    /// debug mode
    /// </summary>
    private void debugMode() {
        if (debugModeIsActive) {
            Debug.DrawLine(crossHair.rectTransform.position, new Vector3(0, 0, 0));       

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


