using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PausedController : MonoBehaviour
{
    public GameObject backButton;
    public GameObject optionsButton;
    public GameObject quitButton;

    public GameObject pausePanel; // Pause UI 패널
    public GameObject optionPanel; // 옵션 UI 패널

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OnClickbackButton()
    {
        ResumeGame(); // 게임 재개
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 시간 재개
        pausePanel.SetActive(false); // UI 비활성화
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 잠금
    }

    public void OnClickOptionsButton()
    {
        pausePanel.SetActive(false); // 일시정지 패널 비활성화
        optionPanel.SetActive(true); // 옵션 패널 활성화
    }

    public void OnClickQuitButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
