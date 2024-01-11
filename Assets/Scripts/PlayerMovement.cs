using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerMovement : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private Vector3 moveDir;

    private void Update()
    {
        if (!IsOwner) return;


        moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.y = 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1f;

        animator.SetFloat("Speed", Math.Abs(moveDir.x) + Math.Abs(moveDir.y));

        float moveSpeed = 3f;
        transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        moveDir = new Vector3(0, 0, 0);
        animator.SetFloat("Speed", Math.Abs(moveDir.x) + Math.Abs(moveDir.y));
    }
}
