using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class FirstPersonController : MonoBehaviour
{
    [Tooltip("The gameObject at witch the camera performs it's rotation")]
    public GameObject Target;
    public float height = 1.80f;
    public float HorizontalSpeed = 5.0f;
    public float RotationAngle = 2.0f;
    [SerializeField] private MouseLook m_MouseLook;

    private Camera m_Camera;
    private float m_YRotation;
    private Vector2 m_Input;
    private CollisionFlags m_CollisionFlags;
    [Tooltip("The maximal value at which the camera stop moving further")]
    public float HorizontalClamp;
    [Tooltip("The maximal value at which the camera stop moving further")]
    public float VerticalClamp;
    public bool InvertVerticalAxys = false;
    private float m_rotationToPerform = 0.0f;
    private bool m_performRotationAroundZ = false;
    private void Start()
    {
        m_Camera = GetComponentInChildren<Camera>();
        Vector3 cameraPosition = new Vector3(this.transform.position.x, this.transform.position.y + this.height/2, this.transform.position.z);
        m_Camera.transform.position = cameraPosition;

       // m_MouseLook.Init(transform , m_Camera.transform);
    }

    private void Update()
    {
        //RotateView();
        ProcessInput();
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

    public void ProcessInput()
    {
        float horizontal1Input = Input.GetAxis("Horizontal1");
        float vertical1Input = Input.GetAxis("Vertical1");
        float rightJoyX1Input = Input.GetAxis("Right Joystick X 1");
        float rightJoyY1Input = Input.GetAxis("Right Joystick Y 1");
        float RT1 = Input.GetAxis("RT 1");
        float LT1 = Input.GetAxis("LT 1");
        Quaternion cameraRotation = m_Camera.transform.rotation;
        Vector3 positiveRotation = new Vector3(0,0,1);

        if (horizontal1Input == 1.0f)
        {
            m_Camera.transform.Translate(Vector3.right * HorizontalSpeed * Time.deltaTime);
        }
        else if (horizontal1Input == -1.0f)
        {
            m_Camera.transform.Translate(Vector3.left * HorizontalSpeed * Time.deltaTime);
        }

        if (rightJoyY1Input == 1.0f)
        {
            if (m_performRotationAroundZ)
            {
                m_rotationToPerform += (cameraRotation.z + RotationAngle);
            }
            else
            {
                m_rotationToPerform += (cameraRotation.x + RotationAngle);
            }
           

            if (m_rotationToPerform > VerticalClamp)
            {
                m_rotationToPerform = VerticalClamp;
            }

            if (m_performRotationAroundZ)
            {
                Debug.Log(m_performRotationAroundZ);
                m_Camera.transform.rotation = Quaternion.AngleAxis(m_rotationToPerform, Vector3.right);
            }
            else
            {
                m_Camera.transform.rotation = Quaternion.AngleAxis(m_rotationToPerform, Vector3.right);
            }
        }
        else if (rightJoyY1Input == -1.0f)
        {
            m_rotationToPerform += (cameraRotation.x + RotationAngle) * -1;
        
            if (m_rotationToPerform < -VerticalClamp)
            {
                m_rotationToPerform = -VerticalClamp;
            }

            m_Camera.transform.rotation = Quaternion.AngleAxis(m_rotationToPerform, Vector3.right);
        }
        //else if (RT1 == 1.0f)
        //{
        //    m_Camera.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        //    m_performRotationAroundZ = !m_performRotationAroundZ;
        //}
        //else if (LT1 == 1.0f)
        //{
        //    m_Camera.transform.rotation = Quaternion.AngleAxis(90, Vector3.down);
        //    m_performRotationAroundZ = !m_performRotationAroundZ;
        //}
    }
}

