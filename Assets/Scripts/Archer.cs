using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Archer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        spriteRenderer.sprite = transform.GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sprite;
        animator.runtimeAnimatorController = transform.GetChild(2).GetChild(1).GetComponent<Animator>().runtimeAnimatorController;
    }
}
