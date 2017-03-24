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
    public float fillingRate = 1f;
    public ActivatableObjective activatable;

    private float currentProgress = 0;
    private float MAX_PROGRESS = 100;

	// Use this for initialization
	void Start () {
        UpdateProgress();
    }

    private void UpdateProgress() {
        float ratio = currentProgress / MAX_PROGRESS;
        ProgressBar.fillAmount = ratio;
    }

    public void InProgress() {
        
        currentProgress += fillingRate;
        if (currentProgress > MAX_PROGRESS) {
            currentProgress = MAX_PROGRESS;
            UpdateProgress();
            
            if (currentProgress >= MAX_PROGRESS) {
                CompletedAction();
                ResetProgress();
            }
        }
        UpdateProgress();
    }

    public void AbandonedProgress() {
        currentProgress = 0;
        UpdateProgress();
    }

    private void ResetProgress() {
        currentProgress = 0;
    }


    private void CompletedAction()
    {
        activatable.gameObject.SetActive(false);
        //ObjectiveManager.Instance.CmdCompleteObjective(activatable.gameObject);
    }
}
