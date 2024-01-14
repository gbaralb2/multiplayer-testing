using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterPersistence
{
    void LoadCharacter(CharacterData data);

    void SaveCharacter(ref CharacterData data);
}
