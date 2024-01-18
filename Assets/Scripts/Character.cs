using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System;
using Unity.Networking.Transport;
using UnityEditor;

public class Character : NetworkBehaviour, ICharacterPersistence
{
    [SerializeField] private EventManager eventManager;
    // [SerializeField] private CharacterDatabase database;
    private FileDataHandler dataHandler;
    public ulong clientId;
    public CharacterData data;
    public NetworkCharacterData networkData;

    public string currentClass;

    public struct NetworkCharacterData : INetworkSerializable
    {
        public FixedString32Bytes charClass;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref charClass);
        }
    }

    private NetworkCharacterData NetworkifyCharData(CharacterData data)
    {
        NetworkCharacterData networkData = new NetworkCharacterData {
            charClass = data.charClass
        };
        return networkData;
    }

    void Start()
    {
        if (!IsOwner) return;

        dataHandler = new FileDataHandler(Application.persistentDataPath, "char.save", false);

        // Loading local character data on startup
        CharacterPersistenceManager.instance.LoadCharacter();

        // uploading character data to database
        clientId = NetworkManager.Singleton.LocalClientId;
        data = dataHandler.Load();
        networkData = NetworkifyCharData(data);
        UploadDataServerRpc(clientId, networkData);

        if (!IsHost)
        {
            RequestDataServerRpc();
        }

        // getting data for local player manager
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            // if key aleady exists
            if (PlayerManager.instance.players.ContainsKey(player.GetComponent<Character>().clientId)) continue;
            PlayerManager.instance.AddPlayer(player.GetComponent<Character>().clientId, player);
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (currentClass == "pawn")
        {
            eventManager.StartEvent("SWITCH_PAWN");
        }

        if (currentClass == "archer")
        {
            eventManager.StartEvent("SWITCH_ARCHER");
        }
    }

    public void SetClass(string charClass)
    {
        if (!IsOwner) return;

        currentClass = charClass;
    }

    public void LoadCharacter(CharacterData data)
    {
        if (!IsOwner) return;

        currentClass = data.charClass;

        if (currentClass == "pawn")
        {
            eventManager.StartEvent("SWITCH_PAWN");
        }

        if (currentClass == "archer")
        {
            eventManager.StartEvent("SWITCH_ARCHER");
        }
    }

    public void SaveCharacter(ref CharacterData data)
    {
        if (!IsOwner) return;
        data.charClass = this.currentClass;
    }


    [ServerRpc]
    private void UploadDataServerRpc(ulong clientId, NetworkCharacterData data, ServerRpcParams serverRpcParams = default)
    {
        CharacterDatabase.instance.UploadData(clientId, data);
        PlayerManager.instance.AddPlayer(serverRpcParams.Receive.SenderClientId, gameObject);
        AddPlayerClientRpc(serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void AddPlayerClientRpc(ulong clientId)
    {
        PlayerManager.instance.AddPlayer(clientId, gameObject);
    }

    [ServerRpc]
    private void RequestDataServerRpc()
    {
        for (int i = 0; i < CharacterDatabase.instance.database.Count; i++)
        {
            // this line is here because client id's never get reused, so this manages for when people leave and rejoin
            if (!CharacterDatabase.instance.database.ContainsKey((ulong)i)) continue;

            CharacterDatabase.instance.LoadServerCharDataClientRpc((ulong)i, CharacterDatabase.instance.database[(ulong)i]);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        CharacterPersistenceManager.instance.SaveCharacter();
        base.OnNetworkDespawn();
    }
}
