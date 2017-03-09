using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowingZone : MonoBehaviour {
    public GameObject elementToShow;

    void Start() {
        OnStartSetUp();
    }

    private void OnStartSetUp()
    {
        
       if (elementToShow.activeInHierarchy) {
            elementToShow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hacker")
        {
            elementToShow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hacker")
        {
            elementToShow.SetActive(false);
        }
    }
}
