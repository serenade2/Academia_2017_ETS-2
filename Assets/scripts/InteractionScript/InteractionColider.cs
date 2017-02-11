using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionColider : MonoBehaviour {

    public Canvas anCanvas;
    public string InputManette;
    public float FillingRate;


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            Debug.Log("stay");
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log(InputManette);
                anCanvas.GetComponent<InteractionManager>().inProgress(FillingRate);
            }
            else {
                anCanvas.GetComponent<InteractionManager>().abandonedProgress(); // arrete le remplisage
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        anCanvas.GetComponent<InteractionManager>().abandonedProgress(); // arrete le remplisage
    }

}
