using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] public TextMeshProUGUI joinCode;
    [SerializeField] public TMP_InputField inputCode;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() => {
            // NetworkManager.Singleton.StartServer();
            DisableUI();
        });
        hostBtn.onClick.AddListener(() => {
            // NetworkManager.Singleton.StartHost();
            DisableUI();
            TestRelay testRelay = GameObject.FindGameObjectWithTag("TestRelay").GetComponent<TestRelay>();
            testRelay.CreateRelay();
        });
        clientBtn.onClick.AddListener(() => {
            // NetworkManager.Singleton.StartClient();
            DisableUI();
            TestRelay testRelay = GameObject.FindGameObjectWithTag("TestRelay").GetComponent<TestRelay>();
            testRelay.JoinRelay(inputCode.text);
        });
    }

    private void DisableUI()
    {
        serverBtn.gameObject.SetActive(false);
        hostBtn.gameObject.SetActive(false);
        clientBtn.gameObject.SetActive(false);
        inputCode.gameObject.SetActive(false);
    }

}
