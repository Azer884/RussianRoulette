using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> BulletPos = new NetworkVariable<int>();
    public NetworkVariable<int> currentPos = new NetworkVariable<int>(0);
    public NetworkVariable<int> currentPlayer = new NetworkVariable<int>();
    [SerializeField] private List<PlayerInfo> players = new();
    private bool isFirstRound = true;

    private void Update() 
    {
        if (IsServer)
        UpdatePlayerInfoList();

        if (isFirstRound)
        {
            currentPlayer.Value = Random.Range(0, players.Count);

            isFirstRound = false;
        }
    }

    private void OnEnable()
    {
        currentPos.OnValueChanged += ChangeBulletPos;
        currentPlayer.OnValueChanged += SwitchPlayer;
    }

    private void OnDisable()
    {
        currentPos.OnValueChanged -= ChangeBulletPos;
        currentPlayer.OnValueChanged -= SwitchPlayer;
    }

    private void ChangeBulletPos(int previousValue, int newValue)
    {
        if (players.Count == 0) return;

        currentPlayer.Value++;
        currentPlayer.Value %= players.Count;
    }

    private void SwitchPlayer(int previousValue, int newValue)
    {
        if (players.Count == 0 || newValue < 0 || newValue >= players.Count)  return;

        players[newValue].ShootingSystem.enabled = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (newValue != i)
            {
                players[i].ShootingSystem.enabled = false;
            }
        }
    }

    private void UpdatePlayerInfoList()
    {
        players.Clear();

        // Get all connected clients
        var connectedClients = NetworkManager.Singleton.ConnectedClientsList;

        foreach (var client in connectedClients)
        {
            // Find the player object associated with the client
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(client.ClientId, out var networkClient))
            {
                GameObject playerObject = networkClient.PlayerObject.gameObject;

                // Get the ShootingSys component from the player object
                ShootingSys shootingSys = playerObject.GetComponent<ShootingSys>();

                shootingSys.bulletPos = BulletPos.Value;

                if (shootingSys != null)
                {
                    // Add the player ID and ShootingSys to the list
                    players.Add(new PlayerInfo(client.ClientId, shootingSys));
                }
            }
        }
    }
}
