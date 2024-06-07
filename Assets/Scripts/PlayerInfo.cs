using System;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public ulong PlayerId;
    public ShootingSys ShootingSystem;

    public PlayerInfo(ulong playerId, ShootingSys shootingSystem)
    {
        PlayerId = playerId;
        ShootingSystem = shootingSystem;
    }
}