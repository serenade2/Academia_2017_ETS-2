using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGlitch : MonoBehaviour
{

    public ChromaticRB cameraEffect;

    public float minFlickerRate = 0.2f;
    public float maxFlickerRate = 2f;
    public float minEffectValue = 1f;
    public float maxEffectValue = 2f;
    public float minTimeOn = 0.1f;
    public float maxTimeOn = 0.3f;
    public float lerpStep = 0.1f;

    // Use this for initialization
    void Start () {

        if (!cameraEffect)
	    {
            cameraEffect = GetComponent<ChromaticRB>();
	    }

	    StartCoroutine(FlickerEffect());
	}

    IEnumerator FlickerEffect()
    {
        while (true)
        {
            float value = Random.Range(minEffectValue, maxEffectValue);
            StartCoroutine(LerpValue(0f, value, lerpStep));
            yield return new WaitForSeconds(Random.Range(minTimeOn, maxTimeOn));
            
            StartCoroutine(LerpValue(value, 0f, lerpStep));

            yield return new WaitForSeconds(Random.Range(minFlickerRate,maxFlickerRate));
        }
    }

    IEnumerator LerpValue(float minValue, float maxValue, float step)
    {
        float progress = 10f;

        while (progress > 0f)
        {
            cameraEffect.ChromaticAbberation = Mathf.Lerp(minValue, maxValue, progress);
            progress -= step;
            yield return null;
        }
    }
}
