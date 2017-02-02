using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionColider : MonoBehaviour {

    public Canvas anCanvas;
    public float ratio;


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            Debug.Log("stay");
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E");
                anCanvas.GetComponent<InteractionManager>().inProgress(ratio);
            }
            else {
                anCanvas.GetComponent<InteractionManager>().abandonedProgress();
            }
        }
        
    }

}
