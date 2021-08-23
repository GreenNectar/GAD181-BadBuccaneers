using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEventTable", menuName = "Bad Buccaneers/Character FMOD Event Table")]
public class CharacterEventTable : ScriptableObject
{
    [SerializeField]
    private List<CharacterEventPair> characterFMODEvents = new List<CharacterEventPair>();
    //private Dictionary<string, CharacterFMODEvents> characterFMODEvents = new Dictionary<string, CharacterFMODEvents>();

    public CharacterFMODEvents GetCharacterEvents(string characterName)
    {
        if (characterFMODEvents.Where(c => c.characterName == characterName).Count() > 0)
        {
            return characterFMODEvents.First(c => c.characterName == characterName).characterFMODEvents;
        }
        else
        {
            return characterFMODEvents[0].characterFMODEvents;
        }

        throw new Exception($"No {typeof(CharacterFMODEvents).Name} associated with {characterName}. Please add it to the {typeof(CharacterEventTable).Name} scriptable object in the resources folder");
    }

    [Serializable]
    public struct CharacterEventPair
    {
        public string characterName;
        public CharacterFMODEvents characterFMODEvents;
    }
}