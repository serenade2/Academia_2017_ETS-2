using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIReticleSelector : NetworkBehaviour {
    public GameObject reticle;
    public Color color;
    public LayerMask layer;
    private List<TagCharacter> tags = new List<TagCharacter>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal1") * Time.deltaTime * 3, 0, Input.GetAxis("Vertical1") * Time.deltaTime * 3, Space.World);
        RaycastHit hit;
        if (Physics.Raycast(transform.position - transform.TransformDirection(Vector3.up * 0.1f), transform.TransformDirection(Vector3.back), out hit, 50f, layer))
        {

            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                if (hit.collider.GetComponent<TagCharacter>().GetIsTagged())
                {
                    tags.Remove(hit.collider.GetComponent<TagCharacter>());
                    hit.collider.GetComponent<TagCharacter>().UnTag();
                }
                else
                {
                    if (tags.Count <= 2)
                    {
                        tags.Add(hit.collider.GetComponent<TagCharacter>());
                        hit.collider.GetComponent<TagCharacter>().Tag(color);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Destroy(hit.collider.gameObject.transform.parent.gameObject);
            }

        }
    }
}
