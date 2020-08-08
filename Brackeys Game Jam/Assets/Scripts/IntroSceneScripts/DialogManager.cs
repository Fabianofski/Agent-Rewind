using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogText;
    public InputMaster1 controls;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }
    private void Awake()
    {
        controls = new InputMaster1();
        controls.IntroScene.nextText.performed += _ => DisplayNextSentence();
    }


    public void StartDialog(Dialog dialog)
    {
        nameText.text = dialog.name;

        sentences.Clear();

        foreach(string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        IEnumerator TypeSentence(string sentences)
        {
            dialogText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogText.text += letter;
                yield return null;
            }
        }

        void EndDialog()
        {
            Debug.Log("End of conversation");
            FindObjectOfType<LevelLoader>().LoadNextLevel();
        }

    }
    void OnEnable()
    {
        controls.Enable();
    }
}