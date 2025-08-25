using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    [TextArea]
    public List<string> allMessages = new List<string>();
    private List<string> remainingMessages = new List<string>();

    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "messages.json");
            LoadRemainingMessages();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetMessages()
    {
        remainingMessages = new List<string>(allMessages);
        Shuffle(remainingMessages);
        SaveRemainingMessages();
    }

    public string GetNextMessage()
    {
        if (remainingMessages.Count == 0)
        {
            ResetMessages();
        }

        string msg = remainingMessages[0];
        remainingMessages.RemoveAt(0);

        SaveRemainingMessages();
        return msg;
    }

    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void SaveRemainingMessages()
    {
        // wrapping messages in a class because List cant be converted directly into JSON
        MessageListWrapper wrapper = new MessageListWrapper();
        wrapper.messages = remainingMessages;
        // converting to JSON
        string json = JsonUtility.ToJson(wrapper, true);
        // defining a path to put messages
        string savePath = Path.Combine(Application.persistentDataPath, "messages.json");
        // writing to the messages.json file with the newly converted JSON content
        File.WriteAllText(savePath, json);
    }

    private void LoadRemainingMessages()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "messages.json");
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            MessageListWrapper wrapper = JsonUtility.FromJson<MessageListWrapper>(json);
            remainingMessages = new List<string>(wrapper.messages);
        }
        else
        {
            ResetMessages();
        }
    }

    public class MessageListWrapper
    {
        public List<string> messages;
    }
}


