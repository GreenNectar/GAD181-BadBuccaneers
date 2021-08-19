using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bad Buccaneers/SFX Table", fileName = "SFXTable")]
public class SFXTable : ScriptableObject
{
    [SerializeField]
    private List<EventStringPair> events = new List<EventStringPair>();

    [Serializable]
    private struct EventStringPair
    {
        public string name;
        [EventRef]
        public string eventName;
    }

    private static SFXTable table;

    public static string Get(string name)
    {
        if (table == null) table = Resources.Load<SFXTable>("Audio/SFXTable");
        return table.events.Find(e => e.name == name).eventName;
    }
}
