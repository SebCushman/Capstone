using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text sentenceText;
    public GameObject textBox;

    public Queue<string> conversation;
    public float timer;
    public float refresh;

    void Start()
    {
        conversation = new Queue<string>();
        timer = 1f;
        textBox.SetActive(false);
    }

    private void Update()
    {
        if(timer > refresh)
        {
            refresh += Time.deltaTime;
        }

        if (FindObjectOfType<Player>().isDialogue && refresh >= timer && Input.GetKeyDown(KeyCode.E))
        {
            refresh = 0;
            DisplayNextSentence();
        }
    }

    public void StartConversation(Dialogue dialogue)
    {
        //Debug.Log($"Talking to {dialogue.NPC_name}");
        nameText.text = dialogue.NPC_name;

        conversation.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            conversation.Enqueue(sentence);
        }
        textBox.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(conversation.Count == 0)
        {
            EndConversation();
            return;
        }

        refresh = 0;
        string sentence = conversation.Dequeue();
        //sentenceText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        //Debug.Log(sentence);
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";

        foreach (var letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return null;
        }
    }

    public void EndConversation()
    {
        FindObjectOfType<Player>().isDialogue = false;
        textBox.SetActive(false);
        //Debug.Log("Finished conversation");
    }
}
