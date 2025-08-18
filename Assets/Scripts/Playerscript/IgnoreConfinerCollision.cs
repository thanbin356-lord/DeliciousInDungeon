using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Cinemachine;

public class IgnoreConfinerCollision : MonoBehaviour
{
    public Unity.Cinemachine.CinemachineConfiner cinemachineConfiner;

    void Start()
    {
        // Lấy Collider của Cinemachine Confiner
        Collider2D confinerCollider = cinemachineConfiner.GetComponent<Collider2D>();

        // Lấy Collider của các đối tượng khác (ví dụ: Tilemap)
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            Collider2D tilemapCollider = tilemap.GetComponent<Collider2D>();

            // Bỏ qua va chạm giữa Collider của Cinemachine Confiner và Collider của Tilemap
            if (tilemapCollider != null)
            {
                Physics2D.IgnoreCollision(confinerCollider, tilemapCollider, true);
            }
        }
    }
}