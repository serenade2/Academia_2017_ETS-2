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

        rM = GameObject.FindGameObjectWithTag("RewindManager").GetComponent<RewindManager>();
        rM.addObserver(this);
    }

    public void updateObserver()
    {
        float ratio = rM.CurrentTime /rM.CooldownTime;
        Debug.Log(ratio);
        pauseImage.fillAmount = ratio;
        RewindImage.fillAmount = ratio;

    }
}
