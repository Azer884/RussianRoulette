using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LeaveGameButton : MonoBehaviour
{
    private Button leaveButton;
    public PlayerSpawner playerSpawner;

    void Start()
    {
        leaveButton = GetComponent<Button>();
        leaveButton.onClick.AddListener(OnLeaveGameClicked);
    }

    private void OnLeaveGameClicked()
    {
        if (NetworkManager.Singleton != null)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.Shutdown();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }

        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
        
        GameObject playerSpawnerObj = GameObject.Find("PlayerSpawner");
        if (playerSpawnerObj != null)
        {
            PlayerSpawner playerSpawner = playerSpawnerObj.GetComponent<PlayerSpawner>();
            if (playerSpawner != null)
            {
                playerSpawner.isStarted = false;
            }
        }

    }
}