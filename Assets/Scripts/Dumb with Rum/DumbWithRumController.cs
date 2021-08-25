using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DumbWithRumController : MicroGamePlayerController
{
    // Drunk managing components
    [SerializeField] private float inebriation = 0f;
    [SerializeField] private float currentInebriation = 0f;
    [SerializeField] private float drinkPotency = 5f;
    [SerializeField] private float soberingSpeed = 5f;
    [SerializeField] private float drunkUpdateDelay = 0.5f;
    [SerializeField] private float inebriationLerpSpeed = 1f;
    [SerializeField] private float inebriationSmoothing = 1f;
    [SerializeField, EventRef] private string drinkEvent;
    [SerializeField, EventRef] private string bodyFallEvent;
    float inebriationMax = 100;
    float inebriationMin = 0;
    private bool fainted;

    // Slider managing components
    public Slider drunkoMetre;


    public Animator animator;

    protected override void Start()
    {
        base.Start();

        PlayerManager.GetPlayerFMODEvent(PlayerNumber).Happy(gameObject);
        animator.SetTrigger("Cheer");
    }


    // Increase points & start Swig Coroutine
    void Update()
    {
        if (fainted) return;

        // Input to start drinking
        if (player.GetButtonDown("Fire"))
        {
            animator.SetTrigger("Swig");
        }

        // Sober up. IF  100 > currentInebriation > 0 & hasn't risen in increased in last second, decrease current inebriation by 5 per second
        if (100 > inebriation && inebriation > 0)
        {
            inebriation -= soberingSpeed * Time.deltaTime;
            // Clamps the inebriation range
            inebriation = Mathf.Clamp(inebriation, inebriationMin, inebriationMax);
        }

        if (currentInebriation >= inebriationMax)
        {
            Faint();
        }

        float offset = Mathf.Pow(Time.deltaTime * inebriationLerpSpeed, inebriationSmoothing);
        if (Mathf.Abs(currentInebriation - inebriation) > offset)
        {
            currentInebriation += currentInebriation > inebriation ? -offset : offset;
        }
        else
        {
            currentInebriation = inebriation;
        }

        // Ensures the slider updates
        drunkoMetre.SetValueWithoutNotify(currentInebriation); 
    }

    
    public void Swig()
    {
        ScoreManager.Instance.AddScoreToPlayer(PlayerNumber, 1);
        RuntimeManager.PlayOneShotAttached(drinkEvent, gameObject);
        StartCoroutine(UpdateDrunkoMetre());
    }

    // Increases the drunk value after a brief delay
    private IEnumerator UpdateDrunkoMetre()
    {
        yield return new WaitForSeconds(drunkUpdateDelay);
        inebriation += drinkPotency;
    }

    // Disqualifies a drunken idiot
    public void Faint()
    {
        fainted = true;
        // Just do some animation
        animator.ResetTrigger("Swig");
        animator.SetTrigger("Faint");

        // If all players are passed out, end the game
        ScoreManager.Instance.EndPlayer(PlayerNumber);
        if (ScoreManager.Instance.AllPlayersFinished)
            GameManager.EndGameStatic(3f);

        // I - Jarrad - disagree with this mechanics
        // Remove half of the player's points
        ScoreManager.Instance.SetPlayerScore(PlayerNumber, ScoreManager.Instance.GetScore(PlayerNumber) / 2);
        
        // Play death sound
        PlayerManager.GetPlayerFMODEvent(PlayerNumber).Death(gameObject);

        // Play drop sound
        Invoke("PlayBodyDropSound", 2.2f);
    }
    
    private void PlayBodyDropSound()
    {
        RuntimeManager.PlayOneShot(bodyFallEvent, transform.position);
    }
}
