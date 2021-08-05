using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ControllerTextReplacer : MonoBehaviour
{
    protected TextMeshProUGUI text;
    protected string startingText;

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

    protected virtual void UpdateText()
    {
        text.text = ReplaceText(startingText, playerNumber);
    }

    protected string ReplaceText(string input, int playerNumber)
    {
        string toReplace = PlayerManager.GetControllerType(playerNumber);
        if (toReplace == "") toReplace = PlayerManager.DefaultControllerType;
        return input.Replace("Controller", toReplace);
    }

    private void Initialise()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
            SetStarting(text.text);
        }
    }
    
    public void SetStarting(string text)
    {
        startingText = text;
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
