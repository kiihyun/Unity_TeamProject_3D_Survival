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


    void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void OnClickNewGame()
    {
        SceneManager.LoadScene("IntroScene"); // 이름으로 로드
        // 또는 Index로 로드: SceneManager.LoadScene(1);
    }
}
