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
    private bool skipTyping = false;

    private HashSet<string> seenStroyDialogues = new HashSet<string>();

    void Start()
    {
        sentences = new Queue<string>();
        nextButton.onClick.AddListener(OnNextClicked);
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (dialogueUI.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            OnNextClicked();
        }
    }

    public void TryStartDialogue(DialogueData dialogue, Transform npcTarget)
    {
        if (dialogue.dialogueType == DialogueType.Story)
        {
            if (seenStroyDialogues.Contains(dialogue.dialogueId))
                return;
            seenStroyDialogues.Add(dialogue.dialogueId);
        }

        if (dialogue.dialogueType == DialogueType.Quest)
        {
            // 퀘스트 조건 확인 로직 필요 시 여기에 추가
        }

        if (dialogue.dialogueType == DialogueType.Tip)
        {
            string randomLine = dialogue.dialogueLines[Random.Range(0, dialogue.dialogueLines.Length)];
            StartDialogue(dialogue.npcName, new string[] { randomLine }, npcTarget);
            return;
        }

        StartDialogue(dialogue.npcName, dialogue.dialogueLines, npcTarget);
    }

    public void StartDialogue(string npcName, string[] dialogueLines, Transform npcTarget)
    {
        UIManager.Instance.ShowDialogueUI();

        npcCamera.Follow = npcTarget;
        npcCamera.LookAt = npcTarget;

        playerCamera.gameObject.SetActive(false);
        npcCamera.gameObject.SetActive(true);

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
            skipTyping = true;
        }
        else
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

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
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            if (skipTyping)
            {
                dialogueText.text = sentence;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    public void EndDialogue()
    {
        UIManager.Instance.HideDialogueUI();

        npcCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    public bool HasSeenDialogue(string dialogueId)
    {
        return seenStroyDialogues.Contains(dialogueId);
    }
}
