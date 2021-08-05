using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkingScript : MicroGamePlayerController
{
    // Scoring components
    public int points = 0;

    public Text scoreText;

    // Drunk managing components
    public float inebriation = 0f;
    public float currentInebriation = 0f;
    float inebriationMax = 100;
    float inebriationMin = 0;
    public float drinkPotency = 5f;
    public float soberingSpeed = 5f;
    public float drunkUpdateDelay = 0.5f;
    private bool fainted;
    public float inebriationLerpSpeed = 1f;
    public float inebriationSmoothing = 1f;

    // Slider managing components
    public Slider drunkoMetre;


    public Animator animator;

    protected override void Start()
    {
        base.Start();
        scoreText.text = points.ToString() + " POINTS";
    }


    // Increase points & start Swig Coroutine
    void Update()
    {
        if (fainted) return;

        // Input to start drinking
        if (player.GetButtonDown("Fire") || Input.GetMouseButtonDown(0))
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
            fainted = true;
            animator.ResetTrigger("Swig");
            animator.SetTrigger("Faint");
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
        points += 1;
        scoreText.text = points.ToString() + " POINTS";
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
        Debug.Log("Fainted");
    }
    
}
