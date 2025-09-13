using UnityEngine;
using TMPro;
using System.Text;
using System;

public class UnlockedMessagesUI : MonoBehaviour
{
    public TextMeshProUGUI messagesText;
    void Awake()
    {
        var unlocked = MessageManager.Instance.GetUnlockedMessages();

        StringBuilder sb = new StringBuilder();
        foreach (string msg in unlocked)
        {
            Debug.Log(msg);
            sb.AppendLine(msg);
        }

        messagesText.text = sb.ToString();
    }
    public void RefreshUI()
    {
        messagesText.text = "";

        var messages = MessageManager.Instance.GetUnlockedMessages();

        foreach (string msg in messages)
        {
            messagesText.text += msg + "\n";
        }
    }
}
