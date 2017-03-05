using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
public class HackerCursor : MonoBehaviour
{
    [Tooltip("The game object at which the cursor will point")]
    public GameObject Target;
    [Tooltip("The cursor of the hacker")]
    public GameObject CursorArrow;
    [Tooltip("The color of the cursor")]
    public Material CursorColor;
    [Tooltip("The space between the cursor and the gameObject")]
    public Vector3 CursorOffset;
    [Tooltip("The speed at which the cursor will spin")]
    public Vector3 RotationSpeed;
    [Tooltip("Determine if the cursor is visible")]
    public bool IsVisible;
    // Use this for initialization
    void Start()
    {
        Material cursorMaterial = CursorArrow.GetComponent<MeshRenderer>().material;
        cursorMaterial.color = CursorColor.color;
        CursorArrow.SetActive(IsVisible);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null || !IsVisible)
        {
            DisableCursor();
        }
        else
        {  
            EnableCursor();
            transform.position = Target.transform.position + CursorOffset;
            //compute the cursor rotation speed
            Vector3 newSpeed = new Vector3(transform.rotation.x + RotationSpeed.x, transform.rotation.y + RotationSpeed.y, transform.rotation.z + RotationSpeed.z);
            transform.Rotate(newSpeed * Time.deltaTime);
        }
    }

    public void SetTarget( GameObject ai)
    {
        Target = ai;
    }

    public void DisableCursor()
    {
        // Disable the cursor pointer
        CursorArrow.SetActive(false);
    }

    public void EnableCursor()
    {
        CursorArrow.SetActive(true);
    }
}
