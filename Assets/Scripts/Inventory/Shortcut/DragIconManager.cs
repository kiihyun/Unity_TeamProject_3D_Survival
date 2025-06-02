using UnityEngine;
using UnityEngine.UI;

public class DragIconManager : MonoBehaviour
{
    //드래그할 때 아이콘이 따라다니게 관리하는 아이콘을 관리하는 스크립트입니다.
    public static DragIconManager Instance { get; private set; }

    public Canvas canvas;
    public Image ghostImage;     // UI Image (유령 아이콘)
    private RectTransform ghostRect;

    private void Awake()
    {
        Instance = this;
        ghostRect = ghostImage.GetComponent<RectTransform>();
        ghostImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostImage.enabled)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out pos
            );
            ghostRect.anchoredPosition = pos;
        }
    }


    public void ShowGhost(Sprite sprite)
    {
        ghostImage.sprite = sprite;
        ghostImage.enabled = true;
    }

    public void HideGhost()
    {
        ghostImage.enabled = false;
    }

}
