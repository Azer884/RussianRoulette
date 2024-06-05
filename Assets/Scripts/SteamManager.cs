using UnityEngine;
using Steamworks;
using TMPro;
using Steamworks.Data;
using Unity.Netcode;
using Netcode.Transports.Facepunch;
using UnityEngine.SceneManagement;

public class SteamManager : MonoBehaviour
{
    [SerializeField]private TMP_InputField lobbyIDInputField;
    [SerializeField]private TextMeshProUGUI lobbyID;
    [SerializeField]private GameObject MainMenu;
    [SerializeField]private GameObject InLobbyMenu;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitForHost;

    private void OnEnable() {
        SteamMatchmaking.OnLobbyCreated += LobbyCreated;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += LobbyJoinRequested;
    }

    private async void LobbyJoinRequested(Lobby lobby, SteamId id)
    {
        await lobby.Join();
    }

    private void LobbyEntered(Lobby lobby)
    {
        LobbySaver.instance.currentLobby = lobby;
        lobbyID.text = lobby.Id.ToString();
        CheckUI();

        if(NetworkManager.Singleton.IsHost)return;
        NetworkManager.Singleton.gameObject.GetComponent<FacepunchTransport>().targetSteamId = lobby.Owner.Id;
        NetworkManager.Singleton.StartClient();
    }

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            NetworkManager.Singleton.StartHost();
        }
    }

    private void OnDisable() {
        SteamMatchmaking.OnLobbyCreated -= LobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= LobbyJoinRequested;
    }
    public async void HostLobby()
    {
        await SteamMatchmaking.CreateLobbyAsync(6);
    }
    public async void JoinLobbyWithID()
    {
        ulong ID;
        if(!ulong.TryParse(lobbyIDInputField.text, out ID))
            return;

        Lobby[] lobbies = await SteamMatchmaking.LobbyList.WithSlotsAvailable(1).RequestAsync();

        foreach (Lobby lobby in lobbies)
        {
            if (lobby.Id == ID)
            {
                await lobby.Join();
                return;
            }
        }
    }

    public void CopyID()
    {
        TextEditor textEditor = new TextEditor
        {
            text = lobbyID.text
        };
        textEditor.SelectAll();
        textEditor.Copy();
    }
    public void LeaveLobby()
    {
        LobbySaver.instance.currentLobby?.Leave();
        LobbySaver.instance.currentLobby = null;
        NetworkManager.Singleton.Shutdown();
        CheckUI();
    }

    private void CheckUI()
    {
        if (LobbySaver.instance.currentLobby == null)
        {
            MainMenu.SetActive(true);
            InLobbyMenu.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(false);
            InLobbyMenu.SetActive(true);
            startButton.SetActive(NetworkManager.Singleton.IsHost);
            waitForHost.SetActive(!NetworkManager.Singleton.IsHost);
        }
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
