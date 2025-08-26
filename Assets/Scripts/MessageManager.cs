using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    [TextArea]
    public List<string> allMessages = new List<string>();
    private List<string> remainingMessages = new List<string>();
    private List<string> unlockedMessages = new List<string>();
    private string unlockedMessagesPath;
    private string savePath;

    // when the game starts, we will assign the savePath and
    // load all remaining messages in our queue (json file)
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "messages.json");
            LoadRemainingMessages();
            LoadUnlockedMessages();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // function to reset messages
    public void ResetMessages()
    {
        // creating a new remaining messages list with every message
        remainingMessages = new List<string>(allMessages);

        // shuffling it
        Shuffle(remainingMessages);

        //saving this new, populated remaining messages list
        SaveRemainingMessages();
    }

    // gets the next message in the list
    public string GetNextMessage()
    {
        // checks if we've gone through all the messages and resets if so
        if (remainingMessages.Count == 0)
        {
            ResetMessages();
        }

        // gets the message, then removes it from the remaining messages list
        string msg = remainingMessages[0];
        remainingMessages.RemoveAt(0);

        unlockedMessages.Add(msg);
        SaveUnlockedMessages();

        // saves the new remaining messages and then returns the current message
        SaveRemainingMessages();
        return msg;
    }

    // shuffles the list by looping through it and switching elements at indices around
    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // function to save the remaining messages
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

    // function to load the remaining messages
    // does this by checking if the file at the specified path exists, then reads the file 
    // and saves this in a variable, which is then used to create a MessageListWrapper variable
    // we create the remaining messages from that wrapper variable
    // if the file doesnt exist, it could be the user's first time opening the program 
    // it will reset the messages, which creates a new messages lists, shuffles, then saves it
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

    private void SaveUnlockedMessages()
    {
        // wrapping messages in a class because List cant be converted directly into JSON
        MessageListWrapper wrapper = new MessageListWrapper();
        wrapper.messages = unlockedMessages;
        // converting to JSON
        string json = JsonUtility.ToJson(wrapper, true);
        // defining a path to put messages
        string unlockedMessagesPath = Path.Combine(Application.persistentDataPath, "unlockedMessages.json");
        // writing to the messages.json file with the newly converted JSON content
        File.WriteAllText(unlockedMessagesPath, json);
    }

    private void LoadUnlockedMessages()
    {
        string unlockedMessagesPath = Path.Combine(Application.persistentDataPath, "unlockedMessages.json");
        if (File.Exists(unlockedMessagesPath))
        {
            string json = File.ReadAllText(unlockedMessagesPath);
            MessageListWrapper wrapper = JsonUtility.FromJson<MessageListWrapper>(json);
            unlockedMessages = new List<string>(wrapper.messages);
        }
        else
        {
            unlockedMessages = new List<string>();
        }
    }

    // we need this wrapper class so that we can convert this object into json
    // since we cant directly convert a list into json
    // Lists can only be converted into a json list through serialization
    [System.Serializable]
    public class MessageListWrapper
    {
        public List<string> messages;
    }

    public List<string> GetUnlockedMessages()
    {
        return new List<string>(unlockedMessages); // copy so external scripts can’t modify the original
    }

}
