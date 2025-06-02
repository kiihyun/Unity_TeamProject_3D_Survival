using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public string targetSceneName = "GameScene";

    public TMP_Text loadingText;
    public Slider loadingBar;

    void Start()
    {
        loadingBar.value = 0f;
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(targetSceneName);
        asyncOp.allowSceneActivation = false;

        float progress = 0f;

        while (!asyncOp.isDone)
        {
            progress = Mathf.Clamp01(asyncOp.progress / 0.9f); // 최대 0.9

            // 실제 게이지는 올리되 텍스트는 99%까지만
            loadingBar.value = progress;

            int displayProgress = Mathf.FloorToInt(progress * 100f);
            if (displayProgress >= 100) displayProgress = 99;
            loadingText.text = $"Loading... {displayProgress}%";

            // 씬 전환 시점에서 100% 표시
            if (progress >= 1f)
            {
                loadingText.text = "Loading... 100%";
                yield return new WaitForSeconds(0.5f);
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
