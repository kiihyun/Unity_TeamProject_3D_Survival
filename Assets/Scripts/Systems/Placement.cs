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

        ghostObject = Instantiate(item.dropPrefab);
        MakeTransparent(ghostObject);
    }

    void Update()
    {
        if (ghostObject == null || currentItem == null || !playerInteraction.hasHit) return;

        Vector3 pos = playerInteraction.lastHit.point;
        Vector3 normal = playerInteraction.lastHit.normal;

        ghostObject.transform.position = pos;
        isPlacementValid = ValidatePlacement(pos, normal);

        UpdatePreviewColor(isPlacementValid);
    }

    bool ValidatePlacement(Vector3 pos, Vector3 normal)
    {
        float slope = Vector3.Angle(normal, Vector3.up);
        if (slope > 25f) return false;

        Collider[] overlaps = Physics.OverlapBox(pos, ghostObject.transform.localScale / 2, ghostObject.transform.rotation);
        foreach (Collider c in overlaps)
        {
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
    {
        if (ghostObject == null || !isPlacementValid || currentItem == null) return;

        Instantiate(currentItem.dropPrefab, ghostObject.transform.position, Quaternion.identity);
        Destroy(ghostObject);
        ghostObject = null;
        currentItem = null;
    }
}
