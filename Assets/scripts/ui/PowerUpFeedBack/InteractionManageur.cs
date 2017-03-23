using System.Collections;
using System.Collections.Generic;
using CustomUtile;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManageur : MonoBehaviour, Observer
{

    public Image pauseImage;
    public Image RewindImage;
    private RewindManager rM;

    private void Start()
    {
        pauseImage.fillAmount = 1f;
        RewindImage.fillAmount = 1f;

        rM = GameObject.FindObjectOfType<RewindManager>();
        rM.addObserver(this);
    }

    public void updateObserver()
    {
        float ratio = rM.CurrentTime /rM.CooldownTime;
        if (ratio > 0.985f)
            ratio = 1f;
        pauseImage.fillAmount = ratio;
        RewindImage.fillAmount = ratio;

    }
}
