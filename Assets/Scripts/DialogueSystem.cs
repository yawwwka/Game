using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private float textSpeed;
    [SerializeField] private Text dialogueText;
    [SerializeField] private string[] lines;
    private Button button;

    public string[] Lines { 
        get
        {
            return lines;
        }
        set 
        {
            lines = value;
        } 
    }

    Character character;

    private int index;

    private void Awake()
    {
        character = FindAnyObjectByType<Character>();
        dialogueText = GetComponentInChildren<Text>();
        button = FindAnyObjectByType<Button>();
        button.onClick.AddListener(() => SkipTextByClick());
    }

    private void Start()
    {
        dialogueText.text = string.Empty;
    }

    private void OnEnable()
    {
        character.InDialogue = true;
        StartDialogue();
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void SkipTextByClick()
    {
        if (dialogueText.text == lines[index])
        {
            NextLines();
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = lines[index];
        }
    }

    private void NextLines()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            character.InDialogue = false;
            Destroy(gameObject);
        }
    }
}
