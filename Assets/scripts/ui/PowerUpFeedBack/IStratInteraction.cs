using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStratInteraction
{

    void usePower();
    void upDateRatio(GameObject g);
    void startParticulSystem(ParticleSystem ps);
    void stopParticulSystem(ParticleSystem ps);
    float getTimeLeft();

}
