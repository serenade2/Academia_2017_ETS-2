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
    // Use this for initialization

    public override void OnStartAuthority()
    {
       
    }

    void Start()
    {
        var materials = GetComponentsInChildren<Material>();
        setCursorColors(materials);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = Target.transform.position + CursorOffset;
    }

    void setCursorColors(Material[] materials)
    {
        foreach(Material mat in materials)
        {
            mat.color = CursorColor.color;
        }
    }
}
