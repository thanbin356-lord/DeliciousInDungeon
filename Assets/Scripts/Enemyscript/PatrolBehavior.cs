using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    public float patrolRangeX = 2f;
    public float patrolRangeY = 1f;
    public float waypointTolerance = 0.1f;
    public float moveSpeed = 1f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        SetNewTargetPosition();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        Vector2 direction = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        animator.SetTrigger("IsRunning");

        // Flip based on movement direction
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (Vector2.Distance(transform.position, targetPosition) < waypointTolerance)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(startPosition.x - patrolRangeX / 2, startPosition.x + patrolRangeX / 2);
        float randomY = Random.Range(startPosition.y - patrolRangeY / 2, startPosition.y + patrolRangeY / 2);
        targetPosition = new Vector2(randomX, randomY);
    }
}
