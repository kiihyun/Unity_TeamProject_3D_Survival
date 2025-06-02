using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Tooltip("NPC가 가진 여러 대사들")]
    public List<DialogueData> dialogueDataList;

    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void StartDialogue()
    {
        if (dialogueManager == null || dialogueDataList == null || dialogueDataList.Count == 0)
            return;

        DialogueData selectedDialogue = SelectDialogue();

        if (selectedDialogue != null)
        {
            dialogueManager.TryStartDialogue(selectedDialogue, transform);
        }
    }

    private DialogueData SelectDialogue()
    {
        // 예시: 조건에 따라 고르기 (우선순위: Story > Quest > Tip)
        foreach (var dialogue in dialogueDataList)
        {
            if (dialogue.dialogueType == DialogueType.Story &&
                !dialogueManager.HasSeenDialogue(dialogue.dialogueId))
            {
                return dialogue;
            }
        }

        foreach (var dialogue in dialogueDataList)
        {
            if (dialogue.dialogueType == DialogueType.Quest &&
                QuestManager.Instance.CheckQuestState(dialogue.requiredQuestId, dialogue.requiredQuestState))
            {
                return dialogue;
            }
        }

        // Tip은 랜덤하게 하나 선택
        List<DialogueData> tipDialogues = dialogueDataList.FindAll(d => d.dialogueType == DialogueType.Tip);
        if (tipDialogues.Count > 0)
        {
            return tipDialogues[Random.Range(0, tipDialogues.Count)];
        }

        return null;
    }
}
