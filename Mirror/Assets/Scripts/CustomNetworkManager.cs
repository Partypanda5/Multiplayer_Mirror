using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    [Header("Player Prefabs")]
    public List<GameObject> playerPrefabs = new List<GameObject>(); // Drag all your player prefabs here

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Safety check
        if (playerPrefabs.Count == 0)
        {
            Debug.LogError("No players assigned!");
            return;
        }

       // int index = Random.Range(0, playerPrefabs.Count); //for assigning randomly

        // Example logic: assign based on player index
        int index = numPlayers % playerPrefabs.Count;

        GameObject selectedPrefab = playerPrefabs[index];

        GameObject player = Instantiate(selectedPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}