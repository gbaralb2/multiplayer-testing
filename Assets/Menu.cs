using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button saveButton, loadButton;

    void Awake()
    {
        saveButton.onClick.AddListener(() => {
            CharacterPersistenceManager.instance.SaveCharacter();
        });
        loadButton.onClick.AddListener(() => {
            CharacterPersistenceManager.instance.LoadCharacter();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
