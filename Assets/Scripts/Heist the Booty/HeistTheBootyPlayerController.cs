using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeistTheBootyPlayerController : MicroGamePlayerController 
{
    protected override void Start()
    {
        base.Start();
    }

    public void Loot()
    {

    }

    //! Put this in the HeisttheBootyManager
    Animator skeletonAnimator;

    bool isLooking = false; // This is to keep track of whether the skelly boy is watching ye
    public void FinishLooking() // Play this from the animator when skelly boy sits back down/looks around
    {
        isLooking = false;
    }

    // Call this from the HeisttheBootyPlayerController when player wants to loot, this will handle the scoring and punishments
    public void Loot(int playerNumber)
    {
        if (isLooking)
        {
            // Punish player
            //TODO Play shoot animation on skeleton
            // (the ending of this animation should go back into the sitting animation after the shooting animation finishes, this will also make that
            // mechanic wanted where he look for a certain amount of time after he shoots, rather than when he first shot)
            // You might want to use the script below for when he is already in his shooting animation
            // This means you don't have to have the animation go back into itself in the animator
            if (skeletonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {
                skeletonAnimator.Play("Shooting", 0, 0f);

            }
            //TODO Play hit animation on player
            ScoreManager.Instance.AddScoreToPlayer(playerNumber, -5);
        }
        else
        {
            // Give player points
            ScoreManager.Instance.AddScoreToPlayer(playerNumber, 5);
        }
    }

    // Don't worry about this for now as I will do it on the final pass, unless you are confident with fx
    public void Shoot() // Will be done on the first frame of the shoot animation of the skelly boy (needs to be first frame as otherwise when he repeats it too quickly it might not get called)
    {
        // Play FX
    }

    private IEnumerator LookSequence()
    {
        while (true)
        {
            // Wait for a random amount of time
            float timeUntilNextLook = Random.Range(4f, 8f);
            yield return new WaitForSeconds(timeUntilNextLook);

            //TODO Start look animation
            isLooking = true;

            // Wait for the skelly boy to stop looking, then continue to next round
            yield return new WaitUntil(() => isLooking == false);
        }
    }

}