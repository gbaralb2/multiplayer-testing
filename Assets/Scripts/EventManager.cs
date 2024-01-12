using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EventManager : NetworkBehaviour
{
    public void StartEvent(string eventName)
    {
        if (eventName.Equals("SWITCH_ARCHER")) {SwitchArcher(); SwitchArcherServerRpc(); SwitchArcherClientRpc();}
    }

    private void SwitchArcher()
    {
        gameObject.GetComponent<Pawn>().enabled = false;
        gameObject.GetComponent<Archer>().enabled = true;
    }

    [ServerRpc]
    private void SwitchArcherServerRpc()
    {
        gameObject.GetComponent<Pawn>().enabled = false;
        gameObject.GetComponent<Archer>().enabled = true;
    }

    [ClientRpc]
    private void SwitchArcherClientRpc()
    {
        gameObject.GetComponent<Pawn>().enabled = false;
        gameObject.GetComponent<Archer>().enabled = true;
    }
}
