using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionColider : MonoBehaviour {
    
    private InteractionManager interactionManager;

    public void Start()
    {
        interactionManager = GetComponentInParent<InteractionManager>();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Hacker") {
            if (Input.GetButton("Submit"))
            {
                interactionManager.InProgress();
            }
            else {
                interactionManager.AbandonedProgress(); // arrete le remplisage
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hacker")
            interactionManager.AbandonedProgress(); // arrete le remplisage
    }

}
