using Rewired;
using Rewired.ControllerExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vibrator : Singleton<Vibrator>
{
    List<Vibration> vibrations = new List<Vibration>();

    private class Vibration
    {
        public float vibration;
        public int player;
        public int motor;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // Get the totals
        float[] playerLeftVibrations = new float[4];
        float[] playerRightVibrations = new float[4];
        foreach (var vibration in vibrations)
        {
            playerLeftVibrations[vibration.player] += vibration.vibration * (1 - vibration.motor);
            playerRightVibrations[vibration.player] += vibration.vibration * vibration.motor;
        }

        // Set the vibrations
        for (int i = 0; i < PlayerManager.PlayerCountScaled; i++)
        {
            Player p = PlayerManager.GetPlayer(i);

            p.SetVibration(0, Mathf.Clamp(playerLeftVibrations[i], 0f, 1f));
            p.SetVibration(1, Mathf.Clamp(playerRightVibrations[i], 0f, 1f));
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        StopVibrations();
    }

    /// <summary>
    /// Set the vibration of the controller
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="motor"></param>
    /// <param name="vibration"></param>
    public void Vibrate(int playerNumber, int motor, float vibration)
    {
        if (PlayerManager.HasPlayer(playerNumber))
        {
            Player p = PlayerManager.GetPlayer(playerNumber);
            p.SetVibration(motor, vibration);
        }
        else if (ReInput.players.Players.First(p => p.id == playerNumber) != null)//.playerCount <= playerNumber)//.AllPlayers.First(p => p.id == playerNumber) != null)
        {
            Player p = ReInput.players.GetPlayer(playerNumber);
            p.SetVibration(motor, vibration);
        }
    }

    /// <summary>
    /// A pulse vibration. It starts at 0 and sin waves to 1 and back to 0
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="motor"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public void PulseVibration(int playerNumber, int motor, float time, float strength = 1f)
    {
        StartCoroutine(PulseVibrationRoutine(playerNumber, motor, time, strength));
    }


    private IEnumerator PulseVibrationRoutine(int playerNumber, int motor, float time, float strength = 1f)
    {
        Vibration vibe = new Vibration { player = playerNumber, vibration = strength, motor = motor };
        vibrations.Add(vibe);
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0f, time);
            vibe.vibration = Mathf.Sin(t / time * Mathf.PI) * strength;
            
            yield return null;
        }
        vibrations.Remove(vibe);
    }

    /// <summary>
    /// An impact vibration. It starts at full and trails off
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="motor"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public void ImpactVbration(int playerNumber, int motor, float time, float strength = 1f)
    {
        StartCoroutine(ImpactVbrationRoutine(playerNumber, motor, time, strength));
    }

    private IEnumerator ImpactVbrationRoutine(int playerNumber, int motor, float time, float strength = 1f)
    {
        Vibration vibe = new Vibration { player = playerNumber, vibration = strength, motor = motor };
        vibrations.Add(vibe);
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0f, time);
            vibe.vibration = Mathf.Sin((1f - (t / time)) * (Mathf.PI / 2f)) * strength;

            yield return null;
        }
        vibrations.Remove(vibe);
    }

    /// <summary>
    /// Stops vibrations on all connected players
    /// </summary>
    private void StopVibrations()
    {
        vibrations.Clear();
        for (int i = 0; i < PlayerManager.PlayerCount; i++)
        {
            Vibrate(i, 0, 0f);
            Vibrate(i, 1, 0f);
        }
    }
}
