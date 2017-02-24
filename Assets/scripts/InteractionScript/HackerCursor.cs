using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HackerCursor : NetworkBehaviour
{
    [Tooltip("The game object at which the cursor will point")]
    public GameObject Target;
    [Tooltip("The cursor of the hacker")]
    public GameObject Cursor;
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
        Material cursorMaterial = Cursor.GetComponent<MeshRenderer>().material;
        cursorMaterial.color = CursorColor.color;
        this.gameObject.SetActive(IsVisible);
    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.SetActive(IsVisible);

        if(Target == null)
        {
            // make the parent cursor the target
            transform.position = this.transform.parent.position + CursorOffset;
        }
        else
        {
            transform.position = Target.transform.position + CursorOffset;
        }
     
        //compute the cursor rotation speed
        Vector3 newSpeed = new Vector3(transform.rotation.x + RotationSpeed.x, transform.rotation.y + RotationSpeed.y, transform.rotation.z + RotationSpeed.z);
        transform.Rotate(newSpeed * Time.deltaTime);
    }

    public void DisableCursor()
    {
        // IsVisible = false;
        this.gameObject.SetActive(false);
    }

    public void EnableCursor()
    {
        //this.IsVisible = true;
        this.gameObject.SetActive(true);
    }
}
