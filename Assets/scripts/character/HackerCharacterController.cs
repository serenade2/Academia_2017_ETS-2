using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// ThirdPerson controller for the Hacker
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HackerCharacterController : NetworkBehaviour, IRewindable
{
    public GameObject hackerCameraPrefab;
    public float speed = 10f;
    private bool canMove = true;
    private CharacterController ctrl;
    private NetworkAnimator hackerNetworkAnimator;
    private float velocity;
    public GameObject LoseDisplay;
    public GameObject WinDisplay;
    private SoundManager soundManager;
    private GameObject _loseDisplay;
    private GameObject _winDisplay;
    // Use this for initialization
    void Start()
    {
        this.ctrl = GetComponent<CharacterController>();
        hackerNetworkAnimator = GetComponent<NetworkAnimator>();

        if (hasAuthority)
        {
            GameObject.Instantiate(hackerCameraPrefab);
        }

        soundManager = GameObject.FindObjectOfType<SoundManager>();
        soundManager.GameStartPlay();
        _loseDisplay = GameObject.Instantiate(LoseDisplay, this.transform.position, this.transform.rotation);
        _loseDisplay.SetActive(false);
        _winDisplay = GameObject.Instantiate(WinDisplay, this.transform.position, this.transform.rotation);
        _winDisplay.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (!hasAuthority)
        {
            return;
        }


        if (canMove)
	    {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal1"), 0f, Input.GetAxis("Vertical1"));
            UpdateAnimation(input, speed);
            velocity = input.magnitude * speed;
            CmdSetVelocity(velocity);
            //CmdUpdateAnimation(input, speed);
            //TODO resume FROM HERE hackerNetworkAnimator.set
            //align the model to the same direction the character is moving.
            //hackerNetworkAnimator.transform.forward = this.transform.forward; 
            ctrl.SimpleMove(input * speed);
        }
    }

    public void Rewind(bool isRewinding)
    {
        if (isRewinding)
        {
            canMove = false;
            hackerNetworkAnimator.animator.speed = -1;
        }
        else
        {
            canMove = true;
            hackerNetworkAnimator.animator.speed = 1;
        }
    }

    public void Pause(bool isPaused)
    {
        if(isPaused == true)
        {
            hackerNetworkAnimator.animator.speed = 0;
            RpcPause(isPaused);
        }
        else
        {
            hackerNetworkAnimator.animator.speed = 1;
            RpcPause(isPaused);
        }
    }

    [ClientRpc]
    public void RpcPause(bool isPaused)
    {
        if (isPaused == true)
        {
            hackerNetworkAnimator.animator.speed = 0;
        }
        else
        {
            hackerNetworkAnimator.animator.speed = 1;
        }
    }

    public void FastForward(bool fastForward)
    {

    }

    public void UpdateAnimation(Vector3 direction, float speed)
    {
        hackerNetworkAnimator.animator.SetFloat("Speed", direction.sqrMagnitude * speed);

        //Update the direction https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
        if(direction != Vector3.zero)
        {
            float step = speed * Time.deltaTime * 3.5f;
            Vector3 rotationDirection = Vector3.RotateTowards(hackerNetworkAnimator.transform.forward, direction, step, 0.0f);
            //Debug.DrawRay(hackerNetworkAnimator.transform.position, rotationDirection, Color.cyan);
            hackerNetworkAnimator.transform.rotation = Quaternion.LookRotation(rotationDirection);
        }
    }

    [Command]
    public void CmdUpdateAnimation(Vector3 direction, float speed)
    {
        UpdateAnimation(direction, speed);
    }

    public float GetVelocity()
    {
        return velocity;
    }

    [Command]
    public void CmdSetVelocity(float velocity)
    {
        this.velocity = velocity;
    } 

    public void UpdateAnimation(float speed)
    {
        hackerNetworkAnimator.animator.SetFloat("Speed", speed);
    }

    public void ShowWinDisplay()
    {
        _winDisplay.SetActive(true);
    }

    public void PlayLoseSound()
    {
        soundManager.PlayLosingClip(false);
    }

    public void DisplayLoseScreen()
    {
        _loseDisplay.SetActive(true);
    }

    [ClientRpc]
    public void RpcDisplayLoseScreen()
    {
        if (hasAuthority)
        {
            DisplayLoseScreen();
        }
    }
    [ClientRpc]
    public void RpcPlayLoseSound()
    {
        if (hasAuthority)
        {
            PlayLoseSound();
        }
    }
}
