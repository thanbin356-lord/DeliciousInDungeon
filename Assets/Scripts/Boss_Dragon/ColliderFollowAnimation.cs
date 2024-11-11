using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollowAnimation : MonoBehaviour
{
    [SerializeField] private List<PolygonCollider2D> colliderSteps;
    [SerializeField] private PolygonCollider2D rootCollider;
    [SerializeField] private int colliderStep = 0;
    public void Advance()
    {
        PolygonCollider2D next = colliderSteps[colliderStep];
        Vector2[] tempArray = (Vector2[])next.points.Clone();
        rootCollider.SetPath(0, tempArray);
    }
}
