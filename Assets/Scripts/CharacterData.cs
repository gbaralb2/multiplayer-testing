using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string charClass;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to Load

    public CharacterData()
    {
        this.charClass = "pawn";
    }
}
