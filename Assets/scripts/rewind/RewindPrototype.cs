using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindPrototype : MonoBehaviour {
    private Stack<IRewindMemento> stateHistory = new Stack<IRewindMemento>();

    private float recordDelay = 0.05f;

    Vector3 startPos;
    Vector3 nextPos;

    private Coroutine record;
    private Coroutine rewind;
    private Rigidbody rb;
    public int memento = 0;
    private RewindMementoFactory rewindMementoFactory = new RewindMementoFactory();
    private bool isRewinding = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        //Start recording
        record = StartCoroutine(Record());
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Joystick2Button4))
        {
            //Stop recording
            StopCoroutine(record);

            //Start rewinding
            rewind = StartCoroutine(Rewind());

            //Deactivate physic
            rb.isKinematic = true;
            if (memento == 1)
            {
                GetComponent<RewindMovement>().CanMove(false);
            }

        }else if(Input.GetKeyUp(KeyCode.Joystick2Button4)){
            //Stop rewinding
            StopCoroutine(rewind);

            //Start recording
            record = StartCoroutine(Record());

            //Activate physic
            rb.isKinematic = false;

            if (memento == 1)
            {
                GetComponent<RewindMovement>().CanMove(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
        {
            //Stop recording
            StopCoroutine(record);

            //Stop rewinding
            if(rewind != null)
                StopCoroutine(rewind);

            if (memento == 1)
            {
                GetComponent<RewindMovement>().CanMove(false);
            }

        }
        else if (Input.GetKeyUp(KeyCode.Joystick2Button1))
        {
            //Start recording
            record = StartCoroutine(Record());

            if (memento == 1)
            {
                GetComponent<RewindMovement>().CanMove(true);
            }

        }

	}

    IEnumerator Record()
    {
        while (true){
            stateHistory.Push(rewindMementoFactory.MakeRewindMemento(memento,gameObject));
            yield return new WaitForSeconds(recordDelay);
        }
    }

    IEnumerator Rewind()
    {
        while (true)
        {
            if (stateHistory.Count > 0)
            {
                stateHistory.Pop().RestoreFromMemento(gameObject);
            }
            else
            {
                //Stop rewinding
                StopCoroutine(rewind);
                if (memento == 1)
                {
                    GetComponent<RewindMovement>().CanMove(true);
                }

                rb.isKinematic = false;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    void Pause()
    {
       
    }

    void LateUpdate()
    {
        if(memento == 0)
            rb.velocity = new Vector3(Input.GetAxis("Horizontal1") * 1f, rb.velocity.y, Input.GetAxis("Vertical1") * 1f);
    }
}
