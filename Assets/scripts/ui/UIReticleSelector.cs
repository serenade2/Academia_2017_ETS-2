using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIReticleSelector : NetworkBehaviour {
    public GameObject reticle;
    public Color color;
    public LayerMask layer;
    private List<TagCharacter> tags = new List<TagCharacter>();
    RaycastHit hit;
    TagCharacter characterTag;
    DestroyCharacter characterDestroy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal1") * Time.deltaTime * 3, 0, Input.GetAxis("Vertical1") * Time.deltaTime * 3, Space.World);
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (Physics.Raycast(transform.position - transform.TransformDirection(Vector3.up * 0.1f), transform.TransformDirection(Vector3.back), out hit, 50f, layer))
            {
                if(hit.collider.transform.parent.GetComponent<TagCharacter>() != null)
                {
                    characterTag = hit.collider.transform.parent.GetComponent<TagCharacter>();
                    if (characterTag.GetIsTagged())
                    {
                        tags.Remove(characterTag);
                        characterTag.UnTag();
                    }
                    else
                    {
                        if (tags.Count <= 2)
                        {
                            tags.Add(characterTag);
                            characterTag.Tag(color);
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (Physics.Raycast(transform.position - transform.TransformDirection(Vector3.up * 0.1f), transform.TransformDirection(Vector3.back), out hit, 50f, layer))
            {
                if (hit.collider.transform.parent.GetComponent<DestroyCharacter>() != null)
                {
                    characterDestroy = hit.collider.transform.parent.GetComponent<DestroyCharacter>();
                    characterDestroy.RpcDestroy();
                }
            }
        }
    }
}
