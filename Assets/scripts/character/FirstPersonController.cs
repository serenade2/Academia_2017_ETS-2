using System;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Tooltip("The gameObject at witch the camera performs it's rotation")]
    public GameObject Target;
    public float height = 1.80f;

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
        Vector3 positiveRotation = new Vector3(0,0,1);

        //if (horizontal1Input > 0.0f)
        //{
        //    m_Camera.transform.Translate(Vector3.right * Time.deltaTime);
        //}
        //else if (horizontal1Input < 0.0f)
        //{
        //    m_Camera.transform.Translate(Vector3.left * Time.deltaTime);
        //}
        //else if (vertical1Input > 0.0f)
        //{
        //    m_Camera.transform.Translate(Vector3.forward * Time.deltaTime);
        //}
        //else if (vertical1Input < 0.0f)
        //{
        //    m_Camera.transform.Translate(Vector3.back * Time.deltaTime);
        //}
        //else
        //{
        //    // dead zone
        //    transform.Translate(Vector3.zero);
        //}

        if (rightJoyY1Input > 0.0f && m_Camera.transform.rotation.x < VerticalClamp)
        {
            Debug.Log("RS+:" + rightJoyY1Input);
            m_Camera.transform.RotateAround(Target.transform.position, Vector3.right, 20 * Time.deltaTime);
           
            //m_Camera.transform.Rotate(Vector3.right);
        }
        else if (rightJoyY1Input < 0.0f && m_Camera.transform.rotation.x > -VerticalClamp)
        {
            //m_Camera.transform.Rotate(Vector3.left);
            m_Camera.transform.RotateAround(Target.transform.position, Vector3.left, 20 * Time.deltaTime);
        }
     
       
    }
}

