using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System;

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
        Debug.Log(dataHandler);

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

        Debug.Log("Current Class: " + currentClass + ". Class in data: " + data.charClass);
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
    private void UploadDataServerRpc(ulong clientId, NetworkCharacterData data)
    {
        CharacterDatabase.instance.UploadData(clientId, data);
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
