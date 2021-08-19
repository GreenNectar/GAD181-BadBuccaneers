using Rewired;
using Rewired.ControllerExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vibrator : Singleton<Vibrator>
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
    public void PulseVibration(int playerNumber, int motor, float time)
    {
        StartCoroutine(PulseVibrationRoutine(playerNumber, motor, time));
    }


    private IEnumerator PulseVibrationRoutine(int playerNumber, int motor, float time)
    {
        Player p = PlayerManager.GetPlayer(playerNumber);
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0f, time);
            p.SetVibration(motor, Mathf.Sin(t / time * Mathf.PI));
            yield return null;
        }
    }

    /// <summary>
    /// An impact vibration. It starts at full and trails off
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="motor"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public void ImpactVbration(int playerNumber, int motor, float time)
    {
        StartCoroutine(ImpactVbrationRoutine(playerNumber, motor, time));
    }

    private IEnumerator ImpactVbrationRoutine(int playerNumber, int motor, float time)
    {
        Player p = PlayerManager.GetPlayer(playerNumber);
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0f, time);
            float vibration = Mathf.Sin((1f - (t / time)) * (Mathf.PI / 2f));
            p.SetVibration(motor, vibration);
            yield return null;
        }
    }

    /// <summary>
    /// Stops vibrations on all connected players
    /// </summary>
    private void StopVibrations()
    {
        for (int i = 0; i < PlayerManager.PlayerCount; i++)
        {
            Vibrate(i, 0, 0f);
            Vibrate(i, 1, 0f);
        }
    }
}
