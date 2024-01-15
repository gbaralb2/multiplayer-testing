using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using TMPro;

public class CharacterPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private CharacterData characterData;
    private List<ICharacterPersistence> characterPersistenceObjects;
    private FileDataHandler dataHandler;

    public static CharacterPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Character Persistence Manager in the scene.");  
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        // this.characterPersistenceObjects = FindAllCharacterPersistenceObjects();
        // LoadCharacter();
    }

    public void NewCharacter()
    {
        this.characterData = new CharacterData();
    }

    public void LoadCharacter()
    {
        this.characterPersistenceObjects = FindAllCharacterPersistenceObjects();

        // TODO - Load any saved data from a file using the data handler
        this.characterData = dataHandler.Load();

        // if no data can be loaded, initialize to a new character
        if (this.characterData == null)
        {
            Debug.Log("No character was found. Initializing character data to defauls");
            NewCharacter();
        }
        // TODO - push the loaded data to all other scripts that need it
        foreach (ICharacterPersistence characterPersistenceObj in characterPersistenceObjects) 
        {
            characterPersistenceObj.LoadCharacter(characterData);
        }

        Debug.Log("Loaded class: " + characterData.charClass);
    }

    public void SaveCharacter()
    {
        // TODO - pass the data to other scripts to they can update it
        foreach (ICharacterPersistence characterPersistenceObj in characterPersistenceObjects)
        {
            characterPersistenceObj.SaveCharacter(ref characterData);

            Debug.Log("found one");
        }

        Debug.Log("Saved class: " + characterData.charClass);

        // TODO - save that data to a file using the data handler
        dataHandler.Save(characterData);
    }

    // private void OnApplicationQuit()
    // {
    //     SaveCharacter();
    // }

    private List<ICharacterPersistence> FindAllCharacterPersistenceObjects()
    {
        IEnumerable<ICharacterPersistence> characterPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ICharacterPersistence>();

        return new List<ICharacterPersistence>(characterPersistenceObjects);
    }
}
