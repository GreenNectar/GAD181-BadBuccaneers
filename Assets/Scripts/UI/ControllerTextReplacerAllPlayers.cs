using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTextReplacerAllPlayers : ControllerTextReplacer
{
    protected override void UpdateText()
    {
        List<string> sections = new List<string>();
        string temp = "";

        foreach (var c in startingText)
        {
            if (c == '<')
            {
                sections.Add(temp);
                temp = "";
            }

            temp += c;

            if (c == '>')
            {
                sections.Add(temp);
                temp = "";
            }
        }

        sections.Add(temp);

        string final = "";
        List<string> alreadyDone = new List<string>();

        foreach (var s in sections)
        {
            string output = "";
            if (s.StartsWith("<") && s.EndsWith(">") && s.Contains("Controller-"))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (PlayerManager.HasPlayer(i))
                    {
                        string replacedText = ReplaceText(s, i);
                        if (!alreadyDone.Contains(replacedText))
                        {
                            output += replacedText;
                            alreadyDone.Add(replacedText);
                        }
                    }
                    }
                }
            else
            {
                output = s;
            }

            final += output;
        }

        text.text = ReplaceText(final, 0);
    }
}