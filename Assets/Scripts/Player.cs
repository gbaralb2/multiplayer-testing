using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour, ICharacterPersistence
{
    [SerializeField] private EventManager eventManager;

    private string currentClass; // this could be a future bug, this is empty at startup

    void Start()
    {
        if (!IsOwner) return;
        CharacterPersistenceManager.instance.LoadCharacter();
    }

    public void SetClass(string charClass)
    {
        if (!IsOwner) return;
        currentClass = charClass;
    }

    public void LoadCharacter(CharacterData data)
    {
        if (!IsOwner) return;

        this.currentClass = data.charClass;

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

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        CharacterPersistenceManager.instance.SaveCharacter();
        base.OnNetworkDespawn();
    }
}
