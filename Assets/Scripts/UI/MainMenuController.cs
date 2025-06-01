using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject loadButton;
    public GameObject SettingButton;
    public GameObject exitButton;
    public GameObject creditButton;
    
    public GameObject mainMenuPanel; // 메인 메뉴 패널 참조
    public GameObject settingsPanel; // 설정 패널 참조

    [Header("Fade Settings")]
    public Image fadePanel;
    public float fadeStartAlpha = 0.3f;
    public float fadeEndAlpha = 1f;
    public float fadeDuration = 1f;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void OnClickNewGame()
    {
        mainMenuPanel.gameObject.SetActive(true); // 페이드 패널 활성화
        fadePanel.gameObject.SetActive(true); // 페이드 패널 활성화

        StartCoroutine(PlayIntroSequence());
    }

    public void OnClickLoadGame()
    {
        // 로드 게임 로직을 여기에 추가
        Debug.Log("Load Game clicked");
    }
    public void OnClickSettings()
    {
        settingsPanel.SetActive(true); // 설정 패널 활성화
        mainMenuPanel.SetActive(false); // 메인 메뉴 패널 비활성화
    }
        
    public void OnClickExitGame()
    {
        #if UNITY_EDITOR
            Debug.Log("Exit Game clicked");
            // 에디터에서는 플레이 모드 종료
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 빌드된 게임에서는 애플리케이션 종료
            Application.Quit();
        #endif
    }

    public void OnClickCredit()
    {
        // 크레딧 로직을 여기에 추가
        Debug.Log("Credit clicked");
        SceneManager.LoadScene("CreditScene"); // 크레딧 씬 로드
    }

    public void OnClickBackToMainMenu()
    {
        settingsPanel.SetActive(false); // 설정 패널 비활성화
        mainMenuPanel.SetActive(true); // 메인 메뉴 패널 활성화
    }

    IEnumerator PlayIntroSequence()
    {
        // 1. 페이드 인
        yield return StartCoroutine(FadeIn());

        // 2. 잠깐 대기
        yield return new WaitForSeconds(1f);

        // 3. 인트로 씬으로 전환
        SceneManager.LoadScene("IntroScene");
    }


    IEnumerator FadeIn()
    {
        Color color = fadePanel.color;
        color.a = fadeStartAlpha;
        fadePanel.color = color;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fadeStartAlpha, fadeEndAlpha, timer / fadeDuration);
            color.a = alpha;
            fadePanel.color = color;
            yield return null;
        }
    }
}
