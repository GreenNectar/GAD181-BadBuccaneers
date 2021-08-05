using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
[Serializable]
public class MicroGame
=======
<<<<<<< Updated upstream
[CreateAssetMenu]
=======
[CreateAssetMenu(menuName = "BadBuccaneers/MicroGame Data", fileName = "MicroGameData")]
>>>>>>> Stashed changes
public class MicroGame : ScriptableObject
>>>>>>> Stashed changes
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
