using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BadBuccaneers/MicroGame Data", fileName = "MicroGameData")]
public class MicroGame : ScriptableObject
{
    public string title;
    [TextArea]
    public string description;
    [TextArea]
    public string developers;

    public int minimumRequiredPlayers = 1;

    [Scene]
    public string microGameScene;

    public Control[] controls;

    public ScoreType scoreType = ScoreType.Points;
    public ScoreLayout scoreLayout = ScoreLayout.Corners;

    [Serializable]
    public class Control
    {
        public string description;
        public string buttons;
    }

    public enum ScoreType { Points, Percentage, Elimination, Race };
    public enum ScoreLayout { Corners, Bottom, Left };
}
