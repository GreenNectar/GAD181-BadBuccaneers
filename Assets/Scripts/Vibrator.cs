using Rewired;
using Rewired.ControllerExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vibrator : Singleton<Vibrator>
{
    public void Vibrate(int playerNumber, int motor, float vibration)
    {
        Player p = PlayerManager.GetPlayer(playerNumber);
        p.SetVibration(motor, vibration);
    }

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

    //public void SineVibrate(int playerNumber, float time)
    //{
    //    StartCoroutine(SineVibrateRoutine(playerNumber, time));
    //} 

    //private IEnumerator SineVibrateRoutine(int playerNumber, float time)
    //{
    //    Player p = ReInput.players.GetPlayer(PlayerManager.GetPlayerId(playerNumber));
    //    float startingTime = time;
    //    while(time > 0f)
    //    {
    //        startingTime -= Time.deltaTime;
    //        p.SetVibration(0, (Mathf.Sin((time / startingTime) * Mathf.PI) + 1f) / 2f);
    //        yield return null;
    //    }
    //}
}
