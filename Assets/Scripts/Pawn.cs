using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        spriteRenderer.sprite = transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite;
        animator.runtimeAnimatorController = transform.GetChild(2).GetChild(0).GetComponent<Animator>().runtimeAnimatorController;
    }

    // GATHERING AND BUILDING
}
