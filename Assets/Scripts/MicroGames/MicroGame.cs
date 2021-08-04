using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MicroGame
{
    public string name;
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
