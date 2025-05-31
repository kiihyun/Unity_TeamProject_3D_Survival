using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public CinemachineVirtualCamera npcCamera;
    public CinemachineVirtualCamera playerCamera;

    public GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    private Queue<string> sentences;
    private string currentSentence;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private PlayerController playerController;

    private HashSet<string> seenStroyDialogues = new HashSet<string>(); // 스토리 대화 중복 방지용

    void Start()
    {
        sentences = new Queue<string>();
        nextButton.onClick.AddListener(OnNextClicked);

        playerController = FindObjectOfType<PlayerController>();

        dialogueUI.SetActive(false);
    }

    public void TryStartDialogue(DialogueData dialogue, Transform npcTarget)
    {
        if (dialogue.dialogueType == DialogueType.Story)
        {
            if (seenStroyDialogues.Contains(dialogue.dialogueId))
                return;
            seenStroyDialogues.Add(dialogue.dialogueId);
        }
        else if (dialogue.dialogueType == DialogueType.Quest)
        {
            //Quest quest = QuestManager.Instance.GetQuestById(dialogue.requiredQuestId);
            //if (quest == null || quest.CurrentState != dialogue.requiredQuestState)
            //    return;
        }
        else if (dialogue.dialogueType == DialogueType.Tip)
        {
            // 팁은 랜덤 대사 한 줄만 출력
            string ramdomLine = dialogue.dialogueLines[Random.Range(0, dialogue.dialogueLines.Length)];
            StartDialogue(dialogue.npcName, new string[] { ramdomLine }, npcTarget);
            return;
        }
        StartDialogue(dialogue.npcName, dialogue.dialogueLines, npcTarget);
    }

    public void StartDialogue(string npcName, string[] dialogueLines, Transform npcTarget)
    {
        // 플레이어 카메라를 비활성화하고 NPC 카메라를 활성화
        npcCamera.Follow = npcTarget;
        npcCamera.LookAt = npcTarget;

        playerCamera.gameObject.SetActive(false);
        npcCamera.gameObject.SetActive(true);

        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);

        UIManager.Instance.ShowDialogueUI(); // 모든 비게임 UI 꺼지고 대화 UI만 켜짐

        nameText.text = npcName;

        sentences.Clear();

        foreach (string line in dialogueLines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    void OnNextClicked()
    {
        if (isTyping)
        {
            // 현재 타이핑 중인 경우, 즉시 전체 문장을 표시
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentSentence; // 전체 문장 표시
                typingCoroutine = null;
            }
        }
        else
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f); // 글자 하나 출력 후 대기
        }

        typingCoroutine = null;
    }

    public void EndDialogue()
    {
        UIManager.Instance.HideDialogueUI(); // 다시 UI 복구

        // 플레이어 컨트롤러 활성화
        playerController?.SetControl(true);

        // 카메라 전환: NPC 카메라 비활성화, 플레이어 카메라 활성화
        npcCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    public bool HasSeenDialogue(string dialogueId)
    {
        return seenStroyDialogues.Contains(dialogueId);
    }
}
