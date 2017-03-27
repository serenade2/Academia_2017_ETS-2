using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControllerInputShow : MonoBehaviour
{
    public GameObject prefab;
    private GameObject test;

    public float showInputsOnStartupTime = 5f;

	// Use this for initialization
	void Start ()
	{
        test=GameObject.Instantiate(prefab);
	    StartCoroutine(ShowInputsBriefly());


	}

    IEnumerator ShowInputsBriefly()
    {
        yield return new WaitForSeconds(showInputsOnStartupTime);
        test.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {



		if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
            test.SetActive(true);

		}
        else if (Input.GetKeyUp(KeyCode.Joystick1Button6) || Input.GetKeyUp(KeyCode.Joystick1Button7))
		{
            
            test.SetActive(false);
        }

    }

}
