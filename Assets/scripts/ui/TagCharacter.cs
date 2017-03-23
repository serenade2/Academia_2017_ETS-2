using cakeslice;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TagCharacter : NetworkBehaviour {
    public GameObject trailPrefab;
    public Transform spawnPosition;
    [SyncVar]
    private bool isTagged = false;
    private Outline outline;
    private Coroutine trailSpawn;

    void Awake()
    {
        // get a reference to the component before disabling it
        outline = GetComponentInChildren<Outline>();
        outline.enabled = isTagged;
    }

    public void Tag()
    {
        outline.enabled = true;
        RpcChangeState(true);
        isTagged = true;
        trailSpawn = StartCoroutine(TrailSpawn());
    }

    public void UnTag()
    {
        outline.enabled = false;
        RpcChangeState(false);
        isTagged = false;
        StopCoroutine(trailSpawn);
    }

    public bool GetIsTagged()
    {
        return isTagged;
    }

    [ClientRpc]
    public void RpcChangeState(bool tagged)
    {
        outline.enabled = tagged;
    }

    public IEnumerator TrailSpawn()
    {
        while (true)
        {
            Instantiate(trailPrefab, spawnPosition.position + new Vector3(Random.Range(-0.5f,0.5f), 0, Random.Range(-0.5f, 0.5f)), trailPrefab.transform.rotation);
            yield return new WaitForSeconds(Random.Range(0.2f,0.5f));
        }
    }
}
