using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueTrigger trigger;
    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Da cham");
        if (collision.gameObject.CompareTag("Player") == true)
            trigger.StartDialogue();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        trigger.EndDialogue();
    }
}
