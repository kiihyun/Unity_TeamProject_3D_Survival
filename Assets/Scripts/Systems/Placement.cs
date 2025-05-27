using UnityEngine;

public class PlacementPreview : MonoBehaviour
{
    public PlayerInteraction playerInteraction;
    private GameObject ghostObject;
    private PlaceableItem currentItem;
    private bool isPlacementValid;

    void Update()
    {
        if (ghostObject == null || currentItem == null) return;
        if (!playerInteraction.hasHit) return;

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

        Collider[] overlaps = Physics.OverlapBox(pos, ghostObject.transform.localScale / 2);
        foreach (Collider c in overlaps)
        {
            if (!c.isTrigger) return false;
        }

        return true;
    }

    void UpdatePreviewColor(bool valid) { /* »ý·« */ }
}
