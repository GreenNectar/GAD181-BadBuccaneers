using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ControllerTextReplacer : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string startingText;

    [SerializeField]
    private int playerNumber = 0;

    void Start()
    {
        Initialise();
        UpdateText();
    }

    void OnEnable()
    {
        Initialise();
        PlayerManager.onUpdateControllerType += UpdateText;
        UpdateText();
    }

    void OnDisable()
    {
        PlayerManager.onUpdateControllerType -= UpdateText;
    }

    private void UpdateText()
    {
        string toReplace = PlayerManager.GetControllerType(playerNumber);
        text.text = startingText.Replace("Controller", toReplace);
    }

    private void Initialise()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
            startingText = text.text;
        }
    }

    //public override TMP_TextInfo GetTextInfo(string text)
    //{
    //    string toReplace = PlayerManager.GetControllerType(0);
    //    TMP_TextInfo info = base.GetTextInfo(text);
    //    info.textComponent.text = info.textComponent.text.Replace("Controller", $"{toReplace}");
    //    Debug.Log(info.textComponent.text);
    //    return info;
    //}

}
