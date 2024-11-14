using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class TimelineManager : MonoBehaviour
{
    private bool fix = false;
    [SerializeField] private Animator playerAnim;
    private RuntimeAnimatorController playerContr;
    [SerializeField] private PlayableDirector director;

    [SerializeField] private GameObject cow;
    [SerializeField] private GameObject bird;
    private Character character;
    private DialogueSystem dialogue;

    private void Awake()
    {
        character = FindAnyObjectByType<Character>();
        dialogue = Resources.Load<DialogueSystem>("DialogueBackground");
    }

    void OnEnable()
    {
        character.InDialogue = true;
        playerContr = playerAnim.runtimeAnimatorController;
        playerAnim.runtimeAnimatorController = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && !fix)
        {
            fix = true;
            playerAnim.runtimeAnimatorController = playerContr;

            DialogueSystem newDialogue = Instantiate(dialogue, FindFirstObjectByType<Canvas>().transform);
            newDialogue.Lines = new string[2];
            newDialogue.Lines[0] = "Игорян: Черт, этот ворон украл мою любимую игрушку!";
            newDialogue.Lines[1] = "Игорян: Нужно догнать его и вернуть мою игрушку.";
            Destroy(cow);
            Destroy(bird);
            Destroy(gameObject);
            Debug.Log("123");
        }
    }
}