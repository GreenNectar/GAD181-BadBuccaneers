using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace R4R.CustomDebug
{
    public class Console : MonoBehaviour
    {
        public bool printStackTrace = false;

        string log;
        bool isOpen;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        private void Log(string logString, string stackTrace, LogType type)
        {
            log += logString + "\n";

            if (printStackTrace)
            {
                log += stackTrace + "\n";
            }

            if (log.Length > 1000)
            {
                log.Substring(log.Length - 1000, 1000);
            }

            CancelInvoke("HideConsole");
            isOpen = true;
            Invoke("HideConsole", 5f);
        }

        private void HideConsole()
        {
            isOpen = false;
        }

        private void OnGUI()
        {
            if (isOpen)
            {
                GUI.TextArea(new Rect(0, 0, Screen.width, 300), log,
                    new GUIStyle {
                        alignment = TextAnchor.LowerLeft,
                        normal = new GUIStyleState { textColor = Color.grey }
                    });
            }
        }
    }
}