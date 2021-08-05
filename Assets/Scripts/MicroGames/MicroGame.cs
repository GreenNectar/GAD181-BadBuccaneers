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
    public int minimumRequiredPlayers = 1;
    [Scene]
    public string microGameScene;
    public Control[] controls;

    [Serializable]
    public class Control
    {
        public string description;
        public string buttons;
    }
}
