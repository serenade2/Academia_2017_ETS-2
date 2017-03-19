using cakeslice;
using UnityEngine;
using UnityEngine.Networking;

public class TagCharacter : NetworkBehaviour {

    [SyncVar]
    private bool isTagged = false;
    private Outline outline;

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
    }

    public void UnTag()
    {
        outline.enabled = false;
        RpcChangeState(false);
        isTagged = false;
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
}
