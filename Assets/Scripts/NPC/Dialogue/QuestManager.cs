using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<string, QuestState> questStates = new();

    void Awake()
    {
        Instance = this;
    }

    public bool CheckQuestState(string questId, QuestState requiredState)
    {
        if (!questStates.ContainsKey(questId)) return false;
        return questStates[questId] == requiredState;
    }

    public void SetQuestState(string questId, QuestState state)
    {
        questStates[questId] = state;
    }
}