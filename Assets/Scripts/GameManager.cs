using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> BulletPos = new NetworkVariable<int>();
    public NetworkVariable<int> currentPlayer = new NetworkVariable<int>();
    [SerializeField] private List<ShootingSys> players = new List<ShootingSys>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        BulletPos.Value = Random.Range(0, 6);    
        currentPlayer.Value = 0;

        
    }

    /*private void Update() {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Number of player objects found: " + playerObjects.Length);

        foreach (var playerObject in playerObjects)
        {
            var playerScript = playerObject.GetComponent<ShootingSys>();
            if (playerScript != null)
            {
                players.Add(playerScript);
                Debug.Log("Added player: " + playerObject.name);
            }
            else
            {
                Debug.LogWarning("No ShootingSys component found on: " + playerObject.name);
            }
        }

        Debug.Log("Total players in list: " + players.Count);
    }*/
    private void OnEnable()
    {
        BulletPos.OnValueChanged += ChangeBulletPos;
        currentPlayer.OnValueChanged += SwitchPlayer;
    }

    private void OnDisable()
    {
        BulletPos.OnValueChanged -= ChangeBulletPos;
        currentPlayer.OnValueChanged -= SwitchPlayer;
    }

    private void ChangeBulletPos(int previousValue, int newValue)
    {
        if (players.Count == 0)
        {
            Debug.LogWarning("No players available to switch.");
            return;
        }

        currentPlayer.Value++;
        currentPlayer.Value %= players.Count;
    }

    private void SwitchPlayer(int previousValue, int newValue)
    {
        if (players.Count == 0)
        {
            Debug.LogWarning("No players available to switch.");
            return;
        }

        if (newValue < 0 || newValue >= players.Count)
        {
            Debug.LogError("Invalid player index: " + newValue);
            return;
        }

        players[newValue].enabled = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (newValue != i)
            {
                players[i].enabled = false;
            }
        }
    }
}
