using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public Animator anim;
    public Button nextButton;
    public bool sentenceFinished;
    public bool fast = false;
    Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        anim.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
            sentenceFinished = false;
            if(fast == true)
            {
                dialogueText.text = sentence;
                break;
            }
        }
        if(dialogueText.text == sentence)
        {
            sentenceFinished = true;
        }    
    }
    void EndDialogue()
    {
        anim.SetBool("IsOpen", false);
    }

    private void Update() {
        if(anim.GetBool("IsOpen") == true)
        {
            if(Input.GetKeyDown(KeyCode.Space) && sentenceFinished == true)
            {
                nextButton.onClick.Invoke();
                fast = false;
            }
            if(Input.GetKeyDown(KeyCode.Space) && sentenceFinished == false)
            {
                fast = true;
            }

        }
    }

}
