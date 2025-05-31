using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject loadButton;
    public GameObject SettingButton;
    public GameObject exitButton;
    public GameObject creditButton;

    public GameObject mainMenuPanel; // 메인 메뉴 패널 참조
    public GameObject settingsPanel; // 설정 패널 참조

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void OnClickNewGame()
    {
        SceneManager.LoadScene("IntroScene"); // 이름으로 로드
        // 또는 Index로 로드: SceneManager.LoadScene(1);
    }

    public void OnClickLoadGame()
    {
        // 로드 게임 로직을 여기에 추가
        Debug.Log("Load Game clicked");
    }
    public void OnClickSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // 설정 패널 토글
        mainMenuPanel.SetActive(!settingsPanel.activeSelf); // 메인 메뉴 패널 숨김/보임

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

    public void OnConfirmBtn()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // 설정 패널 토글
        mainMenuPanel.SetActive(!settingsPanel.activeSelf); // 메인 메뉴 패널 숨김/보임
    }
}
