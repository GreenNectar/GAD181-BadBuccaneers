using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // Store the players (key is the player number, value is the ReInput id of the controller/player)
    private static Dictionary<int, int> players = new Dictionary<int, int>();

    // Just so we can check the player count
    public static int PlayerCount
    {
        get
        {
            return players.Count;
        }
    }

    /// <summary>
    /// Shifts the players so the player numbers start from 0 and increment by 1 in order
    /// </summary>
    public static void ShiftPlayers()
    {
        // We don't need to shift players if we have the maximum amount of players
        if (players.Count < 4)
        {
            // This is the shifted dictionary, we will be putting the shifted players into here
            Dictionary<int, int> shiftedPlayers = new Dictionary<int, int>();

            // This is just so we can increment inside the foreach loop
            int index = 0;

            // Due to the players not being added into the dictionary in order (some might leave and join randomly) we need to order them
            foreach (var player in players.OrderBy(p => p.Key))
            {
                shiftedPlayers.Add(index, player.Value);
                index++;
            }

            // Set the player registery to the new shifted registery
            players = shiftedPlayers;
        }

        //// This was for debugging
        //foreach (var player in players)
        //{
        //    Debug.Log($"Player {player.Key} has id of {player.Value}");
        //}
    }

    /// <summary>
    /// Add a player to the registery
    /// </summary>
    /// <param name="playerNumber">This is the position of the player (player 1, player 2 etc)</param>
    /// <param name="playerId">This is the id of the player in ReWired</param>
    public static void AddPlayer(int playerNumber, int playerId)
    {
        // We can't have more than four players
        if (playerNumber > 4)
        {
            throw new System.Exception("Cannot have more than four players");
        }

        if (!players.ContainsKey(playerNumber))
        {
            // Add the player's id to the register
            players.Add(playerNumber, playerId);
        }
        else
        {
            // Change the player's id in the register
            players.Remove(playerNumber);
            players.Add(playerNumber, playerId);
        }
    }

    /// <summary>
    /// Remove the player
    /// </summary>
    /// <param name="playerNumber"></param>
    public static void RemovePlayer(int playerNumber)
    {
        players.Remove(playerNumber);
    }

    /// <summary>
    /// Returns the player's id associated with a player
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static int GetPlayerId(int playerNumber)
    {
        // Set the default value to the player number
        int value = playerNumber;

        // Get the player's id
        if (players.ContainsKey(playerNumber))
        {
            players.TryGetValue(playerNumber, out value);
        }

        // Return that poo
        return value;
    }

    /// <summary>
    /// Returns true if the player is in the registery
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool HasPlayer(int playerNumber)
    {
        return players.ContainsKey(playerNumber);
    }

    /// <summary>
    /// Returns true if the player's id is in the registery
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public static bool HasPlayerId(int playerId)
    {
        return players.ContainsValue(playerId);
    }

    /// <summary>
    /// Returns the number of the player with the given id
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public static int GetPlayerNumber(int playerId)
    {
        foreach (var kvPair in players)
        {
            if (kvPair.Value == playerId)
            {
                return kvPair.Key;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the first free player position (if player 1 leaves, then 1 will be the first free position)
    /// </summary>
    /// <returns></returns>
    public static int FirstFreePosition()
    {
        List<int> freePositions = new List<int>{ 0, 1, 2, 3 };

        for (int i = 0; i < 4; i++)
        {
            foreach (var player in players)
            {
                freePositions.Remove(player.Key);
            }
        }

        if (freePositions.Count > 0)
        {
            return freePositions[0];
        }

        return -1;
    }
}