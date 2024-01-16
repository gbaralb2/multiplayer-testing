using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class EventManager : NetworkBehaviour
{
    public void StartEvent(string eventName)
    {
        if (eventName.Equals("SWITCH_ARCHER")) {SwitchArcher(); SwitchArcherServerRpc(); SwitchArcherClientRpc();}
        if (eventName.Equals("SWITCH_PAWN")) {SwitchPawn(); SwitchPawnServerRpc(); SwitchPawnClientRpc();}
    }

    private void SwitchPawn()
    {
        gameObject.GetComponent<Archer>().enabled = false;
        gameObject.GetComponent<Pawn>().enabled = true;
        gameObject.GetComponent<Character>().SetClass("pawn");
        gameObject.GetComponent<Character>().data.charClass = "pawn";
    }

    [ServerRpc(RequireOwnership = false)]
    private void SwitchPawnServerRpc()
    {
        gameObject.GetComponent<Archer>().enabled = false;
        gameObject.GetComponent<Pawn>().enabled = true;
    }

    [ClientRpc]
    private void SwitchPawnClientRpc()
    {
        gameObject.GetComponent<Archer>().enabled = false;
        gameObject.GetComponent<Pawn>().enabled = true;
    }

    private void SwitchArcher()
    {
        gameObject.GetComponent<Pawn>().enabled = false;
        gameObject.GetComponent<Archer>().enabled = true;
        gameObject.GetComponent<Character>().SetClass("archer");
        gameObject.GetComponent<Character>().data.charClass = "archer";
    }

    [ServerRpc(RequireOwnership = false)]
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
