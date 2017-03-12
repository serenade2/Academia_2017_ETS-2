using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour {

    public RawImage test;

    float maxWidth;
    float maxHeight;

    Vector2 input;


    // Use this for initialization
    void Start () {
        test.rectTransform.position = new Vector2(0f,0f);
        maxWidth = GetComponentInParent<Canvas>().pixelRect.width;
        maxHeight = GetComponentInParent<Canvas>().pixelRect.height;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown) {
            
        }

        Debug.Log(Input.GetAxis("Horizontal1"));
        Debug.DrawLine(test.rectTransform.position,new Vector3(Input.GetAxis("Horizontal1"), 0,  Input.GetAxis("Vertical1")));



        //    if (test.rectTransform.position.x >= 0)
        //{
        //    if (Input.GetAxis("Horizontal1") > 0)
        //    {

        //        input = new Vector2(test.rectTransform.position.x + Input.GetAxis("Horizontal1"), test.rectTransform.position.y + Input.GetAxis("Vertical1"));
        //    }
        //    else {

        //    }

        //}
        //else {
        //    input = new Vector2(test.rectTransform.position.x + Input.GetAxis("Horizontal1"), test.rectTransform.position.y +Input.GetAxis("Vertical1"));
        //}

        input = new Vector2(test.rectTransform.position.x + Input.GetAxis("Horizontal1"), test.rectTransform.position.y + Input.GetAxis("Vertical1"));


        test.rectTransform.position = input;
        
    }
}
