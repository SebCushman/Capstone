using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;

    private void Update()
    {
        if ((FindObjectOfType<Player>().transform.position - transform.position).magnitude <= 2 && !FindObjectOfType<Player>().isDialogue && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<Player>().isDialogue = true;
            CommenceDialogue();
        }
    }

    public void CommenceDialogue()
    {
        FindObjectOfType<DialogueManager>().StartConversation(dialogue);
    }
}
