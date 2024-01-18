using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private float destroyTime = 2.5f;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.otherCollider.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            anim.SetTrigger("Impact");
            Debug.Log("collision!");
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
