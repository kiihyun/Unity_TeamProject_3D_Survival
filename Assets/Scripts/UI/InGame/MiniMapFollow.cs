using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public Vector3 offset = new Vector3(0, 20, 0); // 미니맵 카메라 높이
    public bool rotateWithPlayer = true; // 플레이어 회전에 따라 카메라 회전 여부

    void LateUpdate()
    {
        if (player == null) return;

        // 플레이어 바로 위 위치로 이동
        Vector3 newPos = player.position + offset;
        transform.position = newPos;

        if (rotateWithPlayer)
        {
            // 플레이어 방향에 따라 카메라 Y축 회전 맞추기
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
        else
        {
            // 고정된 위쪽 방향 (Y축 0)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
