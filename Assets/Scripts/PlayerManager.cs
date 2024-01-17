using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public Dictionary<ulong , GameObject> players;

    public static PlayerManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player Manager in the scene.");  
        }
        instance = this;
        players = new Dictionary<ulong, GameObject>();
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
    }

    private void Update()
    {
        Debug.Log("Player count: " + players.Count);
    }

    private void OnClientConnect(ulong clientId)
    {
        AddPlayerClientRpc(clientId);
    }

    public void AddPlayer(ulong clientId, GameObject player)
    {
        // never add the same key twice
        if (players.ContainsKey(clientId)) return;
        players.Add(clientId, player);
    }

    [ClientRpc]
    public void AddPlayerClientRpc(ulong clientId)
    {

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Debug.Log("ran");
            if (player.GetComponent<Character>().clientId == clientId)
            {
                AddPlayer(clientId, player);
            }
        }
    }
    // the big idea of this is to be able to access a local player based on their clientid
}