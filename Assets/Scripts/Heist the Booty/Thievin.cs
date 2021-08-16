using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thievin : MonoBehaviour
{
    public Text scoreText;
    public float score = 0f;
    public float pointsPerSec = 1f;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = (int)score + "Score";
        score += pointsPerSec * Time.deltaTime;

    }
}