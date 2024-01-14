using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacterPersistence
{
    private string currentClass; // this could be a future bug, this is empty at startup

    public void SetClass(string charClass)
    {
        currentClass = charClass;
    }

    public void LoadCharacter(CharacterData data)
    {
        this.currentClass = data.charClass;
    }

    public void SaveCharacter(ref CharacterData data)
    {
        data.charClass = this.currentClass;
    }
}
