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
        
       if (elementToShow.active) {
            elementToShow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        elementToShow.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        elementToShow.SetActive(false);
    }
}
