using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
