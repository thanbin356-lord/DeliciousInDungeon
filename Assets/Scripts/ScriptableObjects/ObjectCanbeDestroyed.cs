using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCanbeDestroyed : MonoBehaviour
{
    Animator animator;

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                IsDestroyed();
            }
        }
        get
        {
            return health;
        }
    }

    public float health = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IsDestroyed()
    {
        animator.SetTrigger("IsDestroyed");
    }
    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
