using System.Collections;
using System.Collections.Generic;
using Kino;
using UnityEngine;

public class CameraGlitchRewind : MonoBehaviour, IRewindable
{
    public float blurrTimeOnPause = 0.2f;


    public void Rewind(bool isRewinding)
    {
        Camera.main.GetComponent<DigitalGlitch>().enabled = isRewinding;
        Camera.main.GetComponent<AnalogGlitch>().enabled = isRewinding;
    }

    public void Pause(bool isPaused)
    {
        if(isPaused)
            StartCoroutine(BlurrScreen());
    }

    public void FastForward(bool isFastForwarding)
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator BlurrScreen()
    {
        var glitch = Camera.main.GetComponent<AnalogGlitch>();
        glitch.enabled = true;
        glitch.colorDrift = 0.7f;
        glitch.scanLineJitter = 0.7f;
        glitch.horizontalShake = 0.7f;
        yield return new WaitForSeconds(blurrTimeOnPause);
        glitch.enabled = false;
        glitch.colorDrift = 0.275f;
        glitch.scanLineJitter = 0.24f;
    }
}
