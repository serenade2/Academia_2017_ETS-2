using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionColider : MonoBehaviour {

    public Canvas anCanvas;
    public string InputManette;
    public float FillingRate;

    private InteractionManager interactionManager;

    public void Start()
    {
        interactionManager = GetComponent<InteractionManager>();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Hacker") {
            Debug.Log("stay");
            if (Input.GetButton("Submit"))
            {
                Debug.Log(InputManette);
                interactionManager.inProgress(FillingRate);
            }
            else {
                interactionManager.abandonedProgress(); // arrete le remplisage
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        interactionManager.abandonedProgress(); // arrete le remplisage
    }

}
