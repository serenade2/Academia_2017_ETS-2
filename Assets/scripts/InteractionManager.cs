/**
 * cree par Julien Thibault
 * 
 * Academia hiver 2017
 * 
 * ------------------------------------
 * Source et inspiration
 * ------------------------------------
 * https://www.youtube.com/watch?v=9W0xLonwbLo
 * https://www.youtube.com/watch?v=5BobLzmqhNE&t=198s
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour {

    public Image ProgressBar;

    private float currentProgress = 0;
    private float MAX_PROGRESS = 100;

	// Use this for initialization
	void Start () {
        UpdateProgress();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateProgress() {
        float ratio = currentProgress / MAX_PROGRESS;
        ProgressBar.rectTransform.localScale = new Vector3(ratio,1,1);
    }

    public void inProgress(float progressFactor) {
        currentProgress += progressFactor;
        if (currentProgress > MAX_PROGRESS) {
            currentProgress = MAX_PROGRESS;
            UpdateProgress();
            if (currentProgress == MAX_PROGRESS) {
                resetProgress();
            }
        }
        UpdateProgress();
    }

    public void abandonedProgress() {
        Debug.Log("abandonedProgress");
        currentProgress = 0;
        UpdateProgress();
    }

    private void resetProgress() {
        Debug.Log("Finish");
        currentProgress = 0;
    }


}
