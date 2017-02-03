/**
The following code have been inpired by the basic networking tutorial provided by Unity3d.
 source : https://unity3d.com/learn/tutorials/topics/multiplayer-networking
¸*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{	
    // Update is called once per frame
    void Update ()
    {
        transform.LookAt(Camera.main.transform);	
    }
}
