using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using Cinemachine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;

    private float previousPosX;


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;
        } 
        else
        {
            vc.Priority = 0;
        }
    }

    private void Update()
    {
        // GLOBAL


        //PLAYER FLIPPING
        if ((transform.position.x - previousPosX) < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if ((transform.position.x - previousPosX) > 0)
        {
            spriteRenderer.flipX = false;
        }
        previousPosX = transform.position.x;


        // PLAYER


        //PLAYER CONTROLLER
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.y = 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1f;

        animator.SetFloat("Speed", Math.Abs(moveDir.x) + Math.Abs(moveDir.y));

        float moveSpeed = 3f;
        transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;


    }
}
