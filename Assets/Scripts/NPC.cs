using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public bool hasQuest = false;
    public Quest quest;
    public Player player;
    public GameObject questWindow;
    public Text questName;
    public Text questDescription;
    public Text questReward;

    public GameObject questIndicator;

    private void Start()
    {
        dialogue.owner = this;
        player = FindObjectOfType<Player>();

        if(quest != null)
        {
            hasQuest = true;
            questIndicator.SetActive(true);
        }

        if(quest.title == "")
        {
            hasQuest = false;
            questIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= 2 && !FindObjectOfType<Player>().isDialogue && Input.GetKeyDown(KeyCode.E))
        {
            player.isDialogue = true;
            CommenceDialogue();
        }

        //if (player.isDialogue)
        //{
        //    if (Input.GetKeyDown(KeyCode.Y))
        //    {
        //        AcceptQuest();
        //    }
        //    else if (Input.GetKeyDown(KeyCode.N))
        //    {
        //        DeclineQuest();
        //    }
        //}
    }

    public void CommenceDialogue()
    {
        FindObjectOfType<DialogueManager>().StartConversation(dialogue);
        //if (hasQuest && !player.isDialogue)
        //{
        //    OpenQuestWindow();
        //}
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        questName.text = quest.title;
        questDescription.text = quest.description;
        questReward.text = $"{quest.xpReward} xp";
    }

    public void AcceptQuest()
    {
        quest.isActive = true;
        questWindow.SetActive(false);
        player.quests = quest;
        player.isDialogue = false;
        Debug.Log($"Title: {quest.title}");
        Debug.Log($"Description: {quest.description}");
        Debug.Log($"Reward: {quest.xpReward}");
    }
    public void DeclineQuest()
    {
        questWindow.SetActive(false);
        player.isDialogue = false;
    }
}
