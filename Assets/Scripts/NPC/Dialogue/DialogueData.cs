using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string dialogueId; // 유니크 ID (스토리 대사 추적용)
    public string npcName;

    public DialogueType dialogueType; // 스토리 / 퀘스트 / 팁

    [Header("퀘스트 조건 (퀘스트 대사용)")]
    public string requiredQuestId; // 퀘스트 ID
    public QuestState requiredQuestState;

    [TextArea(2, 5)]
    public string[] dialogueLines;
}

public enum DialogueType
{
    Story,
    Quest,
    Tip
}

public enum QuestState
{
    None,
    Started,
    Completed
}
