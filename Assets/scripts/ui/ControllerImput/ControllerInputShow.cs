using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControllerInputShow : MonoBehaviour
{
    public GameObject prefab;
    private GameObject test;

	// Use this for initialization
	void Start ()
	{
        test=GameObject.Instantiate(prefab);
        test.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
            test.SetActive(true);

		}
        else if (Input.GetKeyUp(KeyCode.Joystick1Button7))
		{
            test.SetActive(false);
        }

    }

}
