using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Steamworks;
using Steamworks.Data;

public class ChatManager : MonoBehaviour
{
    [SerializeField]private TMP_InputField MessageInpField;
    [SerializeField]private TextMeshProUGUI MessageTemplate;
    [SerializeField]private GameObject MessagesContainer;


    private void Start() {
        MessageTemplate.text = "";
    }
    void OnEnable()
    {
        SteamMatchmaking.OnChatMessage += ChatSent;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += LobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += LobbyMemberLeft;
    }
    void OnDisable()
    {
        SteamMatchmaking.OnChatMessage -= ChatSent;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= LobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= LobbyMemberLeft;
    }

    private void LobbyMemberLeft(Lobby lobby, Friend friend) => AddMessageToBox(friend.Name + " Left the lobby" + "\n");

    private void LobbyMemberJoined(Lobby lobby, Friend friend) => AddMessageToBox(friend.Name + " Joined the lobby" + "\n");

    private void LobbyEntered(Lobby lobby) => AddMessageToBox("You entered the lobby ");

    private void ChatSent(Lobby lobby, Friend friend, string msg)
    {
        AddMessageToBox(friend.Name + ": " + msg + "\n");
    }

    private void AddMessageToBox(string msg)
    {
        GameObject message = Instantiate(MessageTemplate.gameObject, MessagesContainer.transform);
        message.GetComponent<TextMeshProUGUI>().text = msg;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleChatBox();
        }
    }

    private void ToggleChatBox()
    {
        if (MessageInpField.gameObject.activeSelf)
        {
            if (!string.IsNullOrEmpty(MessageInpField.text))
            {
                LobbySaver.instance.currentLobby?.SendChatString(MessageInpField.text);
                MessageInpField.text = "";
            }
            MessageInpField.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            MessageInpField.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(MessageInpField.gameObject);
        }

    }
}
