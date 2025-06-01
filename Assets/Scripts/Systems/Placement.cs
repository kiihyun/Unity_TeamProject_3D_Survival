using UnityEngine;

public class PlacementPreview : MonoBehaviour
{
    public PlayerInteraction playerInteraction; // 레이캐스트 정보
    private GameObject ghostObject;
    private ItemData currentItem;
    private bool isPlacementValid;
    private bool clickedThisFrame = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedThisFrame = true;
        }

        if (ghostObject == null || currentItem == null || !playerInteraction.hasHit)
        {
            clickedThisFrame = false;
            return;
        }

        // Ground 레이어 위일 때만 설치 가능
        if (playerInteraction.lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector3 hitPos = playerInteraction.lastHit.point;

            // 위치를 항상 땅 위로 고정
            PositionGhostOnGround(hitPos);

            isPlacementValid = true;
            UpdatePreviewColor(isPlacementValid);

            if (clickedThisFrame && isPlacementValid)
            {
                Debug.Log("설치 시도됨!");
                ConfirmPlacement();
            }
        }

        clickedThisFrame = false;
    }

    public void StartPlacing(ItemData item)
    {
        if (!item.canPlace) return;

        currentItem = item;

        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = Instantiate(item.placeablePrefab);
        MakeTransparent(ghostObject);

        // 물리 영향 제거
        foreach (var rb in ghostObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (var col in ghostObject.GetComponentsInChildren<Collider>())
        {
            col.isTrigger = true;
        }
    }

    void PositionGhostOnGround(Vector3 hitPos)
    {
        // 박스 콜라이더 기준 높이 보정
        BoxCollider col = ghostObject.GetComponentInChildren<BoxCollider>();
        if (col != null)
        {
            float yOffset = col.size.y / 2f;
            Vector3 correctedPos = new Vector3(hitPos.x, hitPos.y + yOffset, hitPos.z);
            ghostObject.transform.position = correctedPos;
        }
        else
        {
            // 콜라이더 없으면 그냥 놓음
            ghostObject.transform.position = hitPos;
        }
    }

    void ConfirmPlacement()
    {
        if (ghostObject == null || !isPlacementValid || currentItem == null) return;

        Instantiate(currentItem.dropPrefab, ghostObject.transform.position, Quaternion.identity);
        Inventory.Instance.RemoveItem(currentItem, 1);
        Debug.Log($"{currentItem.name} 설치 완료!");

        Destroy(ghostObject);
        currentItem = null;
        isPlacementValid = false;
    }

    void UpdatePreviewColor(bool valid)
    {
        Color c = valid ? new Color(0f, 0.5f, 1f, 0.4f) : new Color(1f, 0f, 0f, 0.4f);

        foreach (var r in ghostObject.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                mat.color = c;
            }
        }
    }

    void MakeTransparent(GameObject obj)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }
}
