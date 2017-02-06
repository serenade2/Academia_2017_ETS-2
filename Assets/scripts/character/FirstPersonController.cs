using System;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float height = 1.80f;

    [SerializeField] private MouseLook m_MouseLook;

    private Camera m_Camera;
    private float m_YRotation;
    private Vector2 m_Input;
    private CollisionFlags m_CollisionFlags;
    
    private void Start()
    {
        m_Camera = GetComponentInChildren<Camera>();
        Vector3 cameraPosition = new Vector3(this.transform.position.x, this.transform.position.y + this.height/2, this.transform.position.z);
        m_Camera.transform.position = cameraPosition;

        m_MouseLook.Init(transform , m_Camera.transform);
    }

    private void Update()
    {
        RotateView();
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation (transform, m_Camera.transform);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.3f, this.height, 0.3f));
    }
}

