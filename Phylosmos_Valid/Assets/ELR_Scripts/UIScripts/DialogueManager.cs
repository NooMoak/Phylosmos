using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    [SerializeField] GameObject analyseText;
    public Animator anim;
    public Button nextButton;
    public bool sentenceFinished;
    public bool fast = false;
    bool hasUIText;
    string UIText;
    Queue<string> sentences;
    Queue<string> names;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        anim.SetBool("IsOpen", true);
        sentences.Clear();
        names.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }
        hasUIText = dialogue.hasUIText;
        UIText = dialogue.UIText;

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
        string name = names.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
    }

    IEnumerator TypeSentence (string sentence, string name)
    {
        dialogueText.text = "";
        nameText.text = name;
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
        if(hasUIText)
        {
            analyseText.SetActive(true);
            analyseText.GetComponent<Text>().text = UIText;
            StartCoroutine(DisableText());
        }
    }

    private void Update() {
        if(anim.GetBool("IsOpen") == true)
        {
            if(Input.GetKeyDown(KeyCode.Return) && sentenceFinished == true)
            {
                nextButton.onClick.Invoke();
                fast = false;
            }
            if(Input.GetKeyDown(KeyCode.Return) && sentenceFinished == false)
            {
                fast = true;
            }

        }
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds (5);
        analyseText.SetActive(false);
    }

}
