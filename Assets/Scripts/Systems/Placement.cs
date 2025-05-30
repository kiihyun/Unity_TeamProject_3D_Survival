using UnityEngine;

public class PlacementPreview : MonoBehaviour
{
    public PlayerInteraction playerInteraction; // 플레이어의 Ray 정보 사용
    private GameObject ghostObject;
    private ItemData currentItem;
    private bool isPlacementValid;

    public void StartPlacing(ItemData item)
    {
        if (!item.canPlace) return;

        currentItem = item;

        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = Instantiate(item.placeablePrefab);
        MakeTransparent(ghostObject);
    }

    void Update()
    {
        if (ghostObject == null || currentItem == null || !playerInteraction.hasHit) return;

        if (playerInteraction.lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector3 pos = playerInteraction.lastHit.point;
            Vector3 normal = playerInteraction.lastHit.normal;

            ghostObject.transform.position = pos;
            isPlacementValid = ValidatePlacement(pos, normal);
            float slope = Vector3.Angle(normal, Vector3.up);
            Debug.Log($"[현재 설치 위치 경사] slope: {slope}도");

            UpdatePreviewColor(isPlacementValid);

            if (Input.GetMouseButtonDown(0))
            {
                ConfirmPlacement();
            }
        }


    }

    bool ValidatePlacement(Vector3 pos, Vector3 normal)
    {

        float slope = Vector3.Angle(normal, Vector3.up);
        if (slope > 25f) return false;

        // ghostObject의 BoxCollider 기준으로 정확한 extents 계산
        BoxCollider collider = ghostObject.GetComponentInChildren<BoxCollider>();
        if (collider == null)
        {
            Debug.LogWarning("BoxCollider가 없습니다.");
            return false;
        }

        Vector3 halfExtents = collider.size * 0.5f;
        Quaternion rotation = ghostObject.transform.rotation;
        int layerMask = ~(1 << LayerMask.NameToLayer("IgnorePlacementCheck")); // 고스트 오브젝트에 해당하는 레이어만 무시해서 설치 여부를 판단

        Collider[] overlaps = Physics.OverlapBox(
            pos + collider.center,
            halfExtents,
            rotation,
            layerMask
            );

        Debug.Log($"[Overlap Check] Found {overlaps.Length} colliders");

        foreach (Collider c in overlaps)
        {
            Debug.Log($"겹침 오브젝트: {c.name}, isTrigger: {c.isTrigger}");
            if (c.gameObject == ghostObject) continue; // 자기 자신이면 무시
            if (c.gameObject.GetComponent<Terrain>() != null) continue; // Terrain도 무시

            if (!c.isTrigger) return false;
        }

        return true;
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

    public void ConfirmPlacement()
    {//
        if (ghostObject == null || !isPlacementValid || currentItem == null) return;

        Instantiate(currentItem.dropPrefab, ghostObject.transform.position, Quaternion.identity);
        Destroy(ghostObject);
        Inventory.Instance.RemoveItem(currentItem, 1);
        Debug.Log($"{currentItem.name}설치완료");
        //ghostObject = null;
        //currentItem = null;

        // 인벤토리가 변경될 때마다 UI 새로고침
        
    }
}
