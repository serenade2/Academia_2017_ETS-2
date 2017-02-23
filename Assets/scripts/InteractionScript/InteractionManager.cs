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
    [Header("Interaction things")]
    [Tooltip("Rate at which the progress bar progresses each tick")]
    public float fillingRate;
    public ActivatableDoor activatableDoor;

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
        ProgressBar.fillAmount = ratio;
        //ProgressBar.rectTransform.localScale = new Vector3(ratio,1,1);
    }

    public void InProgress() {
        
        currentProgress += fillingRate;
        if (currentProgress > MAX_PROGRESS) {
            currentProgress = MAX_PROGRESS;
            UpdateProgress();
            Debug.Log("inProgress");
            if (currentProgress >= MAX_PROGRESS) {
                CompletedAction();
                ResetProgress();
            }
        }
        UpdateProgress();
    }

    public void AbandonedProgress() {
        Debug.Log("abandonedProgress");
        currentProgress = 0;
        UpdateProgress();
    }

    private void ResetProgress() {
        Debug.Log("Finish");
        currentProgress = 0;
    }

    private void CompletedAction()
    {
        activatableDoor.Activate();
    }



}
