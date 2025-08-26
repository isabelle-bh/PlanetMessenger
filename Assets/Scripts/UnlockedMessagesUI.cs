using UnityEngine;
using TMPro;
using System.Text;

public class UnlockedMessagesUI : MonoBehaviour
{
    public TextMeshProUGUI messagesText; // Reference to one TMP text component

    void Update()
    {
        var unlocked = MessageManager.Instance.GetUnlockedMessages();

        // Build a combined string
        StringBuilder sb = new StringBuilder();
        foreach (string msg in unlocked)
        {
            sb.AppendLine(msg); // Adds a newline after each message
        }

        // Set all unlocked messages into the one text box
        messagesText.text = sb.ToString();
    }
}
