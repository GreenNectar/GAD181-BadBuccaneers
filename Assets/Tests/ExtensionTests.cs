using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class ExtensionTests
{
    [UnityTest]
    public IEnumerator OrdinalTest()
    {
        string[] referenceOrdinals = {
            "1st",
            "2nd",
            "3rd",
            "4th",
            "5th",
            "6th",
            "7th",
            "8th",
            "9th",
            "10th",
            "11th",
            "12th",
            "13th",
            "14th",
            "15th",
            "16th",
            "17th",
            "18th",
            "19th",
            "20th",
            "21st",
            "22nd",
            "23rd",
            "24th",
            "25th",
            "26th",
            "27th",
            "28th",
            "29th",
            "30th",
            "31st",
            "32nd",
            "33rd",
            "34th",
        };

        string[] createdOrdinals = new string[34];

        for (int i = 0; i < 34; i++)
        {
            createdOrdinals[i] = (i + 1).Ordinal();
        }

        Assert.AreEqual(referenceOrdinals, createdOrdinals);

        yield return null;
    }
}
