using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColourRenderer = null;

    [SyncVar(hook =nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook =nameof(HandleDisplayColourUpdated))]
    [SerializeField]
    private Color displayColour = Color.black;


    #region Server

    [Server]
    public void SetDisplayName(string newDisplayname)
    {


        displayName = newDisplayname;
    }

    [Server]
    public void SetDisplayColour(Color newDisplayColour)
    {
        displayColour = newDisplayColour;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        RPCLogNewName(newDisplayName);

        if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }

        SetDisplayName(newDisplayName);
    }


    #endregion

    #region Client

    private void HandleDisplayNameUpdated(string oldName ,string newName)
    {
        displayNameText.text = newName;
    }

    private void HandleDisplayColourUpdated(Color oldColour, Color newColour)
    {
        displayColourRenderer.material.SetColor("_BaseColor", newColour);
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }

    [ClientRpc]
    private void RPCLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }


    #endregion


}
