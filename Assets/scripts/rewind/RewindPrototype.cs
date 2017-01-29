using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindPrototype : MonoBehaviour {
    private Stack<CharacterMemento> stateHistory = new Stack<CharacterMemento>();
    private float recordDelay = 0.05f;
    private IEnumerator record;
    private IEnumerator rewind;
    private bool isRecordRunning = false;
    private bool isRewindRunning = false;
    private Vector3 rewindVelocity;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        record = Record();
        rewind = Rewind();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude > 0)
        {
            if(!isRecordRunning){
                StartCoroutine(record);
                isRecordRunning = true;
            }
        }
        else
        {
            StopCoroutine(record);
            isRecordRunning = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (stateHistory.Count > 0) {
                StopCoroutine(record);
                isRecordRunning = false;
                StartCoroutine(rewind);
                rb.isKinematic = true;
                isRewindRunning = true;
            }
        }else if(Input.GetKeyUp(KeyCode.R)){
            StopCoroutine(rewind);
            rb.isKinematic = false;
            isRewindRunning = false;
        }

	}

    private CharacterMemento ToMemento()
    {
        return new CharacterMemento(transform.position);
    }

    private void FromMemento(CharacterMemento memento)
    {
        this.transform.position = memento.GetPos();
    }

    IEnumerator Record()
    {
        while (true){
            stateHistory.Push(ToMemento());
            yield return new WaitForSeconds(recordDelay);
        }
    }

    IEnumerator Rewind()
    {
        Vector3 startPos = Vector3.zero;
        Vector3 nextPos = Vector3.zero;
        float i = 0;
        while (true)
        {
            if (stateHistory.Count > 0)
            {
                if(i == 0){
                    startPos = transform.position;
                    nextPos = stateHistory.Pop().GetPos();
                }
                transform.position = Vector3.Lerp(startPos, nextPos, i);
                //transform.position = stateHistory.Pop().GetPos();
            }
            else
            {
                StopCoroutine(rewind);
                isRewindRunning = false;
            }
            i += 0.25f;
            if(i > 1){
                i = 0;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    /*Play()
    {
       stateHistory
    }*/

    void Pause()
    {
       
    }

    void LateUpdate()
    {
        if (!isRewindRunning)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal") * 2, rb.velocity.y, Input.GetAxis("Vertical") * 2);
        }
        else
        {

        }
    }
}
