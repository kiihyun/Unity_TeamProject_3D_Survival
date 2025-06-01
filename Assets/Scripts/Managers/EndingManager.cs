using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("Text Settings")]
    public TMP_Text introText;
    [TextArea]
    public string[] lines; // 출력할 문장들
    public float typingSpeed = 0.005f;
    public float delayBetweenLines = 0.1f;

    [Header("After Intro")]
    public float waitAfterText = 0.3f;
    public string nextSceneName;

    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
                skipTyping = true;
        }
    }

    IEnumerator PlayIntroSequence()
    {
        introText.text = "";

        foreach (string line in lines)
        {
            yield return StartCoroutine(TypeLine(line));

            // 다음 줄로 넘어가기 전에 클릭 대기
            while (!Input.GetMouseButtonDown(0))
                yield return null;

            yield return new WaitForSeconds(0.05f); // 빠르게 연속 클릭 방지
        }

        yield return new WaitForSeconds(waitAfterText);
#if UNITY_EDITOR
        Debug.Log("Exit Game clicked");
        // 에디터에서는 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 빌드된 게임에서는 애플리케이션 종료
            Application.Quit();
#endif
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        skipTyping = false;
        string currentLine = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (skipTyping)
            {
                currentLine = line;
                introText.text += line.Substring(i); // 남은 텍스트 한 번에 추가
                break;
            }

            currentLine += line[i];
            introText.text += line[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        introText.text += "\n"; // 줄 바꿈
        isTyping = false;
    }
}
