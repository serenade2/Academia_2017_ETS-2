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
        if (!rM.CooldownSplit)
        {
            pauseImage.fillAmount = RatioCalculateur(rM.CurrentTime, rM.CooldownTime);
            RewindImage.fillAmount = RatioCalculateur(rM.CurrentTime, rM.CooldownTime); ;
        }
        else
        {
            if (rM.PauseCooldown)
            {
                pauseImage.fillAmount = RatioCalculateur(rM.PauseCooldownTime, rM.CooldownTime);
            }
            else if (rM.RewindCooldown)
            {
                RewindImage.fillAmount = RatioCalculateur(rM.RewindCooldownTime, rM.CooldownTime);
            }

        }
    }

    public float RatioCalculateur(float f1, float f2)
    {
        float ratio = f1 / f2;
        if (ratio > 0.985f)
            ratio = 1f;
        return ratio;
    }
}
