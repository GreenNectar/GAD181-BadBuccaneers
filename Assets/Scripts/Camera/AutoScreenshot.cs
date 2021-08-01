using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScreenshot : Screenshot
{
    [SerializeField]
    private GameObject[] objects;

    [Button("Auto Screenshots")]
    public void TakeScreenshots()
    {
        foreach (var go in objects)
        {
            go.SetActive(false);
        }

        for (int i = 0; i < objects.Length - 1; i++)
        {
            if (i > 0)
            {
                objects[i - 1].SetActive(false);
            }

            objects[i].SetActive(true);

            string n = screenshotName != "" ? screenshotName + "-" : "";

            TakeScreenshot(n + objects[i].name);
        }
    }
}
