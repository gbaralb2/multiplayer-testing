using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using Unity.Networking.Transport;
using System;
using Mono.Cecil.Cil;
using System.Data.Common;

public class CharacterDatabase : NetworkBehaviour
{
    public Dictionary<ulong, Character.NetworkCharacterData> database;

    public static CharacterDatabase instance { get; private set; }
    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one CharacterDatabase in the scene.");  
        }
        instance = this;
        database = new Dictionary<ulong, Character.NetworkCharacterData>();
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
    }

    private void Update()
    {
        Debug.Log(database.Count);
    }

    public void UploadData(ulong clientId, Character.NetworkCharacterData data)
    {
        database.Add(clientId, data);
    }

    [ClientRpc]
    public void LoadServerCharDataClientRpc(ulong clientId, Character.NetworkCharacterData networkData)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<Character>().clientId == clientId)
            player.GetComponent<Character>().currentClass = networkData.charClass.ToString();
            player.GetComponent<Character>().LoadCharacter(player.GetComponent<Character>().data);
        }
    }

    private void SyncDatabase()
    {
        List<ulong> idList = new List<ulong>(database.Keys);
        if (NetworkManager.ConnectedClientsIds.Count != idList.Count)
        {
            foreach (ulong id in idList)
            {
                if (!NetworkManager.ConnectedClientsIds.Contains(id)) RemoveDataServerRpc(id);
            }
        }
        // foreach (ulong id in idList)
        // {
        //     if (!NetworkManager.ConnectedClientsIds.Contains(id)) RemoveDataServerRpc(id);
        // }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SyncDatabaseServerRpc()
    {
        SyncDatabase();
    }

    [ServerRpc]
    private void RemoveDataServerRpc(ulong clientId)
    {
        database.Remove(clientId);
    }

    private void OnClientConnect(ulong clientId)
    {
        SyncDatabaseServerRpc();
    }
}
